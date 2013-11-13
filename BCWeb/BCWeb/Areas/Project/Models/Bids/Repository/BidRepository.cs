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
        private DbSet<BaseBid> _baseBids;
        private DbSet<ComputedBid> _computedBids;
        private DbSet<BCModel.Projects.BidPackage> _bidPackages;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<Invitation> _invites;
        private DbSet<UserProfile> _users;
        private DbSet<CompanyProfile> _companies;

        public BidRepository()
        {
            _baseBids = _context.BaseBids;
            _computedBids = _context.ComputedBids;
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
            return _bidPackages.Include(b => b.Scopes);
        }

        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _users.Find(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _companies.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public Invitation GetInvite(params object[] key)
        {
            return _invites.Find(key);
        }

        public IQueryable<Invitation> QueryInvites()
        {
            return _invites;
        }

        public void CreateBaseBid(BaseBid bid)
        {
            throw new NotImplementedException();
        }

        public void UpdateBaseBid(BaseBid bid)
        {
            throw new NotImplementedException();
        }

        public void DeleteBaseBid(BaseBid bid)
        {
            throw new NotImplementedException();
        }

        public BaseBid GetBaseBid(params object[] key)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BaseBid> QueryBaseBid()
        {
            throw new NotImplementedException();
        }

        public void CreateComputedBid(ComputedBid bid)
        {
            throw new NotImplementedException();
        }

        public void UpdateComputedBid(ComputedBid bid)
        {
            throw new NotImplementedException();
        }

        public void DeleteComputedBid(ComputedBid bid)
        {
            throw new NotImplementedException();
        }

        public ComputedBid GetComputedBid(params object[] key)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ComputedBid> QueryComputedBid()
        {
            throw new NotImplementedException();
        }
    }
}