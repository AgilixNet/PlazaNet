using PlazaNetNegocio.Models;

namespace PlazaNetNegocio.Repositories
{
    public interface IAdminsRepository
    {
        Task<List<Admin>> GetAllAsync();
        Task<Admin?> GetByIdAsync(Guid id);
        Task<Admin?> GetByEmailAsync(string email);
        Task<Admin> CreateAsync(Admin admin);
        Task<bool> UpdateAsync(Admin admin);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
    }
}
