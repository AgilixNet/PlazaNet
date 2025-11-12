using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PlazaNetNegocio.Services
{
    public class SupabaseService : ISupabaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SupabaseService> _logger;
        private readonly string _supabaseUrl;
        private readonly string _supabaseServiceKey;
        private readonly string _supabaseAnonKey;

        public SupabaseService(
            HttpClient httpClient, 
            IConfiguration configuration,
            ILogger<SupabaseService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            
            _supabaseUrl = _configuration["Supabase:Url"] ?? "";
            _supabaseServiceKey = _configuration["Supabase:ServiceKey"] ?? "";
            _supabaseAnonKey = _configuration["Supabase:AnonKey"] ?? "";
        }

        public async Task<(bool success, string? userId, string? plazaId, string? error)> CrearAdminPlazaCompleto(
            string email,
            string password,
            string nombreRepresentante,
            string nombrePlaza,
            string? telefono,
            Guid solicitudId)
        {
            try
            {
                // 1. Crear usuario en Supabase Auth
                var userId = await CrearUsuarioAuth(email, password);
                if (userId == null)
                {
                    return (false, null, null, "Error al crear usuario en Supabase Auth");
                }

                _logger.LogInformation($"Usuario creado en Auth: {userId}");

                // 2. Crear plaza
                var plazaId = await CrearPlaza(nombrePlaza, email, telefono);
                if (plazaId == null)
                {
                    return (false, userId, null, "Error al crear plaza");
                }

                _logger.LogInformation($"Plaza creada: {plazaId}");

                // 3. Crear perfil vinculado
                var perfilCreado = await CrearPerfil(userId, nombreRepresentante, email, plazaId);
                if (!perfilCreado)
                {
                    return (false, userId, plazaId, "Error al crear perfil");
                }

                _logger.LogInformation($"Perfil creado para usuario {userId}");

                return (true, userId, plazaId, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear admin de plaza completo");
                return (false, null, null, ex.Message);
            }
        }

        private async Task<string?> CrearUsuarioAuth(string email, string password)
        {
            try
            {
                var request = new
                {
                    email = email,
                    password = password,
                    email_confirm = true // Auto-confirmar email
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_supabaseUrl}/auth/v1/admin/users");
                httpRequest.Headers.Add("apikey", _supabaseServiceKey);
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseServiceKey);
                httpRequest.Content = content;

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al crear usuario en Auth: {responseContent}");
                    return null;
                }

                var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
                return result.GetProperty("id").GetString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al crear usuario en Auth");
                return null;
            }
        }

        private async Task<string?> CrearPlaza(string nombrePlaza, string email, string? telefono)
        {
            try
            {
                var plaza = new
                {
                    nombre = nombrePlaza,
                    ciudad = "", // Se puede actualizar después
                    ubicacion = "", // Se puede actualizar después
                    telefono = telefono ?? "",
                    email = email,
                    estado = "activo"
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(plaza),
                    Encoding.UTF8,
                    "application/json"
                );

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_supabaseUrl}/rest/v1/plazas");
                httpRequest.Headers.Add("apikey", _supabaseServiceKey); // Usar SERVICE KEY para bypass RLS
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseServiceKey);
                httpRequest.Headers.Add("Prefer", "return=representation");
                httpRequest.Content = content;

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al crear plaza: {responseContent}");
                    return null;
                }

                var result = JsonSerializer.Deserialize<JsonElement[]>(responseContent);
                if (result != null && result.Length > 0)
                {
                    return result[0].GetProperty("id").GetString();
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al crear plaza");
                return null;
            }
        }

        private async Task<bool> CrearPerfil(string userId, string nombre, string correo, string plazaId)
        {
            try
            {
                var perfil = new
                {
                    id = userId, // Mismo ID que auth.users
                    nombre = nombre,
                    rol = "Owner",
                    correo = correo,
                    plaza_id = plazaId
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(perfil),
                    Encoding.UTF8,
                    "application/json"
                );

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_supabaseUrl}/rest/v1/perfiles");
                httpRequest.Headers.Add("apikey", _supabaseServiceKey); // Usar SERVICE KEY para bypass RLS
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseServiceKey);
                httpRequest.Headers.Add("Prefer", "return=representation");
                httpRequest.Content = content;

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al crear perfil: {responseContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al crear perfil");
                return false;
            }
        }
    }
}
