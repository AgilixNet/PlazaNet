using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlazaNetNegocio.Models
{
    [Table("solicitudes")]
    public class Solicitud
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } // UUID en Postgres ↔ Guid en C#

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } // timestamp with time zone

        [Required]
        [Column("nombre_representante")]
        public string NombreRepresentante { get; set; } = "";

        [Required]
        [Column("email")]
        public string Email { get; set; } = "";

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Required]
        [Column("nombre_plaza")]
        public string NombrePlaza { get; set; } = "";

        [Required]
        [Column("tipo_suscripcion")]
        public string TipoSuscripcion { get; set; } = ""; // 'basico' | 'pro' | 'full'

        [Column("cedula_url")]
        public string? CedulaUrl { get; set; }

        [Column("rut_url")]
        public string? RutUrl { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = "pendiente"; // 'pendiente' | 'aprobada' | 'rechazada'
    }
}
