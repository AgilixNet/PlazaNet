namespace PlazaNetNegocio.DTOs
{
    public class SolicitudUpdateDTO
    {
        public string? NombreRepresentante { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? NombrePlaza { get; set; }
        public string? TipoSuscripcion { get; set; } // basico/pro/full
        public string? CedulaUrl { get; set; }
        public string? RutUrl { get; set; }
        public string? Estado { get; set; } // pendiente/aprobada/rechazada
    }
}
