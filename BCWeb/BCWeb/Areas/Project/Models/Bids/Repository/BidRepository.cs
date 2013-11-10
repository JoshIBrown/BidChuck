using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Bids.Repository
{
    public class BidRepository : RepositoryBase, IBidRepository
    {
        private DbSet<Bid> _bids;
        private DbSet<BCModel.Projects.BidPackage> _bidPackages;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<Invitation> _invites;
        private DbSet<UserProfile> _users;
        private DbSet<CompanyProfile> _companies;

        public BidRepository()
        {
            _bids = _context.Bids;
            _bidPackages = _context.BidPackages;
            _projects = _context.Projects;
            _invites = _context.Invitations;
            _users = _context.UserProfiles;
            _companies = _context.Companies;
        }

        public BCModel.Projects.Project GetProject(int id)
        {
            return _projects.Find(id);
        }

        public BCModel.Projects.BidPackage GetBidPackage(int id)
        {
            return _bidPackages.Find(id);
        }

        public IQueryable<BCModel.Projects.BidPackage> QueryBidPackages()
        {
            return _bidPackages;
        }

        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _users.Find(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _companies.Find(id);
        }

        public void Create(BCModel.Projects.Bid entity)
        {
            _bids.Add(entity);
        }

        public void Update(BCModel.Projects.Bid entity)
        {
            var current = _bids.Find(entity.Id);
            _context.Entry<Bid>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            Delete(_bids.Find(id));
        }

        public void Delete(BCModel.Projects.Bid entity)
        {
            _bids.Remove(entity);
        }

        public BCModel.Projects.Bid Get(params object[] key)
        {
            return _bids.Find(key);
        }

        public IQueryable<BCModel.Projects.Bid> Query()
        {
            return _bids;
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public Invitation GetInvite(int id)
        {
            return _invites.Find(id);
        }

        public IQueryable<Invitation> QueryInvites()
        {
            return _invites;
        }
    }
}