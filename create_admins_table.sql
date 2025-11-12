-- Crear tabla admins en Supabase
-- Ejecutar este script en el SQL Editor de Supabase

CREATE TABLE IF NOT EXISTS public.admins (
    id UUID DEFAULT gen_random_uuid() PRIMARY KEY,
    created_at TIMESTAMPTZ DEFAULT timezone('utc'::text, now()) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(500) NOT NULL,
    nombre_plaza VARCHAR(255) NOT NULL,
    nombre_representante VARCHAR(255) NOT NULL,
    telefono VARCHAR(50),
    tipo_suscripcion VARCHAR(50) NOT NULL CHECK (tipo_suscripcion IN ('basico', 'pro', 'full')),
    fecha_expiracion TIMESTAMPTZ,
    estado VARCHAR(50) DEFAULT 'activo' NOT NULL CHECK (estado IN ('activo', 'suspendido', 'cancelado')),
    solicitud_id UUID REFERENCES public.solicitudes(id) ON DELETE SET NULL
);

-- Crear índices para mejorar el rendimiento
CREATE INDEX IF NOT EXISTS idx_admins_email ON public.admins(email);
CREATE INDEX IF NOT EXISTS idx_admins_estado ON public.admins(estado);
CREATE INDEX IF NOT EXISTS idx_admins_solicitud_id ON public.admins(solicitud_id);

-- Habilitar RLS (Row Level Security) si es necesario
ALTER TABLE public.admins ENABLE ROW LEVEL SECURITY;

-- Comentarios para documentación
COMMENT ON TABLE public.admins IS 'Tabla de administradores de plazas';
COMMENT ON COLUMN public.admins.email IS 'Email único del administrador (usado para login)';
COMMENT ON COLUMN public.admins.password_hash IS 'Hash de la contraseña';
COMMENT ON COLUMN public.admins.tipo_suscripcion IS 'Tipo de suscripción: basico, pro o full';
COMMENT ON COLUMN public.admins.estado IS 'Estado del administrador: activo, suspendido o cancelado';
COMMENT ON COLUMN public.admins.solicitud_id IS 'Referencia a la solicitud que originó este admin';
