using BCModel;
using BCModel.Projects;
using BCModel.SocialNetwork;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Company.Repository
{
    public class CompanyProfileRepository : RepositoryBase, ICompanyProfileRepository
    {
        private DbSet<CompanyProfile> _companies;
        private DbSet<UserProfile> _users;
        private DbSet<ContactConnection> _connections;
        private DbSet<ContactRequest> _requests;
        private DbSet<BlackList> _blackLists;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<BidPackage> _bidPackages;

        public CompanyProfileRepository()
        {
            _companies = _context.Companies;
            _users = _context.UserProfiles;
            _connections = _context.NetworkConnections;
            _requests = _context.ConnectionRequests;
            _blackLists = _context.BlackLists;
            _projects = _context.Projects;
            _bidPackages = _context.BidPackages;
        }

        public void Create(BCModel.CompanyProfile entity)
        {
            _companies.Add(entity);
        }

        public void Update(BCModel.CompanyProfile entity)
        {
            var current = _companies.Find(entity.Id);
            _context.Entry<CompanyProfile>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(params object[] key)
        {
            Delete(_companies.Find(key));
        }

        public void Delete(BCModel.CompanyProfile entity)
        {
            _companies.Remove(entity);
        }

        public BCModel.CompanyProfile Get(params object[] key)
        {
            return _companies.Find(key);
        }

        public IQueryable<BCModel.CompanyProfile> Query()
        {
            return _companies.Include(c => c.State).Include(c => c.Users);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<State> QueryStates()
        {
            return _context.States;
        }


        //public IQueryable<BusinessType> QueryBusinessTypes()
        //{
        //    return _context.BusinessTypes;
        //}


        public IQueryable<UserProfile> QueryUserProfiles()
        {
            return _users;
        }


        public UserProfile GetUserProfile(int id)
        {
            return _users.Find(id);
        }


        public IQueryable<BCModel.SocialNetwork.ContactConnection> QueryNetworkConnections()
        {
            return _connections;
        }


        public IQueryable<ContactRequest> QueryConnectionRequests()
        {
            return _requests;
        }

        public IQueryable<BlackList> QueryBlackLists()
        {
            return _blackLists;
        }


        public BCModel.Projects.Project FindProject(int id)
        {
            return _projects.Find(id);
        }

        public BCModel.Projects.BidPackage FindBidPackage(int id)
        {
            return _bidPackages.Find(id);
        }
    }
}