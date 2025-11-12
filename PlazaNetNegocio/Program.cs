using Microsoft.EntityFrameworkCore;
using PlazaNetNegocio.Data;
using PlazaNetNegocio.Repositories;
using PlazaNetNegocio.Services;

var builder = WebApplication.CreateBuilder(args);

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