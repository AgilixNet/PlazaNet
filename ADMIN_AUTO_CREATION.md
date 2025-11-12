# 🚀 Creación Automática de Administradores - PlazaNet

## ✅ Funcionalidad Implementada

Cuando una solicitud es **aprobada** (estado cambia a `"aprobada"`), el sistema automáticamente:

1. ✅ Crea un usuario administrador en la tabla `admins`
2. ✅ Genera una contraseña segura aleatoria
3. ✅ Envía un email con las credenciales al administrador

## 📋 Archivos Modificados/Creados

### Nuevos Archivos:
1. **Models/Admin.cs** - Modelo de datos para administradores
2. **Repositories/IAdminsRepository.cs** - Interfaz del repositorio
3. **Repositories/AdminsRepository.cs** - Implementación del repositorio
4. **create_admins_table.sql** - Script SQL para crear la tabla en Supabase

### Archivos Modificados:
1. **Services/IEmailService.cs** - Agregado método `SendCredencialesEmailAsync`
2. **Services/EmailService.cs** - Implementación del email de credenciales
3. **Services/SolicitudesService.cs** - Lógica para crear admin al aprobar
4. **Data/AppDbContext.cs** - Agregado DbSet de Admins
5. **Program.cs** - Registrado AdminsRepository

## 🗄️ Configuración de Base de Datos

### 1. Crear la tabla `admins` en Supabase

Ve a tu proyecto en Supabase → SQL Editor y ejecuta el script `create_admins_table.sql`:

```sql
CREATE TABLE IF NOT EXISTS public.admins (
    id UUID DEFAULT gen_random_uuid() PRIMARY KEY,
    created_at TIMESTAMPTZ DEFAULT timezone('utc'::text, now()) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(500) NOT NULL,
    nombre_plaza VARCHAR(255) NOT NULL,
    nombre_representante VARCHAR(255) NOT NULL,
    telefono VARCHAR(50),
    tipo_suscripcion VARCHAR(50) NOT NULL,
    fecha_expiracion TIMESTAMPTZ,
    estado VARCHAR(50) DEFAULT 'activo' NOT NULL,
    solicitud_id UUID REFERENCES public.solicitudes(id) ON DELETE SET NULL
);
```

## 📧 Configuración de Email

Asegúrate de configurar las credenciales SMTP en `appsettings.json`:

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "tu-email@gmail.com",
    "SmtpPassword": "tu-password-de-aplicacion",
    "FromEmail": "noreply@plazanet.com",
    "FromName": "PlazaNet"
  }
}
```

## 🎯 Flujo de Trabajo

### Paso 1: Crear una Solicitud
```bash
POST /api/solicitudes
{
  "nombreRepresentante": "Juan Pérez",
  "email": "juan@plazacentral.com",
  "telefono": "3001234567",
  "nombrePlaza": "Plaza Central",
  "tipoSuscripcion": "pro",
  "cedulaUrl": "https://...",
  "rutUrl": "https://..."
}
```

### Paso 2: Aprobar la Solicitud
```bash
PUT /api/solicitudes/{id}
{
  "estado": "aprobada"
}
```

### Paso 3: Resultado Automático

El sistema automáticamente:

1. **Crea el Admin**:
   - Email: `juan@plazacentral.com`
   - Password: Generada automáticamente (ej: `Kp9m#N2xR@t5`)
   - Estado: `activo`
   - Fecha expiración: +30 días

2. **Envía Email**:
   ```
   Para: juan@plazacentral.com
   Asunto: ¡Bienvenido a PlazaNet! - Credenciales de Acceso
   
   Contenido:
   - Usuario: juan@plazacentral.com
   - Contraseña: Kp9m#N2xR@t5
   - Instrucciones de acceso
   ```

## 🔐 Seguridad

### Generación de Contraseña
- **Longitud**: 12 caracteres
- **Caracteres**: Mayúsculas, minúsculas, números y símbolos
- **Aleatoriedad**: Usa `RandomNumberGenerator` criptográficamente seguro

### Hash de Contraseña
- Usa SHA256 con salt
- ⚠️ **Para producción**: Considerar usar **BCrypt.Net** o **Argon2**

Instalación de BCrypt (recomendado):
```bash
dotnet add package BCrypt.Net-Next
```

