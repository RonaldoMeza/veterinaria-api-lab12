using Microsoft.EntityFrameworkCore;
using veterinaria_api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Veterinaria")
    ?? throw new InvalidOperationException(
        "No se configuró la cadena de conexión 'ConnectionStrings:Veterinaria'. Use User Secrets o una variable de entorno.");

builder.Services.AddDbContext<VeterinariaContext>(options => options.UseSqlServer(connectionString));

var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"] ?? "http://localhost:3000";
builder.Services.AddCors(options => options.AddPolicy("Frontend", policy => policy
    .WithOrigins(allowedOrigin)
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

// Swagger queda disponible también después de publicar la API.
app.UseSwagger();
app.UseSwaggerUI();

// Render termina HTTPS en su proxy y se comunica por HTTP con el contenedor.
// Localmente se mantiene la redirección HTTPS del perfil de desarrollo.
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.UseCors("Frontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
