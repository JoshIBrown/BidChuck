using BCModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Account.Repository
{
    public class UserProfileRepository : RepositoryBase, IUserProfileRepository
    {
        private BidChuckContext _context;
        private DbSet<UserProfile> _profiles;

        public UserProfileRepository()
        {
            _context = new BidChuckContext();
            _profiles = _context.UserProfiles;
        }


        public UserProfile GetProfile(int id)
        {
            return _profiles.Find(id);
        }

        public void CreateProfile(UserProfile profile)
        {
            _profiles.Add(profile);
            _context.SaveChanges();
        }

        public void UpdateProfile(UserProfile profile)
        {
            var current = _profiles.Find(profile.UserId);
            _context.Entry<UserProfile>(current).CurrentValues.SetValues(profile);
            _context.SaveChanges();
        }

        public void DeleteProfile(UserProfile profile)
        {
            _profiles.Remove(profile);
            _context.SaveChanges();
        }

        public void DeleteProfile(int id)
        {
            _profiles.Remove(_profiles.Find(id));
            _context.SaveChanges();
        }

        public IQueryable<UserProfile> QueryProfiles()
        {
            return _profiles;
        }

        public IQueryable<State> QueryStates()
        {
            return _context.State;
        }

        public IQueryable<BusinessType> QueryBusinessTypes()
        {
            return _context.BusinessTypes;
        }
    }
}