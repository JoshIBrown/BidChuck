using BCWeb.Models;
using BCModel;
using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.Repository
{
    public class BidPackageRepository : RepositoryBase, IBidPackageRepository
    {
        private DbSet<BCModel.Projects.BidPackage> _bidPackages;
        private DbSet<CompanyProfile> _companies;
        private DbSet<BidPackageXInvitee> _invites;
        private DbSet<BidPackageXScope> _selectedScopes;
        private DbSet<Scope> _scopes;
        private DbSet<UserProfile> _users;

        public BidPackageRepository()
        {
            _bidPackages = _context.BidPackages;
            _companies = _context.Companies;
            _invites = _context.BidPackageXInvitees;
            _selectedScopes = _context.BidPackageScopes;
            _scopes = _context.Scopes;
            _users = _context.UserProfiles;
        }
        public BCModel.Projects.Project GetProject(int id)
        {
            throw new NotImplementedException();
        }

        public BCModel.CompanyProfile GetCompany(int id)
        {
            return _companies.Find(id);
        }

        public IQueryable<BCModel.Scope> QueryScopes()
        {
            return _scopes;
        }

        public IQueryable<BCModel.Projects.BidPackageXScope> QuerySelectedScopes()
        {
            return _selectedScopes;
        }

        public IQueryable<BCModel.Projects.BidPackageXInvitee> QueryInvites()
        {
            return _invites;
        }

        public IQueryable<BCModel.CompanyProfile> QueryCompanies()
        {
            return _companies;
        }

        public void Create(BCModel.Projects.BidPackage entity)
        {
            _bidPackages.Add(entity);
        }

        public void Update(BCModel.Projects.BidPackage entity)
        {
            var current = _bidPackages.Find(entity.Id);
            _context.Entry<BCModel.Projects.BidPackage>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            Delete(_bidPackages.Find(id));
        }

        public void Delete(BCModel.Projects.BidPackage entity)
        {
            _bidPackages.Remove(entity);
        }

        public BCModel.Projects.BidPackage Get(int id)
        {
            return _bidPackages.Find(id);
        }

        public IQueryable<BCModel.Projects.BidPackage> Query()
        {
            return _bidPackages;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public BidPackageXInvitee GetInvite(int id)
        {
            return _invites.Find(id);
        }


        public UserProfile GetUser(int id)
        {
            return _users.Find(id);
        }
    }
}