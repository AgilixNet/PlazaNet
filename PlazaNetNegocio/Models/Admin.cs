using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlazaNetNegocio.Models
{
    [Table("admins")]
    public class Admin
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } // UUID en Postgres ↔ Guid en C#

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } // timestamp with time zone

        [Required]
        [Column("email")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = "";

        [Required]
        [Column("nombre_plaza")]
        public string NombrePlaza { get; set; } = "";

        [Required]
        [Column("nombre_representante")]
        public string NombreRepresentante { get; set; } = "";

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Required]
        [Column("tipo_suscripcion")]
        public string TipoSuscripcion { get; set; } = ""; // 'basico' | 'pro' | 'full'

        [Column("fecha_expiracion")]
        public DateTimeOffset? FechaExpiracion { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = "activo"; // 'activo' | 'suspendido' | 'cancelado'

        // Relación opcional con la solicitud que lo originó
        [Column("solicitud_id")]
        public Guid? SolicitudId { get; set; }

        [ForeignKey("SolicitudId")]
        public Solicitud? Solicitud { get; set; }
    }
}
