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
        private DbSet<BidPackageXScope> _bidPackageScopes;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<Invitation> _invites;
        private DbSet<UserProfile> _users;
        private DbSet<CompanyProfile> _companies;

        public BidRepository()
        {
            _baseBids = _context.BaseBids;
            _computedBids = _context.ComputedBids;
            _bidPackages = _context.BidPackages;
            _bidPackageScopes = _context.BidPackageScopes;
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
            return _invites.Include(i => i.BidPackage);
        }

        public void DeleteBaseBid(BaseBid bid)
        {
            _baseBids.Remove(bid);
        }

        public BaseBid GetBaseBid(params object[] key)
        {
            return _baseBids.Find(key);
        }

        public IQueryable<BaseBid> QueryBaseBid()
        {
            return _baseBids.Include(s => s.Scope);
        }

        public void DeleteComputedBid(ComputedBid bid)
        {
            _computedBids.Remove(bid);
        }

        public ComputedBid GetComputedBid(params object[] key)
        {
            return _computedBids.Find(key);
        }

        public IQueryable<ComputedBid> QueryComputedBid()
        {
            return _computedBids.Include(s => s.Scope).Include(b => b.BidPackage);
        }


        public void UpdateInvitation(Invitation invite)
        {
            var current = _invites.Find(invite.BidPackageId, invite.SentToId);
            _context.Entry<Invitation>(current).CurrentValues.SetValues(invite);
        }

        public void AddOrUpdateBaseBid(BaseBid bid)
        {
            var current = _baseBids.Find(bid.ProjectId, bid.SentToId, bid.ScopeId);
            if (current != null)
                _context.Entry<BaseBid>(current).CurrentValues.SetValues(bid);
            else
                _baseBids.Add(bid);
        }

        public void AddOrUpdateComputedBid(ComputedBid bid)
        {
            var current = _computedBids.Find(bid.BidPackageId, bid.SentToId, bid.ScopeId);
            if (current != null)
                _context.Entry<ComputedBid>(current).CurrentValues.SetValues(bid);
            else
                _computedBids.Add(bid);
        }


        public IQueryable<BidPackageXScope> QueryBidPackageScopes()
        {
            return _bidPackageScopes.Include(i => i.BidPackage).Include(i => i.Scope);
        }
    }
}