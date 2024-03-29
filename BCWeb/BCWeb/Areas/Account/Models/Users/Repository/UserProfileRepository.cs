﻿using BCModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Users.Repository
{
    public class UserProfileRepository : RepositoryBase, IUserProfileRepository
    {
        private DbSet<UserProfile> _profiles;
        private DbSet<CompanyProfile> _companies;

        public UserProfileRepository()
        {
            _profiles = _context.UserProfiles;
            _companies = _context.Companies;
        }

        public void Create(UserProfile entity)
        {
            throw new NotImplementedException("Use web security to create a user");
        }

        public void Update(UserProfile entity)
        {
            var current = _profiles.Find(entity.UserId);
            _context.Entry<UserProfile>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(params object[] key)
        {
            Delete(_profiles.Find(key));
        }

        public void Delete(UserProfile entity)
        {
            _profiles.Remove(entity);
        }

        public UserProfile Get(params object[] key)
        {
            return _profiles.Find(key);
        }

        public IQueryable<UserProfile> Query()
        {
            return _profiles;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public CompanyProfile GetCompany(int id)
        {
            return _companies.Find(id);
        }


        public IQueryable<CompanyProfile> QueryCompanyProfile()
        {
            return _companies;
        }
    }
}