using Microsoft.EntityFrameworkCore;
using PlazaNetNegocio.Data;
using PlazaNetNegocio.Models;

namespace PlazaNetNegocio.Repositories
{
    public class SolicitudesRepository : ISolicitudesRepository
    {
        private readonly AppDbContext _context;

        public SolicitudesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Solicitud>> GetAllAsync()
        {
            return await _context.Solicitudes
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Solicitud?> GetByIdAsync(Guid id)
        {
            return await _context.Solicitudes.FindAsync(id);
        }

        public async Task<Solicitud> CreateAsync(Solicitud nueva)
        {
            _context.Solicitudes.Add(nueva);
            await _context.SaveChangesAsync(); // aquí se rellenan Id y CreatedAt si vienen por default desde la BD
            return nueva;
        }

        public async Task<bool> UpdateAsync(Solicitud actualizada)
        {
            _context.Solicitudes.Update(actualizada);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Solicitudes.FindAsync(id);
            if (entity == null) return false;

            _context.Solicitudes.Remove(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}
