using Microsoft.EntityFrameworkCore;
using SistemaControlServiciosLimpieza.Api.Data;
using SistemaControlServiciosLimpieza.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

// Registrar AutoMapper y nuestros perfiles:
builder.Services.AddAutoMapper(cfg =>
{
    // Registrar el perfil manualmente (opcional):
    cfg.AddProfile<MappingProfile>();
}, typeof(Program).Assembly /* escanear automát. perfiles en el assembly */);



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
