using Microsoft.EntityFrameworkCore;
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

// Controllers (API tradicional)
builder.Services.AddControllers();

// **AGREGAR CORS AQUÍ** 👇
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
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