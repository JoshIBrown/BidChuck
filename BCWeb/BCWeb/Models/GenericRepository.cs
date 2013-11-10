using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCModel;
using System.Data.Entity;

namespace BCWeb.Models
{
    public class GenericRepository<T> : RepositoryBase, IGenericRepository<T> where T : class
    {
        protected DbSet<T> _entities;

        public GenericRepository()
        {
            _entities = _context.Set<T>();
        }

        public void Create(T entity)
        {
            _entities.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _context.Entry<T>(entity).State = System.Data.EntityState.Modified;
        }

        public void Delete(int id)
        {
            _context.Set<T>().Remove(_entities.Find(id));
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public T Get(params object[] key)
        {
            return _entities.Find(key);
        }

        public IQueryable<T> Query()
        {
            return _entities.AsQueryable();
        }

        public bool Exists(int id)
        {
            bool exist = _entities.Find(id) != null;
            return exist;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}