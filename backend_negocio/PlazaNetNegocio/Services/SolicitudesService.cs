using PlazaNetNegocio.DTOs;
using PlazaNetNegocio.Models;
using PlazaNetNegocio.Repositories;

namespace PlazaNetNegocio.Services
{
    public class SolicitudesService : ISolicitudesService
    {
        private readonly ISolicitudesRepository _repo;

        private static readonly HashSet<string> TiposValidos =
            new(new[] { "basico", "pro", "full" });

        private static readonly HashSet<string> EstadosValidos =
            new(new[] { "pendiente", "aprobada", "rechazada" });

        public SolicitudesService(ISolicitudesRepository repo)
        {
            _repo = repo;
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

                entity.Estado = dto.Estado;
            }

            var ok = await _repo.UpdateAsync(entity);
            if (!ok) return null;

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
    }
}
