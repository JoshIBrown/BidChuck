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
        private DbSet<Invitation> _invites;
        private DbSet<BidPackageXScope> _selectedScopes;
        private DbSet<Scope> _scopes;
        private DbSet<UserProfile> _users;
        private DbSet<BCModel.Projects.Project> _projects;

        public BidPackageRepository()
        {
            _bidPackages = _context.BidPackages;
            _companies = _context.Companies;
            _invites = _context.Invitations;
            _selectedScopes = _context.BidPackageScopes;
            _scopes = _context.Scopes;
            _users = _context.UserProfiles;
            _projects = _context.Projects;
        }
        public BCModel.Projects.Project GetProject(int id)
        {
            return _projects.Find(id);
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

        public IQueryable<BCModel.Projects.Invitation> QueryInvites()
        {
            return _invites.Include(i => i.SentTo);
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

        public void Delete(params object[] key)
        {
            Delete(_bidPackages.Find(key));
        }

        public void Delete(BCModel.Projects.BidPackage entity)
        {
            _bidPackages.Remove(entity);
        }

        public BCModel.Projects.BidPackage Get(params object[] key)
        {
            return _bidPackages.Find(key);
        }

        public IQueryable<BCModel.Projects.BidPackage> Query()
        {
            return _bidPackages.Include(b => b.Invitees).Include(b => b.Scopes).Include(b => b.ComputedBids);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Invitation GetInvite(int bidPackageId, int companyId)
        {
            return _invites.Find(bidPackageId,companyId);
        }


        public UserProfile GetUser(int id)
        {
            return _users.Find(id);
        }
    }
}