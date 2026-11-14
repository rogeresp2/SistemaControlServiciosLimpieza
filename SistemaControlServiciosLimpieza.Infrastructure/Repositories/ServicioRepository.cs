using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SistemaControlServiciosLimpieza.Domain.Entities;

namespace SistemaControlServiciosLimpieza.Infrastructure.Repositories
{
    public class ServicioRepository : GenericRepository<Servicio>
    {
        private readonly ApplicationDbContext _context;

        public ServicioRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public IEnumerable<Servicio> GetAllWithCliente()
        {
            return _context.Servicios.Include(s => s.Cliente).ToList();
        }

        public bool Exists(int id)
        {
            return _context.Servicios.Any(s => s.Id == id);
        }

        public bool Update(int id, Servicio servicio)
        {
            var existing = _context.Servicios.FirstOrDefault(s => s.Id == id);
            if (existing == null)
                return false;

            existing.Name = servicio.Name;
            existing.ServiceDate = servicio.ServiceDate;
            existing.Cost = servicio.Cost;
            existing.Status = servicio.Status;
            existing.ClienteId = servicio.ClienteId;

            return true;
        }
    }
}