using BCModel;
using BCModel.Projects;
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
        private DbSet<CompanyProfile> _companies;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<BidPackage> _bidPackages;

        public ScopeRepository()
            : base()
        {
            _scopes = _context.Scopes;
            _users = _context.UserProfiles;
            _companies = _context.Companies;
            _projects = _context.Projects;
            _bidPackages = _context.BidPackages;
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

        public void Delete(params object[] key)
        {
            _scopes.Remove(_scopes.Find(key));
        }

        public void Delete(Scope entity)
        {
            _scopes.Remove(entity);
        }

        public Scope Get(params object[] key)
        {
            return _scopes.Find(key);
        }

        public IQueryable<Scope> Query()
        {
            return _scopes;
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public CompanyProfile GetCompany(int id)
        {
            return _companies.Find(id);
        }


        public BCModel.Projects.Project GetProject(int id)
        {
            return _projects.Find(id);
        }

        public BCModel.Projects.BidPackage GetBidPackage(int id)
        {
            return _bidPackages.Find(id);
        }
    }
}