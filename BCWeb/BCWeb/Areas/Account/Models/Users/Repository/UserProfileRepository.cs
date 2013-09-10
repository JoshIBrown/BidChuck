using BCModel;
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

        public UserProfileRepository()
        {
            _profiles = _context.UserProfiles;
        }

        public void Create(UserProfile entity)
        {
            throw new NotImplementedException();
        }

        public void Update(UserProfile entity)
        {
            var current = _profiles.Find(entity.UserId);
            _context.Entry<UserProfile>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            Delete(_profiles.Find(id));
        }

        public void Delete(UserProfile entity)
        {
            _profiles.Remove(entity);
        }

        public UserProfile Get(int id)
        {
            return _profiles.Find(id);
        }

        public IQueryable<UserProfile> Query()
        {
            return _profiles;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}