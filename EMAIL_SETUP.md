# Configuración de Email para PlazaNet

## ✅ Implementación Completada

Se ha implementado el envío automático de correos electrónicos cuando una solicitud es aprobada.

## 📋 Archivos Creados/Modificados

1. **Services/IEmailService.cs** - Interfaz del servicio de email
2. **Services/EmailService.cs** - Implementación del servicio de email con MailKit
3. **Services/SolicitudesService.cs** - Modificado para enviar email al aprobar solicitudes
4. **Program.cs** - Registrado EmailService en el contenedor de DI
5. **appsettings.json** - Agregada configuración de email

## 🔧 Configuración Requerida

### Opción 1: Gmail (Recomendado para desarrollo)

1. **Habilitar verificación en 2 pasos** en tu cuenta de Gmail
2. **Generar una contraseña de aplicación**:
   - Ve a https://myaccount.google.com/security
   - Busca "Contraseñas de aplicaciones"
   - Genera una nueva contraseña para "Correo"
   - Copia la contraseña de 16 caracteres

3. **Actualizar `appsettings.json`**:
```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "tu-email@gmail.com",
    "SmtpPassword": "xxxx xxxx xxxx xxxx",  // Contraseña de aplicación
    "FromEmail": "tu-email@gmail.com",
    "FromName": "PlazaNet"
  }
}
```

### Opción 2: Outlook/Hotmail

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp-mail.outlook.com",
    "SmtpPort": "587",
    "SmtpUser": "tu-email@outlook.com",
    "SmtpPassword": "tu-contraseña",
    "FromEmail": "tu-email@outlook.com",
    "FromName": "PlazaNet"
  }
}
```

### Opción 3: SendGrid (Recomendado para producción)

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.sendgrid.net",
    "SmtpPort": "587",
    "SmtpUser": "apikey",
    "SmtpPassword": "TU_API_KEY_DE_SENDGRID",
    "FromEmail": "noreply@tudominio.com",
    "FromName": "PlazaNet"
  }
}
```

## 🚀 Cómo Funciona

1. Cuando se actualiza una solicitud mediante `PUT /api/solicitudes/{id}`
2. Si el `estado` cambia de cualquier valor a `"aprobada"`
3. Se envía automáticamente un correo al email del representante
4. El correo incluye:
   - Nombre del representante
   - Nombre de la plaza
   - Tipo de suscripción
   - Instrucciones para proceder con el pago

## 📧 Ejemplo de Uso

```bash
# Aprobar una solicitud (esto enviará el email)
PUT /api/solicitudes/[guid-de-solicitud]
Content-Type: application/json

{
  "estado": "aprobada"
}
```

## 🔒 Seguridad

**IMPORTANTE**: 
- Nunca subas credenciales de email a GitHub
- Usa variables de entorno en producción
- Considera usar `appsettings.Development.json` para desarrollo local
- En producción, configura las credenciales en Azure App Settings o similar

### Ejemplo con Variables de Entorno:

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "${EMAIL_USER}",
    "SmtpPassword": "${EMAIL_PASSWORD}",
    "FromEmail": "${EMAIL_FROM}",
    "FromName": "PlazaNet"
  }
}
```

## 🧪 Pruebas

1. Crea una solicitud
2. Actualiza su estado a "aprobada"
3. Verifica que el correo llegue al email del representante
4. Revisa los logs para confirmar el envío

## 💡 Sobre Pagos y Suscripciones

**No se recomienda usar Supabase para procesamiento de pagos** directamente. Para pagos, considera:

### Opciones de Pasarelas de Pago:

1. **Stripe** (Recomendado globalmente)
   - Fácil integración con .NET
   - Manejo de suscripciones
   - Webhooks para confirmación de pago

2. **PayPal**
   - Ampliamente usado
   - API bien documentada

3. **MercadoPago** (Para Latinoamérica)
   - Popular en la región
   - Soporte local

4. **Wompi/PayU** (Para Colombia)
   - Procesadores locales
   - Integración con bancos colombianos

### Flujo Recomendado:

1. ✅ Solicitud aprobada → Email enviado (IMPLEMENTADO)
2. Usuario recibe email con link de pago
3. Usuario paga mediante pasarela (Stripe/PayPal/etc.)
4. Webhook confirma el pago
5. Sistema actualiza estado de suscripción
6. Usuario recibe credenciales de acceso

## 📚 Próximos Pasos Sugeridos

1. [ ] Integrar Stripe/PayPal para pagos
2. [ ] Crear tabla de suscripciones en la BD
3. [ ] Implementar webhooks para confirmación de pago
4. [ ] Enviar email con credenciales después del pago
5. [ ] Implementar recordatorios de renovación de suscripción
