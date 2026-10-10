using SistemaControlServiciosLimpieza.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace SistemaControlServiciosLimpieza.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Empleado> Empleados { get; set; } = null!;
        public DbSet<Servicio> Servicios { get; set; } = null!;
        public DbSet<Asignacion> Asignaciones { get; set; } = null!;
    }
}
