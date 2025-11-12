# Configuración de Variables de Entorno

## Variables Requeridas

El proyecto requiere las siguientes variables de entorno:

### 1. Base de Datos (PostgreSQL/Supabase)
```
DATABASE_URL=Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.homkixaonxdourgskrwg;Password=8EbN_5diz9Z3G&A;SslMode=Require;Trust Server Certificate=true
```

### 2. Email (Brevo SMTP)
```
SMTP_HOST=smtp-relay.brevo.com
SMTP_PORT=587
SMTP_USER=9b71b0001@smtp-brevo.com
SMTP_PASSWORD=xsmtpsib-731f86f590299eb6ad7111fa8ac216dfc0e1aacc23922deb6de38df20034e192-CLYObFdWgZ9StUvj
FROM_EMAIL=fabiotrianaar1707@gmail.com
```

### 3. Supabase Auth y API
```
SUPABASE_URL=https://vcotkzkiwbyybncbjeiv.supabase.co
SUPABASE_ANON_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZjb3Rremtpd2J5eWJuY2JqZWl2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjE0NzcwMjAsImV4cCI6MjA3NzA1MzAyMH0.057uw60e6QR2M9gx-3nm9Rt370tagPvMFBP7QBedmz4
SUPABASE_SERVICE_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZjb3Rremtpd2J5eWJuY2JqZWl2Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc2MTQ3NzAyMCwiZXhwIjoyMDc3MDUzMDIwfQ.Ev2psRp0QgtnJBfIv_W3WZNKXgpknbrRPVDGOQjKPx0
```

## Configuración en Desarrollo Local

### Archivo .env
Crea un archivo `.env` en la raíz del proyecto `PlazaNetNegocio/` con todas las variables:

```bash
# Base de Datos
DATABASE_URL=Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.homkixaonxdourgskrwg;Password=8EbN_5diz9Z3G&A;SslMode=Require;Trust Server Certificate=true

# Email
SMTP_HOST=smtp-relay.brevo.com
SMTP_PORT=587
SMTP_USER=9b71b0001@smtp-brevo.com
SMTP_PASSWORD=xsmtpsib-731f86f590299eb6ad7111fa8ac216dfc0e1aacc23922deb6de38df20034e192-CLYObFdWgZ9StUvj
FROM_EMAIL=fabiotrianaar1707@gmail.com

# Supabase
SUPABASE_URL=https://vcotkzkiwbyybncbjeiv.supabase.co
SUPABASE_ANON_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZjb3Rremtpd2J5eWJuY2JqZWl2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjE0NzcwMjAsImV4cCI6MjA3NzA1MzAyMH0.057uw60e6QR2M9gx-3nm9Rt370tagPvMFBP7QBedmz4
SUPABASE_SERVICE_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZjb3Rremtpd2J5eWJuY2JqZWl2Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc2MTQ3NzAyMCwiZXhwIjoyMDc3MDUzMDIwfQ.Ev2psRp0QgtnJBfIv_W3WZNKXgpknbrRPVDGOQjKPx0
```

**Nota**: El archivo `.env` está en `.gitignore` y NO se subirá a GitHub.

## Configuración en Vercel (Producción)

### Pasos para configurar en Vercel:

1. **Ir a tu proyecto en Vercel**
   - Dashboard > Tu Proyecto > Settings

2. **Agregar variables de entorno**
   - Settings > Environment Variables

3. **Agregar cada variable:**

```
Name: DATABASE_URL
Value: Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.homkixaonxdourgskrwg;Password=8EbN_5diz9Z3G&A;SslMode=Require;Trust Server Certificate=true
Environment: Production, Preview, Development
```

```
Name: SMTP_HOST
Value: smtp-relay.brevo.com
Environment: Production, Preview, Development
```

```
Name: SMTP_PORT
Value: 587
Environment: Production, Preview, Development
```

```
Name: SMTP_USER
Value: 9b71b0001@smtp-brevo.com
Environment: Production, Preview, Development
```

```
Name: SMTP_PASSWORD
Value: xsmtpsib-731f86f590299eb6ad7111fa8ac216dfc0e1aacc23922deb6de38df20034e192-CLYObFdWgZ9StUvj
Environment: Production, Preview, Development
```

```
Name: FROM_EMAIL
Value: fabiotrianaar1707@gmail.com
Environment: Production, Preview, Development
```

```
Name: SUPABASE_URL
Value: https://vcotkzkiwbyybncbjeiv.supabase.co
Environment: Production, Preview, Development
```

```
Name: SUPABASE_ANON_KEY
Value: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZjb3Rremtpd2J5eWJuY2JqZWl2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjE0NzcwMjAsImV4cCI6MjA3NzA1MzAyMH0.057uw60e6QR2M9gx-3nm9Rt370tagPvMFBP7QBedmz4
Environment: Production, Preview, Development
```

```
Name: SUPABASE_SERVICE_KEY
Value: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZjb3Rremtpd2J5eWJuY2JqZWl2Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc2MTQ3NzAyMCwiZXhwIjoyMDc3MDUzMDIwfQ.Ev2psRp0QgtnJBfIv_W3WZNKXgpknbrRPVDGOQjKPx0
Environment: Production, Preview, Development
```

4. **Guardar y re-deployar**
   - Después de agregar todas las variables, hacer un nuevo deploy

## Seguridad

### ⚠️ IMPORTANTE - Service Role Key

La **SUPABASE_SERVICE_KEY** es extremadamente sensible:
- Permite bypass de Row Level Security
- Puede crear/modificar/eliminar cualquier dato
- Puede crear usuarios en Auth
- **NUNCA** exponerla en frontend
- **SOLO** usar en backend

### Variables Seguras

Estas variables SON gitignored:
- `.env` ✅ Gitignored
- `.env.local` ✅ Gitignored
- `.env.production` ✅ Gitignored

### Variables de Ejemplo

El archivo `.env.example` tiene placeholders y SÍ está en Git para que otros desarrolladores sepan qué variables necesitan.

## Verificación

Para verificar que las variables están cargadas correctamente:

```csharp
// En Program.cs
var dbUrl = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"DB URL loaded: {!string.IsNullOrEmpty(dbUrl)}");

var supabaseUrl = builder.Configuration["Supabase:Url"];
Console.WriteLine($"Supabase URL loaded: {!string.IsNullOrEmpty(supabaseUrl)}");
```

## Troubleshooting

### Error: "ConnectionString not initialized"
- Verificar que el archivo `.env` existe
- Verificar que `DATABASE_URL` está definida
- Reiniciar el servidor

### Error: "Tenant or user not found"
- Verificar credenciales de base de datos
- Verificar que el puerto es correcto (5432 para direct, 6543 para pooler)

### Error al crear usuario en Supabase Auth
- Verificar que `SUPABASE_SERVICE_KEY` es correcta
- Verificar que no es la anon key
- Verificar permisos en Supabase

## Recursos

- [DotNetEnv Documentation](https://github.com/tonerdo/dotenv.net)
- [Vercel Environment Variables](https://vercel.com/docs/environment-variables)
- [Supabase Auth API](https://supabase.com/docs/reference/javascript/auth-admin-createuser)
