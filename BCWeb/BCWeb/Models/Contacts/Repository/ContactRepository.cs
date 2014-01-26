using BCModel;
using BCModel.SocialNetwork;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Contacts.Repository
{
    public class ContactRepository : RepositoryBase, IContactRepository
    {
        private DbSet<ContactConnection> _conns;
        private DbSet<ContactRequest> _reqs;
        private DbSet<CompanyProfile> _companies;
        private DbSet<BlackList> _blackLists;

        public ContactRepository()
        {
            _conns = _context.NetworkConnections;
            _reqs = _context.ConnectionRequests;
            _companies = _context.Companies;
            _blackLists = _context.BlackLists;
        }

        public void AddNetworkConnection(BCModel.SocialNetwork.ContactConnection entity)
        {
            _conns.Add(entity);
        }

        public void AddNetworkRequest(BCModel.SocialNetwork.ContactRequest entity)
        {
            _reqs.Add(entity);
        }

        public void DeleteNetworkConnection(BCModel.SocialNetwork.ContactConnection entity)
        {
            _conns.Remove(entity);
        }

        public BCModel.SocialNetwork.ContactConnection FindNetworkConnection(params object[] key)
        {
            return _conns.Find(key);
        }

        public BCModel.SocialNetwork.ContactRequest FindNetworkRequest(Guid id)
        {
            return _reqs.Find(id);
        }

        public IQueryable<BCModel.SocialNetwork.ContactConnection> QueryNetworkConnections()
        {
            return _conns.Include(l => l.Left).Include(r => r.Right);
        }

        public IQueryable<BCModel.SocialNetwork.ContactRequest> QueryContactRequests()
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


        public void UpdateNetworkRequest(ContactRequest entity)
        {
            var current = _reqs.Find(entity.Id);
            _context.Entry<ContactRequest>(current).CurrentValues.SetValues(entity);
        }


        public IQueryable<BlackList> QueryBlackList()
        {
            return _blackLists;
        }

        public void AddBlackList(BlackList entity)
        {
            _blackLists.Add(entity);
        }

        public void DeleteBlackList(BlackList entity)
        {
            _blackLists.Remove(entity);
        }

        public BlackList FindBlackList(params object[] key)
        {
            return _blackLists.Find(key);
        }
    }
}