using BCModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Account.Repository
{
    public class AccountRepository: RepositoryBase, IAccountRepository
    {
        public DbSet<UserProfile> _users;
        public DbSet<CompanyProfile> _companies;

        public AccountRepository()
        {
            _users = _context.UserProfiles;
            _companies = _context.Companies;
        }
        public IQueryable<BCModel.UserProfile> QueryUserProfiles()
        {
            return _users;
        }

        public IQueryable<BCModel.CompanyProfile> QueryCompanyProfiles()
        {
            return _companies;
        }

        public IQueryable<BCModel.State> QueryStates()
        {
            return _context.States;
        }

        //public IQueryable<BCModel.BusinessType> QueryBusinessTypes()
        //{
        //    return _context.BusinessTypes;
        //}

        public void UpdateUserProfile(BCModel.UserProfile profile)
        {
            var current = _users.Find(profile.UserId);
            _context.Entry<UserProfile>(current).CurrentValues.SetValues(profile);
        }

        public void CreateCompany(BCModel.CompanyProfile company)
        {
            _companies.Add(company);
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public UserProfile GetUserProfile(int id)
        {
            return _users.Find(id);
        }

        public CompanyProfile GetCompanyProfile(int id)
        {
            return _companies.Find(id);
        }
    }
}