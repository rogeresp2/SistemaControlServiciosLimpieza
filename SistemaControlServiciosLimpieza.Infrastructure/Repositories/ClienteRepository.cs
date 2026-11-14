using System;
using System.Collections.Generic;
using System.Text;
using SistemaControlServiciosLimpieza.Domain.Entities;

namespace SistemaControlServiciosLimpieza.Infrastructure.Repositories
{
    public class ClienteRepository : GenericRepository<Cliente>
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public bool Exists(int id)
        {
            return _context.Clientes.Any(c => c.Id == id);
        }

        public bool Update(int id, Cliente cliente)
        {
            var existing = _context.Clientes.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return false;

            existing.Name = cliente.Name;
            existing.Phone = cliente.Phone;
            existing.Email = cliente.Email;
            existing.IsActive = cliente.IsActive;

            return true;
        }
    }
}