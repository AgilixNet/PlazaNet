namespace PlazaNetNegocio.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Envía un correo de notificación al administrador de la plaza
        /// informando que la solicitud ha sido aprobada y debe proceder con el pago.
        /// </summary>
        Task SendSolicitudAprobadaEmailAsync(
            string emailDestinatario,
            string nombreRepresentante,
            string nombrePlaza,
            string tipoSuscripcion);

        /// <summary>
        /// Envía un correo con las credenciales de acceso al nuevo administrador.
        /// </summary>
        Task SendCredencialesEmailAsync(
            string emailDestinatario,
            string nombreRepresentante,
            string nombrePlaza,
            string password);
    }
}
