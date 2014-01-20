using BCModel;
using BCModel.SocialNetwork;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Company.Models.Repository
{
    public class NetworkRepository : RepositoryBase, INetworkRepository
    {
        private DbSet<NetworkConnection> _conns;
        private DbSet<NetworkRequest> _reqs;
        private DbSet<CompanyProfile> _companies;

        public NetworkRepository()
        {
            _conns = _context.SocialNetworkConnections;
            _reqs = _context.SocialNetworkRequests;
            _companies = _context.Companies;
        }

        public void AddNetworkConnection(BCModel.SocialNetwork.NetworkConnection entity)
        {
            _conns.Add(entity);
        }

        public void AddNetworkRequest(BCModel.SocialNetwork.NetworkRequest entity)
        {
            _reqs.Add(entity);
        }

        public void DeleteNetworkConnection(BCModel.SocialNetwork.NetworkConnection entity)
        {
            _conns.Remove(entity);
        }

        public BCModel.SocialNetwork.NetworkConnection FindNetworkConnection(params object[] key)
        {
            return _conns.Find(key);
        }

        public BCModel.SocialNetwork.NetworkRequest FindNetworkRequest(Guid id)
        {
            return _reqs.Find(id);
        }

        public IQueryable<BCModel.SocialNetwork.NetworkConnection> QueryNetworkConnections()
        {
            return _conns.Include(l => l.Left).Include(r => r.Right);
        }

        public IQueryable<BCModel.SocialNetwork.NetworkRequest> QueryNetworkRequests()
        {
            return _reqs.Include(s => s.Sender).Include(r => r.Recipient);
        }

        public BCModel.CompanyProfile FindCompanyProfile(int id)
        {
            return _companies.Find(id);
        }

        public IQueryable<BCModel.CompanyProfile> QueryCompanyProfiles()
        {
            return _companies;
        }

        public BCModel.UserProfile FindUserProfile(int id)
        {
            return _context.UserProfiles.Find(id);
        }

        public void SaveChanges()
        {
            SaveChanges();
        }


        public void UpdateNetworkRequest(NetworkRequest entity)
        {
            var current = _reqs.Find(entity.Id);
            _context.Entry<NetworkRequest>(current).CurrentValues.SetValues(entity);
        }
    }
}