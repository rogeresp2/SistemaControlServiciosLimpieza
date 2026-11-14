using System;
using System.Collections.Generic;
using System.Text;
using SistemaControlServiciosLimpieza.Domain.Entities;

namespace SistemaControlServiciosLimpieza.Infrastructure.Repositories
{
    public class EmpleadoRepository : GenericRepository<Empleado>
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public bool Exists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }

        public bool Update(int id, Empleado empleado)
        {
            var existing = _context.Empleados.FirstOrDefault(e => e.Id == id);
            if (existing == null)
                return false;

            existing.Name = empleado.Name;
            existing.Phone = empleado.Phone;
            existing.Email = empleado.Email;
            existing.Position = empleado.Position;
            existing.IsActive = empleado.IsActive;

            return true;
        }
    }
}