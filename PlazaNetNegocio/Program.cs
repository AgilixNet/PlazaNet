using Microsoft.EntityFrameworkCore;
using System.Linq;
using PlazaNetNegocio.Data;
using PlazaNetNegocio.Repositories;
using PlazaNetNegocio.Services;
using DotNetEnv;

// Cargar variables de entorno desde archivo .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configurar variables de entorno
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
builder.Configuration["EmailSettings:SmtpHost"] = Environment.GetEnvironmentVariable("SMTP_HOST") 
    ?? builder.Configuration["EmailSettings:SmtpHost"];
builder.Configuration["EmailSettings:SmtpPort"] = Environment.GetEnvironmentVariable("SMTP_PORT") 
    ?? builder.Configuration["EmailSettings:SmtpPort"];
builder.Configuration["EmailSettings:SmtpUser"] = Environment.GetEnvironmentVariable("SMTP_USER") 
    ?? builder.Configuration["EmailSettings:SmtpUser"];
builder.Configuration["EmailSettings:SmtpPassword"] = Environment.GetEnvironmentVariable("SMTP_PASSWORD") 
    ?? builder.Configuration["EmailSettings:SmtpPassword"];
builder.Configuration["EmailSettings:FromEmail"] = Environment.GetEnvironmentVariable("FROM_EMAIL") 
    ?? builder.Configuration["EmailSettings:FromEmail"];

// Configurar variables de Supabase
builder.Configuration["Supabase:Url"] = Environment.GetEnvironmentVariable("SUPABASE_URL") 
    ?? builder.Configuration["Supabase:Url"];
builder.Configuration["Supabase:AnonKey"] = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY") 
    ?? builder.Configuration["Supabase:AnonKey"];
builder.Configuration["Supabase:ServiceKey"] = Environment.GetEnvironmentVariable("SUPABASE_SERVICE_KEY") 
    ?? builder.Configuration["Supabase:ServiceKey"];

// Controllers (API tradicional)
builder.Services.AddControllers();

// Configurar CORS (permitir dominios definidos por variable de entorno ALLOWED_ORIGINS)
// Ejemplo de ALLOWED_ORIGINS: "https://plazanet.vercel.app;https://1234-abc.ngrok-free.app;https://*.ngrok-free.app"
var allowedOriginsEnv = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS");
var allowedOrigins = (allowedOriginsEnv ?? "https://plazanet.vercel.app")
    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // Soporta coincidencia exacta y patrones con *.dominio (p.ej. https://*.ngrok-free.app) solo para DEV
        policy.SetIsOriginAllowed(origin =>
        {
            // Exact match
            if (allowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase))
                return true;

            // Wildcard subdomain match (https://*.example.com)
            foreach (var allowed in allowedOrigins)
            {
                if (allowed.StartsWith("https://*.") || allowed.StartsWith("http://*."))
                {
                    var https = allowed.StartsWith("https://*.");
                    var hostSuffix = allowed.Replace("https://*.", string.Empty)
                                             .Replace("http://*.", string.Empty);
                    if (Uri.TryCreate(origin, UriKind.Absolute, out var uri)
                        && uri.Host.EndsWith(hostSuffix, StringComparison.OrdinalIgnoreCase)
                        && ((https && uri.Scheme == Uri.UriSchemeHttps) || (!https && uri.Scheme == Uri.UriSchemeHttp)))
                    {
                        return true;
                    }
                }
            }

            return false;
        })
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// Swagger (para probar la API rápido)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext apuntando a Supabase (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias para Repository y Service
builder.Services.AddScoped<ISolicitudesRepository, SolicitudesRepository>();
builder.Services.AddScoped<IAdminsRepository, AdminsRepository>();
builder.Services.AddScoped<ISolicitudesService, SolicitudesService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<ISupabaseService, SupabaseService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// **USAR CORS ANTES DE AUTHORIZATION** 👇
app.UseCors();

app.UseHttpsRedirection();

// si luego metes auth/JWT, iría aquí:
// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();