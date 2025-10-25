using PlazaNetNegocio.Models;

namespace PlazaNetNegocio.Repositories
{
    public interface ISolicitudesRepository
    {
        Task<List<Solicitud>> GetAllAsync();
        Task<Solicitud?> GetByIdAsync(Guid id);
        Task<Solicitud> CreateAsync(Solicitud nueva);
        Task<bool> UpdateAsync(Solicitud actualizada);
        Task<bool> DeleteAsync(Guid id);
    }
}