## 📊 Estructura de la Tabla Admins

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | UUID | ID único del admin |
| `created_at` | TIMESTAMPTZ | Fecha de creación |
| `email` | VARCHAR(255) | Email único (login) |
| `password_hash` | VARCHAR(500) | Hash de contraseña |
| `nombre_plaza` | VARCHAR(255) | Nombre de la plaza |
| `nombre_representante` | VARCHAR(255) | Nombre del admin |
| `telefono` | VARCHAR(50) | Teléfono (opcional) |
| `tipo_suscripcion` | VARCHAR(50) | basico/pro/full |
| `fecha_expiracion` | TIMESTAMPTZ | Fecha de expiración |
| `estado` | VARCHAR(50) | activo/suspendido/cancelado |
| `solicitud_id` | UUID | FK a solicitudes |

## 🧪 Pruebas

### 1. Verificar Creación de Admin
```bash
# Aprobar una solicitud
PUT /api/solicitudes/{guid}
{
  "estado": "aprobada"
}

# Verificar logs
# Debe aparecer: "Admin creado exitosamente para solicitud {Id}"
# Debe aparecer: "Email de credenciales enviado para solicitud {Id}"
```

### 2. Verificar en Supabase
```sql
SELECT * FROM admins ORDER BY created_at DESC LIMIT 5;
```

### 3. Verificar Email
- Revisar el email del representante
- Debe recibir credenciales de acceso

## ⚠️ Consideraciones Importantes

### Email ya existe
Si el email ya está registrado como admin, el sistema:
- **No crea** un nuevo admin
- **Registra** un warning en los logs
- **Continúa** con la aprobación de la solicitud

### Error al crear admin
Si falla la creación del admin o el envío de email:
- ✅ La solicitud **sí se aprueba** (estado = "aprobada")
- ❌ El admin **no se crea**
- 📝 Se registra el error en logs
- 🔄 Se puede reintentar manualmente

### Logs a revisar
```
✅ "Admin creado exitosamente para solicitud {Id} - Email: {Email}"
✅ "Email de credenciales enviado para solicitud {Id} - Plaza: {Plaza}"
⚠️ "Ya existe un admin con el email {Email} para la solicitud {Id}"
❌ "Error al crear admin o enviar credenciales para solicitud {Id}"
```

## 🔄 Próximos Pasos Sugeridos

1. [ ] **Mejorar seguridad de passwords**
   - Implementar BCrypt o Argon2
   - Usar salt único por usuario

2. [ ] **Agregar reset de password**
   - Endpoint para solicitar cambio
   - Email con token temporal

3. [ ] **Implementar login**
   - Endpoint de autenticación
   - Generación de JWT tokens

4. [ ] **Dashboard de administración**
   - Ver admins registrados
   - Suspender/activar cuentas
   - Extender suscripciones

5. [ ] **Notificaciones de expiración**
   - Email 7 días antes de expirar
   - Email al expirar suscripción

## 📚 Ejemplo Completo

```bash
# 1. Crear solicitud
POST http://localhost:5000/api/solicitudes
Content-Type: application/json

{
  "nombreRepresentante": "María García",
  "email": "maria@plazaeste.com",
  "telefono": "3009876543",
  "nombrePlaza": "Plaza del Este",
  "tipoSuscripcion": "full"
}

# 2. Aprobar solicitud (reemplazar {id} con el ID devuelto)
PUT http://localhost:5000/api/solicitudes/{id}
Content-Type: application/json

{
  "estado": "aprobada"
}

# 3. Verificar en Supabase
SELECT * FROM admins WHERE email = 'maria@plazaeste.com';

# 4. María recibe email con:
# Usuario: maria@plazaeste.com
# Contraseña: x7Kp#M9nT@q2
```

## 🆘 Troubleshooting

### El admin no se crea
1. Verificar que la tabla `admins` existe en Supabase
2. Revisar logs de la aplicación
3. Verificar que el email no esté duplicado

### El email no llega
1. Verificar configuración SMTP en `appsettings.json`
2. Revisar spam/correo no deseado
3. Revisar logs: buscar errores de SMTP

### Password muy débil
- Modificar `GenerarPasswordSegura()` en `SolicitudesService.cs`
- Aumentar longitud o complejidad de caracteres
