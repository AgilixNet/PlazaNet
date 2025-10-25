using System.ComponentModel.DataAnnotations;

namespace PlazaNetNegocio.DTOs
{
    public class SolicitudCreateDTO
    {
        [Required]
        public string NombreRepresentante { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string? Telefono { get; set; }

        [Required]
        public string NombrePlaza { get; set; } = "";

        // debe ser "basico", "pro" o "full"
        [Required]
        public string TipoSuscripcion { get; set; } = "";

        public string? CedulaUrl { get; set; }
        public string? RutUrl { get; set; }
    }
}
