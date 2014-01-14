using BCModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Search.Repository
{
    public class SearchRepository: RepositoryBase, ISearchRepository
    {
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<CompanyProfile> _companies;
        private DbSet<UserProfile> _users;

        public SearchRepository()
        {
            _projects = _context.Projects;
            _companies = _context.Companies;
            _users = _context.UserProfiles;

        }
        public IQueryable<BCModel.Projects.Project> QueryProjects()
        {
            return _projects;
        }

        public IQueryable<BCModel.CompanyProfile> QueryCompanyProfiles()
        {
            return _companies;
        }

        public IQueryable<BCModel.UserProfile> QueryUserProfile()
        {
            return _users;
        }

        public BCModel.UserProfile FindUserProfile(int id)
        {
            return _users.Find(id);
        }

        public BCModel.CompanyProfile FindCompanyProfile(int id)
        {
            return _companies.Find(id);
        }

        public BCModel.Projects.Project FindProject(int id)
        {
            return _projects.Find(id);
        }
    }
}