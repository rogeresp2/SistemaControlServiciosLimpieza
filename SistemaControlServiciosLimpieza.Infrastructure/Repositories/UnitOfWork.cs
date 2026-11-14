using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SistemaControlServiciosLimpieza.Infrastructure.Repositories
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ClienteRepository Clientes { get; }
        public EmpleadoRepository Empleados { get; }
        public ServicioRepository Servicios { get; }
        public AsignacionRepository Asignaciones { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            ClienteRepository clienteRepository,
            EmpleadoRepository empleadoRepository,
            ServicioRepository servicioRepository,
            AsignacionRepository asignacionRepository)
        {
            this._context = context;
            Clientes = clienteRepository;
            Empleados = empleadoRepository;
            Servicios = servicioRepository;
            Asignaciones = asignacionRepository;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }
        public void Rollback()
        {
            _context.Database.RollbackTransaction();
        }
        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }
    }
}