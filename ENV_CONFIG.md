# 🔐 Configuración de Variables de Entorno

Este proyecto utiliza variables de entorno para proteger información sensible como credenciales de base de datos y configuración de email.

## 📝 Variables Requeridas

### Base de Datos (Supabase)
- `DATABASE_URL`: Cadena de conexión completa a PostgreSQL

### Email (Brevo/SMTP)
- `SMTP_HOST`: Servidor SMTP (ej: smtp-relay.brevo.com)
- `SMTP_PORT`: Puerto SMTP (usualmente 587)
- `SMTP_USER`: Usuario SMTP
- `SMTP_PASSWORD`: Contraseña o API key SMTP
- `FROM_EMAIL`: Email del remitente

## 🖥️ Desarrollo Local

1. Copia el archivo `.env.example` a `.env`:
   ```bash
   copy .env.example .env
   ```

2. Edita `.env` con tus credenciales reales

3. El archivo `.env` está en `.gitignore` y nunca se subirá a GitHub

## ☁️ Despliegue en Vercel

### Configurar Variables de Entorno en Vercel:

1. Ve a tu proyecto en Vercel Dashboard
2. Click en **Settings** → **Environment Variables**
3. Agrega cada variable:

#### DATABASE_URL
```
Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.homkixaonxdourgskrwg;Password=8EbN_5diz9Z3G&A;SslMode=Require;Trust Server Certificate=true
```

#### SMTP_HOST
```
smtp-relay.brevo.com
```

#### SMTP_PORT
```
587
```

#### SMTP_USER
```
9b71b0001@smtp-brevo.com
```

#### SMTP_PASSWORD
```
xsmtpsib-731f86f590299eb6ad7111fa8ac216dfc0e1aacc23922deb6de38df20034e192-CLYObFdWgZ9StUvj
```

#### FROM_EMAIL
```
fabiotrianaar1707@gmail.com
```

### Aplicar Variables

- Marca **Production**, **Preview** y **Development** para cada variable
- Click en **Save**
- Redespliega tu proyecto para aplicar los cambios

## 🚀 Otras Plataformas

### Azure App Service
1. Ve a **Configuration** → **Application Settings**
2. Agrega cada variable como "New application setting"

### Railway
1. Ve a tu proyecto → **Variables**
2. Agrega cada variable

### Heroku
```bash
heroku config:set DATABASE_URL="tu-valor"
heroku config:set SMTP_HOST="smtp-relay.brevo.com"
# ... etc
```

## ⚠️ Seguridad

- ✅ **NUNCA** subas el archivo `.env` a GitHub
- ✅ **SIEMPRE** usa `.env.example` como plantilla (sin valores reales)
- ✅ Rota las credenciales periódicamente
- ✅ Usa diferentes credenciales para desarrollo y producción

## 🔍 Verificar Configuración

Para verificar que las variables se cargan correctamente, revisa los logs del servidor al iniciar.

Si todo está bien, deberías ver:
```
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
```

Si falta alguna variable, el sistema usará valores vacíos por defecto y puede fallar.
