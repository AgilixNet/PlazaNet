# Integración con Supabase Auth

## Resumen

Cuando una solicitud es **aprobada**, el sistema ahora:

1. **Crea el usuario en Supabase Auth** - Con email y contraseña generada
2. **Crea la plaza en tabla `plazas`** - Con datos de la solicitud
3. **Crea el perfil en tabla `perfiles`** - Vinculado al usuario y plaza con rol "AdminPlaza"
4. **Crea registro local en tabla `admins`** - En la base de datos de solicitudes
5. **Envía email con credenciales** - Al administrador de la plaza

## Flujo Completo

```
Solicitud Aprobada
       ↓
1. Generar contraseña segura (12 caracteres)
       ↓
2. Crear usuario en Supabase Auth
   POST /auth/v1/admin/users
   - email
   - password
   - email_confirm: true
       ↓
3. Crear plaza en Supabase
   POST /rest/v1/plazas
   - nombre: solicitud.NombrePlaza
   - email: solicitud.Email
   - telefono: solicitud.Telefono
   - estado: "activo"
       ↓
4. Crear perfil en Supabase
   POST /rest/v1/perfiles
   - id: auth.users.id
   - nombre: solicitud.NombreRepresentante
   - rol: "AdminPlaza"
   - correo: solicitud.Email
   - plaza_id: plaza.id
       ↓
5. Crear admin local (BD solicitudes)
   - PasswordHash
   - Datos de suscripción
       ↓
6. Enviar email con credenciales
```

## Estructura de Datos

### Supabase Auth (auth.users)
```
id: UUID (generado automáticamente)
email: string
encrypted_password: string (Supabase lo encripta)
email_confirmed_at: timestamp (auto-confirmado)
```

### Tabla perfiles
```
id: UUID (mismo que auth.users.id)
nombre: string
rol: "AdminPlaza"
correo: string
plaza_id: UUID (FK a plazas)
created_at: timestamp
```

### Tabla plazas
```
id: UUID (generado automáticamente)
nombre: string
ciudad: string
ubicacion: string
telefono: string
email: string
estado: "activo" | "inactivo"
created_at: timestamp
```

## Configuración Necesaria

### Variables de Entorno (.env)

```bash
# Supabase Auth y API
SUPABASE_URL=https://vcotkzkiwbyybncbjeiv.supabase.co
SUPABASE_ANON_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
SUPABASE_SERVICE_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### En Vercel

Agregar las mismas 3 variables:
- `SUPABASE_URL`
- `SUPABASE_ANON_KEY` (para operaciones REST)
- `SUPABASE_SERVICE_KEY` (para crear usuarios en Auth)

## Servicios Involucrados

### SupabaseService
- `CrearAdminPlazaCompleto()` - Método principal
- `CrearUsuarioAuth()` - Crea usuario en Supabase Auth
- `CrearPlaza()` - Inserta registro en tabla plazas
- `CrearPerfil()` - Inserta registro en tabla perfiles

### SolicitudesService
- `UpdateAsync()` - Detecta cambio de estado a "aprobada"
- Llama a `SupabaseService.CrearAdminPlazaCompleto()`
- Crea admin local
- Envía email con credenciales

## Seguridad

### Service Role Key
- **NUNCA** exponer en el frontend
- Solo usar en backend para operaciones administrativas
- Permite crear usuarios sin autenticación
- Permite bypass de Row Level Security

### Anon Key
- Segura para usar en frontend
- Respeta Row Level Security
- Solo para operaciones de usuarios autenticados

## Testing

### Probar el flujo completo:

1. **Crear solicitud**
```bash
POST http://localhost:5000/api/solicitudes
{
  "nombreRepresentante": "Juan Pérez",
  "email": "juan@example.com",
  "telefono": "555-1234",
  "nombrePlaza": "Plaza Central",
  "tipoSuscripcion": "pro"
}
```

2. **Aprobar solicitud**
```bash
PUT http://localhost:5000/api/solicitudes/{id}
{
  "estado": "aprobada"
}
```

3. **Verificar en Supabase**
- Auth > Users: Debe aparecer juan@example.com
- Table Editor > plazas: Debe aparecer "Plaza Central"
- Table Editor > perfiles: Debe aparecer el perfil vinculado

4. **Verificar email**
- Revisar bandeja de juan@example.com
- Debe llegar email con credenciales

## Logs

El sistema genera logs detallados:

```
Admin creado en Supabase - UserId: {uuid}, PlazaId: {uuid}
Admin local creado exitosamente para solicitud {id}
Email de credenciales enviado para solicitud {id} - Plaza: {nombre}
```

## Manejo de Errores

Si algo falla:
- El error se registra en logs
- La solicitud sigue marcada como "aprobada"
- El proceso puede reintentarse manualmente
- Los registros parciales quedan en Supabase

## Notas Importantes

1. **Email único**: No se pueden crear dos usuarios con el mismo email
2. **Auto-confirmación**: Los usuarios se crean con email confirmado
3. **Contraseña segura**: 12 caracteres con letras, números y símbolos
4. **Vinculación**: perfil.id === auth.users.id
5. **Estado inicial**: Plaza y perfil se crean en estado "activo"
