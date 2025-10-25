using Microsoft.EntityFrameworkCore;
using PlazaNetNegocio.Data;
using PlazaNetNegocio.Repositories;
using PlazaNetNegocio.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers (API tradicional)
builder.Services.AddControllers();

// Swagger (para probar la API rápido)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext apuntando a Supabase (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias para Repository y Service
builder.Services.AddScoped<ISolicitudesRepository, SolicitudesRepository>();
builder.Services.AddScoped<ISolicitudesService, SolicitudesService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// si luego metes auth/JWT, iría aquí:
// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
