using Microsoft.EntityFrameworkCore;
using PlazaNetNegocio.Models;

namespace PlazaNetNegocio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Solicitud> Solicitudes { get; set; }

        // Esto es opcional, pero útil si quieres tunear el mapeo.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Solicitud>(entity =>
            {
                // La tabla ya existe. Solo marcamos la PK.
                entity.HasKey(e => e.Id);

                // valores por defecto (la BD ya los tiene).
                entity.Property(e => e.Estado)
                      .HasDefaultValue("pendiente");

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("timezone('utc'::text, now())");

                entity.Property(e => e.Id)
                      .HasDefaultValueSql("gen_random_uuid()");
            });
        }
    }
}
