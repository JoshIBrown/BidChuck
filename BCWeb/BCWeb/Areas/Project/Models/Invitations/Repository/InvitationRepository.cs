using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Invitations.Repository
{
    public class InvitationRepository : RepositoryBase, IInvitationRepository
    {
        private DbSet<Invitation> _invites;
        private DbSet<BCModel.Projects.BidPackage> _bidPackages;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<BCModel.UserProfile> _users;
        private DbSet<BCModel.CompanyProfile> _companies;
        private DbSet<BCModel.Scope> _scopes;
        private DbSet<CompanyXScope> _companyScopes;

        public InvitationRepository()
        {
            _invites = _context.Invitations;
            _bidPackages = _context.BidPackages;
            _projects = _context.Projects;
            _users = _context.UserProfiles;
            _companies = _context.Companies;
            _scopes = _context.Scopes;
            _companyScopes = _context.CompanyScopes;
        }

        public BCModel.UserProfile GetUerProfile(int id)
        {
            return _users.Find(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _companies.Find(id);
        }

        public BCModel.Projects.BidPackage GetBidPackage(int id)
        {
            return _bidPackages.Find(id);
        }

        public BCModel.Projects.Project GetProject(int id)
        {
            return _projects.Find(id);
        }

        public void Create(BCModel.Projects.Invitation entity)
        {
            _invites.Add(entity);
        }

        public void Update(BCModel.Projects.Invitation entity)
        {
            var current = _invites.Find(entity.BidPackageId, entity.SentToId);
            _context.Entry<Invitation>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(params object[] key)
        {
            Delete(_invites.Find(key));
        }

        public void Delete(BCModel.Projects.Invitation entity)
        {
            _invites.Remove(entity);
        }

        public BCModel.Projects.Invitation Get(params object[] key)
        {
            return _invites.Find(key);
        }

        public IQueryable<BCModel.Projects.Invitation> Query()
        {
            return _invites.Include(i => i.SentTo).Include(i => i.BidPackage);
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public IQueryable<BCModel.CompanyProfile> QueryCompanies()
        {
            return _companies.Include(s => s.InvitesToBPs).Include(s => s.Scopes);
        }


        public IQueryable<BCModel.Scope> QueryScopes()
        {
            return _scopes;
        }


        public IQueryable<CompanyXScope> QueryCompanyScopes()
        {
            return _companyScopes;
        }
    }
}