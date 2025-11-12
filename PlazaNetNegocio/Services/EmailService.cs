using MailKit.Net.Smtp;
using MimeKit;

namespace PlazaNetNegocio.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendSolicitudAprobadaEmailAsync(
            string emailDestinatario,
            string nombreRepresentante,
            string nombrePlaza,
            string tipoSuscripcion)
        {
            try
            {
                var message = new MimeMessage();

                // Remitente
                var fromEmail = _config["EmailSettings:FromEmail"] ?? "noreply@plazanet.com";
                var fromName = _config["EmailSettings:FromName"] ?? "PlazaNet";
                message.From.Add(new MailboxAddress(fromName, fromEmail));

                // Destinatario
                message.To.Add(new MailboxAddress(nombreRepresentante, emailDestinatario));

                // Asunto
                message.Subject = "Solicitud Aprobada - Proceder con el Pago de Suscripción";

                // Cuerpo del mensaje
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = GenerarCuerpoHtml(nombreRepresentante, nombrePlaza, tipoSuscripcion),
                    TextBody = GenerarCuerpoTexto(nombreRepresentante, nombrePlaza, tipoSuscripcion)
                };

                message.Body = bodyBuilder.ToMessageBody();

                // Enviar correo
                using var client = new SmtpClient();

                var smtpHost = _config["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"] ?? "587");
                var smtpUser = _config["EmailSettings:SmtpUser"];
                var smtpPass = _config["EmailSettings:SmtpPassword"];

                await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                if (!string.IsNullOrEmpty(smtpUser) && !string.IsNullOrEmpty(smtpPass))
                {
                    await client.AuthenticateAsync(smtpUser, smtpPass);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation(
                    "Email enviado exitosamente a {Email} para la plaza {Plaza}",
                    emailDestinatario,
                    nombrePlaza);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email a {Email}", emailDestinatario);
                throw;
            }
        }

        private string GenerarCuerpoHtml(string nombreRepresentante, string nombrePlaza, string tipoSuscripcion)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f9f9f9; }}
        .footer {{ padding: 20px; text-align: center; font-size: 12px; color: #666; }}
        .highlight {{ background-color: #fff3cd; padding: 10px; border-left: 4px solid #ffc107; margin: 20px 0; }}
        .plan {{ font-weight: bold; color: #4CAF50; text-transform: uppercase; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>¡Solicitud Aprobada!</h1>
        </div>
        <div class='content'>
            <p>Estimado/a <strong>{nombreRepresentante}</strong>,</p>
            
            <p>Nos complace informarle que su solicitud de suscripción para <strong>{nombrePlaza}</strong> ha sido aprobada exitosamente.</p>
            
            <div class='highlight'>
                <p><strong>Plan seleccionado:</strong> <span class='plan'>{tipoSuscripcion}</span></p>
            </div>
            
            <h3>Próximos pasos:</h3>
            <ol>
                <li>Proceder con el pago de la suscripción correspondiente</li>
                <li>Una vez confirmado el pago, recibirá las credenciales de acceso</li>
                <li>Podrá comenzar a utilizar todos los beneficios de PlazaNet</li>
            </ol>
            
            <p>Para realizar el pago, por favor comuníquese con nuestro equipo de soporte o acceda al portal de pagos.</p>
            
            <p>Si tiene alguna pregunta, no dude en contactarnos.</p>
            
            <p>Atentamente,<br>
            <strong>Equipo PlazaNet</strong></p>
        </div>
        <div class='footer'>
            <p>&copy; 2025 PlazaNet. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GenerarCuerpoTexto(string nombreRepresentante, string nombrePlaza, string tipoSuscripcion)
        {
            return $@"
¡Solicitud Aprobada!

Estimado/a {nombreRepresentante},

Nos complace informarle que su solicitud de suscripción para {nombrePlaza} ha sido aprobada exitosamente.

Plan seleccionado: {tipoSuscripcion.ToUpper()}

Próximos pasos:
1. Proceder con el pago de la suscripción correspondiente
2. Una vez confirmado el pago, recibirá las credenciales de acceso
3. Podrá comenzar a utilizar todos los beneficios de PlazaNet

Para realizar el pago, por favor comuníquese con nuestro equipo de soporte o acceda al portal de pagos.

Si tiene alguna pregunta, no dude en contactarnos.

Atentamente,
Equipo PlazaNet

---
© 2025 PlazaNet. Todos los derechos reservados.
";
        }

        public async Task SendCredencialesEmailAsync(
            string emailDestinatario,
            string nombreRepresentante,
            string nombrePlaza,
            string password)
        {
            try
            {
                var message = new MimeMessage();

                // Remitente
                var fromEmail = _config["EmailSettings:FromEmail"] ?? "noreply@plazanet.com";
                var fromName = _config["EmailSettings:FromName"] ?? "PlazaNet";
                message.From.Add(new MailboxAddress(fromName, fromEmail));

                // Destinatario
                message.To.Add(new MailboxAddress(nombreRepresentante, emailDestinatario));

                // Asunto
                message.Subject = "¡Bienvenido a PlazaNet! - Credenciales de Acceso";

                // Cuerpo del mensaje
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = GenerarCuerpoCredencialesHtml(nombreRepresentante, nombrePlaza, emailDestinatario, password),
                    TextBody = GenerarCuerpoCredencialesTexto(nombreRepresentante, nombrePlaza, emailDestinatario, password)
                };

                message.Body = bodyBuilder.ToMessageBody();

                // Enviar correo
                using var client = new SmtpClient();

                var smtpHost = _config["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"] ?? "587");
                var smtpUser = _config["EmailSettings:SmtpUser"];
                var smtpPass = _config["EmailSettings:SmtpPassword"];

                await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                if (!string.IsNullOrEmpty(smtpUser) && !string.IsNullOrEmpty(smtpPass))
                {
                    await client.AuthenticateAsync(smtpUser, smtpPass);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation(
                    "Email de credenciales enviado exitosamente a {Email} para la plaza {Plaza}",
                    emailDestinatario,
                    nombrePlaza);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email de credenciales a {Email}", emailDestinatario);
                throw;
            }
        }

        private string GenerarCuerpoCredencialesHtml(string nombreRepresentante, string nombrePlaza, string email, string password)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f9f9f9; }}
        .footer {{ padding: 20px; text-align: center; font-size: 12px; color: #666; }}
        .credentials {{ background-color: #e3f2fd; padding: 20px; border-radius: 8px; margin: 20px 0; }}
        .credential-item {{ margin: 10px 0; }}
        .credential-label {{ font-weight: bold; color: #1976D2; }}
        .credential-value {{ font-family: monospace; background-color: white; padding: 8px 12px; border-radius: 4px; display: inline-block; margin-left: 10px; }}
        .warning {{ background-color: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 20px 0; }}
        .button {{ background-color: #2196F3; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>¡Bienvenido a PlazaNet!</h1>
        </div>
        <div class='content'>
            <p>Estimado/a <strong>{nombreRepresentante}</strong>,</p>
            
            <p>¡Su cuenta de administrador ha sido creada exitosamente para <strong>{nombrePlaza}</strong>!</p>
            
            <p>A continuación encontrará sus credenciales de acceso al sistema:</p>
            
            <div class='credentials'>
                <div class='credential-item'>
                    <span class='credential-label'>📧 Usuario (Email):</span>
                    <span class='credential-value'>{email}</span>
                </div>
                <div class='credential-item'>
                    <span class='credential-label'>🔑 Contraseña:</span>
                    <span class='credential-value'>{password}</span>
                </div>
            </div>
            
            <div class='warning'>
                <strong>⚠️ Importante:</strong>
                <ul style='margin: 10px 0;'>
                    <li>Guarde esta contraseña en un lugar seguro</li>
                    <li>Le recomendamos cambiar su contraseña después del primer inicio de sesión</li>
                    <li>No comparta sus credenciales con nadie</li>
                </ul>
            </div>
            
            <p>Para acceder a su panel de administración:</p>
            <ol>
                <li>Ingrese a la plataforma PlazaNet</li>
                <li>Use el email y contraseña proporcionados arriba</li>
                <li>Complete la configuración de su perfil</li>
            </ol>
            
            <a href='#' class='button'>Iniciar Sesión</a>
            
            <p style='margin-top: 30px;'>Si tiene alguna pregunta o necesita asistencia, no dude en contactarnos.</p>
            
            <p>Atentamente,<br>
            <strong>Equipo PlazaNet</strong></p>
        </div>
        <div class='footer'>
            <p>&copy; 2025 PlazaNet. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GenerarCuerpoCredencialesTexto(string nombreRepresentante, string nombrePlaza, string email, string password)
        {
            return $@"
¡Bienvenido a PlazaNet!

Estimado/a {nombreRepresentante},

¡Su cuenta de administrador ha sido creada exitosamente para {nombrePlaza}!

A continuación encontrará sus credenciales de acceso al sistema:

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
CREDENCIALES DE ACCESO
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📧 Usuario (Email): {email}
🔑 Contraseña: {password}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

⚠️ IMPORTANTE:
- Guarde esta contraseña en un lugar seguro
- Le recomendamos cambiar su contraseña después del primer inicio de sesión
- No comparta sus credenciales con nadie

Para acceder a su panel de administración:
1. Ingrese a la plataforma PlazaNet
2. Use el email y contraseña proporcionados arriba
3. Complete la configuración de su perfil

Si tiene alguna pregunta o necesita asistencia, no dude en contactarnos.

Atentamente,
Equipo PlazaNet

---
© 2025 PlazaNet. Todos los derechos reservados.
";
        }
    }
}
