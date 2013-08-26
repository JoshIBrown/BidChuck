using BCModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Scopes.Repository
{
    public class ScopeRepository : RepositoryBase, IScopeRepository
    {
        private DbSet<Scope> _scopes;
        private DbSet<UserProfile> _users;

        public ScopeRepository()
            : base()
        {
            _scopes = _context.Scopes;
            _users = _context.UserProfiles;
        }



        public UserProfile GetUser(int id)
        {
            return _users.Find(id);
        }

        public void Create(Scope entity)
        {
            _scopes.Add(entity);
        }

        public void Update(Scope entity)
        {
            var current = _scopes.Find(entity.Id);
            _context.Entry<Scope>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            _scopes.Remove(_scopes.Find(id));
        }

        public void Delete(Scope entity)
        {
            _scopes.Remove(entity);
        }

        public Scope Get(int id)
        {
            return _scopes.Find(id);
        }

        public IQueryable<Scope> Query()
        {
            return _scopes;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}