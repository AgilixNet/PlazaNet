using System;
using System.Threading.Tasks;

namespace PlazaNetNegocio.Services
{
    public interface ISupabaseService
    {
        Task<(bool success, string? userId, string? plazaId, string? error)> CrearAdminPlazaCompleto(
            string email,
            string password,
            string nombreRepresentante,
            string nombrePlaza,
            string? telefono,
            Guid solicitudId
        );
    }
}
