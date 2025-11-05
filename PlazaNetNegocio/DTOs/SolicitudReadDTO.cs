using System;

namespace PlazaNetNegocio.DTOs
{
    public class SolicitudReadDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string NombreRepresentante { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Telefono { get; set; }
        public string NombrePlaza { get; set; } = "";
        public string TipoSuscripcion { get; set; } = "";
        public string? CedulaUrl { get; set; }
        public string? RutUrl { get; set; }
        public string Estado { get; set; } = "";
    }
}

