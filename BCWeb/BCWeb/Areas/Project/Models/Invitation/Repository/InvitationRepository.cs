using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Invitation.Repository
{
    public class InvitationRepository : RepositoryBase, IInvitationRepository
    {
        private DbSet<BidPackageXInvitee> _invites;
        private DbSet<BCModel.Projects.BidPackage> _bidPackages;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<BCModel.UserProfile> _users;
        private DbSet<BCModel.CompanyProfile> _companies;

        public InvitationRepository()
        {
            _invites = _context.BidPackageXInvitees;
            _bidPackages = _context.BidPackages;
            _projects = _context.Projects;
            _users = _context.UserProfiles;
            _companies = _context.Companies;
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

        public void Create(BCModel.Projects.BidPackageXInvitee entity)
        {
            _invites.Add(entity);
        }

        public void Update(BCModel.Projects.BidPackageXInvitee entity)
        {
            var current = _invites.Find(entity.Id);
            _context.Entry<BidPackageXInvitee>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            Delete(_invites.Find(id));
        }

        public void Delete(BCModel.Projects.BidPackageXInvitee entity)
        {
            _invites.Remove(entity);
        }

        public BCModel.Projects.BidPackageXInvitee Get(int id)
        {
            return _invites.Find(id);
        }

        public IQueryable<BCModel.Projects.BidPackageXInvitee> Query()
        {
            return _invites.Include(i=>i.Company).Include(i=>i.BidPackage);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}