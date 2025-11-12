using PlazaNetNegocio.DTOs;
using PlazaNetNegocio.Models;
using PlazaNetNegocio.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace PlazaNetNegocio.Services
{
    public class SolicitudesService : ISolicitudesService
    {
        private readonly ISolicitudesRepository _repo;
        private readonly IAdminsRepository _adminsRepo;
        private readonly IEmailService _emailService;
        private readonly ISupabaseService _supabaseService;
        private readonly ILogger<SolicitudesService> _logger;

        private static readonly HashSet<string> TiposValidos =
            new(new[] { "basico", "pro", "full" });

        private static readonly HashSet<string> EstadosValidos =
            new(new[] { "pendiente", "aprobada", "rechazada" });

        public SolicitudesService(
            ISolicitudesRepository repo,
            IAdminsRepository adminsRepo,
            IEmailService emailService,
            ISupabaseService supabaseService,
            ILogger<SolicitudesService> logger)
        {
            _repo = repo;
            _adminsRepo = adminsRepo;
            _emailService = emailService;
            _supabaseService = supabaseService;
            _logger = logger;
        }

        public async Task<List<SolicitudReadDTO>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.Select(MapToReadDTO).ToList();
        }

        public async Task<SolicitudReadDTO?> GetByIdAsync(Guid id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return null;
            return MapToReadDTO(s);
        }

        public async Task<SolicitudReadDTO> CreateAsync(SolicitudCreateDTO dto)
        {
            if (!TiposValidos.Contains(dto.TipoSuscripcion))
                throw new ArgumentException("tipo_suscripcion inválido. Use basico, pro o full.");

            var entity = new Solicitud
            {
                // Id se genera en DB
                // CreatedAt se genera en DB
                NombreRepresentante = dto.NombreRepresentante,
                Email = dto.Email,
                Telefono = dto.Telefono,
                NombrePlaza = dto.NombrePlaza,
                TipoSuscripcion = dto.TipoSuscripcion,
                CedulaUrl = dto.CedulaUrl,
                RutUrl = dto.RutUrl,
                Estado = "pendiente" // estado inicial fijo
            };

            var creada = await _repo.CreateAsync(entity);
            return MapToReadDTO(creada);
        }

        public async Task<SolicitudReadDTO?> UpdateAsync(Guid id, SolicitudUpdateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            // Guardar el estado anterior para detectar cambios
            var estadoAnterior = entity.Estado;

            // solo pisamos campos que llegaron
            if (dto.NombreRepresentante != null)
                entity.NombreRepresentante = dto.NombreRepresentante;

            if (dto.Email != null)
                entity.Email = dto.Email;

            if (dto.Telefono != null)
                entity.Telefono = dto.Telefono;

            if (dto.NombrePlaza != null)
                entity.NombrePlaza = dto.NombrePlaza;

            if (dto.TipoSuscripcion != null)
            {
                if (!TiposValidos.Contains(dto.TipoSuscripcion))
                    throw new ArgumentException("tipo_suscripcion inválido. Use basico, pro o full.");

                entity.TipoSuscripcion = dto.TipoSuscripcion;
            }

            if (dto.CedulaUrl != null)
                entity.CedulaUrl = dto.CedulaUrl;

            if (dto.RutUrl != null)
                entity.RutUrl = dto.RutUrl;

            if (dto.Estado != null)
            {
                if (!EstadosValidos.Contains(dto.Estado))
                    throw new ArgumentException("estado inválido. Use pendiente, aprobada o rechazada.");

                // Validar que no se pueda cambiar de "aprobada" a otro estado
                if (estadoAnterior == "aprobada" && dto.Estado != "aprobada")
                    throw new ArgumentException("No se puede cambiar el estado de una solicitud aprobada. Una vez aprobada, la solicitud no puede ser modificada.");

                entity.Estado = dto.Estado;
            }

            var ok = await _repo.UpdateAsync(entity);
            if (!ok) return null;

            // Si el estado cambió a "aprobada", crear admin completo en Supabase
            if (estadoAnterior != "aprobada" && entity.Estado == "aprobada")
            {
                try
                {
                    // Verificar si ya existe un admin con este email
                    var adminExistente = await _adminsRepo.GetByEmailAsync(entity.Email);
                    
                    if (adminExistente == null)
                    {
                        // Generar contraseña aleatoria segura
                        string passwordGenerada = GenerarPasswordSegura();
                        
                        // 1. Crear usuario en Supabase Auth, plaza y perfil
                        var (success, userId, plazaId, error) = await _supabaseService.CrearAdminPlazaCompleto(
                            entity.Email,
                            passwordGenerada,
                            entity.NombreRepresentante,
                            entity.NombrePlaza,
                            entity.Telefono,
                            entity.Id
                        );

                        if (!success)
                        {
                            _logger.LogError(
                                "Error al crear admin en Supabase para solicitud {Id}: {Error}",
                                entity.Id,
                                error);
                            throw new Exception($"Error al crear admin en Supabase: {error}");
                        }

                        _logger.LogInformation(
                            "Admin creado en Supabase - UserId: {UserId}, PlazaId: {PlazaId}",
                            userId,
                            plazaId);

                        // 2. Crear registro local en tabla admins
                        var nuevoAdmin = new Admin
                        {
                            Email = entity.Email,
                            PasswordHash = HashPassword(passwordGenerada),
                            NombrePlaza = entity.NombrePlaza,
                            NombreRepresentante = entity.NombreRepresentante,
                            Telefono = entity.Telefono,
                            TipoSuscripcion = entity.TipoSuscripcion,
                            Estado = "activo",
                            SolicitudId = entity.Id,
                            FechaExpiracion = CalcularFechaExpiracion(entity.TipoSuscripcion)
                        };

                        await _adminsRepo.CreateAsync(nuevoAdmin);

                        _logger.LogInformation(
                            "Admin local creado exitosamente para solicitud {Id} - Email: {Email}",
                            entity.Id,
                            entity.Email);

                        // 3. Enviar email con credenciales
                        await _emailService.SendCredencialesEmailAsync(
                            entity.Email,
                            entity.NombreRepresentante,
                            entity.NombrePlaza,
                            passwordGenerada);

                        _logger.LogInformation(
                            "Email de credenciales enviado para solicitud {Id} - Plaza: {Plaza}",
                            entity.Id,
                            entity.NombrePlaza);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Ya existe un admin con el email {Email} para la solicitud {Id}",
                            entity.Email,
                            entity.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error al crear admin o enviar credenciales para solicitud {Id}. La solicitud fue aprobada pero la creación del admin falló.",
                        entity.Id);
                    // No lanzamos excepción para que la actualización se complete
                    // Se puede reintentar la creación del admin manualmente
                }
            }

            return MapToReadDTO(entity);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return _repo.DeleteAsync(id);
        }

        // --------- helper mapping -----------
        private static SolicitudReadDTO MapToReadDTO(Solicitud s)
        {
            return new SolicitudReadDTO
            {
                Id = s.Id,
                CreatedAt = s.CreatedAt,
                NombreRepresentante = s.NombreRepresentante,
                Email = s.Email,
                Telefono = s.Telefono,
                NombrePlaza = s.NombrePlaza,
                TipoSuscripcion = s.TipoSuscripcion,
                CedulaUrl = s.CedulaUrl,
                RutUrl = s.RutUrl,
                Estado = s.Estado
            };
        }

        // --------- helper password -----------
        private static string GenerarPasswordSegura()
        {
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%&*";
            var password = new char[12];
            var random = RandomNumberGenerator.Create();
            var bytes = new byte[12];
            random.GetBytes(bytes);

            for (int i = 0; i < 12; i++)
            {
                password[i] = caracteres[bytes[i] % caracteres.Length];
            }

            return new string(password);
        }

        private static string HashPassword(string password)
        {
            // Usar BCrypt o similar en producción
            // Por ahora usamos un hash simple con salt
            using var sha256 = SHA256.Create();
            var saltedPassword = password + "PlazaNetSalt2025"; // En producción usar un salt único por usuario
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(bytes);
        }

        private static DateTimeOffset CalcularFechaExpiracion(string tipoSuscripcion)
        {
            // Calcular fecha de expiración según el tipo de suscripción
            // Por defecto 30 días
            return DateTimeOffset.UtcNow.AddDays(30);
        }
    }
}
