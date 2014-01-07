using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Documents.Repository
{
    public class ProjectDocRepository : RepositoryBase, IProjectDocRepository
    {
        private DbSet<ProjectDocument> _docs;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<BCModel.CompanyProfile> _companies;
        private DbSet<Invitation> _invites;

        public ProjectDocRepository()
        {
            _docs = _context.ProjectDocs;
            _projects = _context.Projects;
            _companies = _context.Companies;
            _invites = _context.Invitations;
        }

        public BCModel.Projects.Project FindProject(int id)
        {
            return _projects.Find(id);
        }

        public BCModel.CompanyProfile FindCompanyProfile(int id)
        {
            return _companies.Find(id);
        }

        public void Create(BCModel.Projects.ProjectDocument entity)
        {
            _docs.Add(entity);
        }

        public void Update(BCModel.Projects.ProjectDocument entity)
        {
            var current = _docs.Find(entity.Id);
            _context.Entry<ProjectDocument>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(params object[] key)
        {
            Delete(_docs.Find(key));
        }

        public void Delete(BCModel.Projects.ProjectDocument entity)
        {
            _docs.Remove(entity);
        }

        public BCModel.Projects.ProjectDocument Get(params object[] key)
        {
            return _docs.Find(key);
        }

        public IQueryable<BCModel.Projects.ProjectDocument> Query()
        {
            return _docs;
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public BCModel.UserProfile FindUser(int id)
        {
            return _context.UserProfiles.Find(id);
        }


        public IQueryable<Invitation> QueryInvites()
        {
            return _invites.Include(i => i.BidPackage);
        }
    }
}