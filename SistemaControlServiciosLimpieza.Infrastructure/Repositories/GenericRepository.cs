using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SistemaControlServiciosLimpieza.Domain.Core;

namespace SistemaControlServiciosLimpieza.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<T> GetAll()
        {
            var entities = _context.Set<T>().ToList();
            return entities;
        }

        public T? GetById(int id)
        {
            var entity = _context.Set<T>().FirstOrDefault(p => p.Id == id);
            return entity;
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

    }
}