using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SistemaControlServiciosLimpieza.Domain.Entities;

namespace SistemaControlServiciosLimpieza.Infrastructure.Repositories
{
    public class AsignacionRepository : GenericRepository<Asignacion>
    {
        private readonly ApplicationDbContext _context;

        public AsignacionRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public IEnumerable<Asignacion> GetAllWithDetails()
        {
            return _context.Asignaciones
                .Include(a => a.Servicio)
                .Include(a => a.Empleado)
                .ToList();
        }

        public bool Update(int id, Asignacion asignacion)
        {
            var existing = _context.Asignaciones.FirstOrDefault(a => a.Id == id);
            if (existing == null)
                return false;

            existing.ServicioId = asignacion.ServicioId;
            existing.EmpleadoId = asignacion.EmpleadoId;
            existing.AssignmentDate = asignacion.AssignmentDate;
            existing.Status = asignacion.Status;

            return true;
        }
    }
}