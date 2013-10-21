using BCModel;
using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.Repository
{
    public class ProjectRepository : RepositoryBase, IProjectRepository
    {
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<UserProfile> _users;
        private DbSet<CompanyProfile> _companies;
        private DbSet<BidPackageXInvitee> _invites;
        private DbSet<BidPackageXScope> _bidPackageScopes;
        private DbSet<BidPackage> _bidPackages;
        private DbSet<Scope> _scopes;
        public ProjectRepository()
        {
            _projects = _context.Projects;
            _users = _context.UserProfiles;
            _companies = _context.Companies;
            _invites = _context.BidPackageXInvitees;
            _bidPackageScopes = _context.BidPackageScopes;
            _bidPackages = _context.BidPackages;
            _scopes = _context.Scopes;
        }
        public IQueryable<BCModel.Projects.ConstructionType> QueryConstructionType()
        {
            return _context.ConstructionTypes;
        }

        public IQueryable<BCModel.Projects.BuildingType> QueryBuildingType()
        {
            return _context.BuildingTypes;
        }

        //public IQueryable<BCModel.Projects.ProjectType> QueryProjectType()
        //{
        //    return _context.ProjectTypes;
        //}

        public void Create(BCModel.Projects.Project entity)
        {
            _projects.Add(entity);
        }

        public void Update(BCModel.Projects.Project entity)
        {
            var current = _projects.Find(entity.Id);
            _context.Entry<BCModel.Projects.Project>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            Delete(_projects.Find(id));
        }

        public void Delete(BCModel.Projects.Project entity)
        {
            _projects.Remove(entity);
        }

        public BCModel.Projects.Project Get(int id)
        {
            return _projects.Find(id);
        }

        public IQueryable<BCModel.Projects.Project> Query()
        {
            // eager load architect, state, and client
            return _projects.Include(s => s.Architect)
                .Include(s => s.State)
                .Include(s => s.Client)
                .Include(s => s.BidPackages)
                .Include(s=> s.CreatedBy)
                .Include(s => s.ConstructionType)
                .Include(s => s.BuildingType);
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public IQueryable<BCModel.State> QueryStates()
        {
            return _context.States;
        }


        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _users.Find(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _companies.Find(id);
        }

        public IQueryable<BCModel.CompanyProfile> GetCompanyProfiles()
        {
            return _companies;
        }


        public IQueryable<BCModel.Projects.BidPackageXInvitee> QueryInvites()
        {
            return _invites.Include(i => i.BidPackage).Include(i => i.BidPackage.Project);
        }


        public IQueryable<BidPackage> QueryBidPackages()
        {
            return _bidPackages.Include(b => b.Scopes).Include(b=>b.Invitees);
        }

        public IQueryable<BidPackageXScope> QueryBidPackageScopes()
        {
            return _bidPackageScopes;
        }


        public IQueryable<Scope> QueryScopes()
        {
            return _scopes;
        }
    }
}