using BCModel;
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

        public CompanyProfileRepository()
        {
            _companies = _context.Companies;
            _users = _context.UserProfiles;
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
    }
}