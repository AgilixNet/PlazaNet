using PlazaNetNegocio.DTOs;

namespace PlazaNetNegocio.Services
{
    public interface ISolicitudesService
    {
        Task<List<SolicitudReadDTO>> GetAllAsync();
        Task<SolicitudReadDTO?> GetByIdAsync(Guid id);
        Task<SolicitudReadDTO> CreateAsync(SolicitudCreateDTO dto);
        Task<SolicitudReadDTO?> UpdateAsync(Guid id, SolicitudUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
