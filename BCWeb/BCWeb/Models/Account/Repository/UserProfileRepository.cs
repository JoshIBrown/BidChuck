using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Account.Repository
{
    public class UserProfileRepository :GenericRepository<UserProfile>
    {
        public override void Update(UserProfile entity)
        {
            var current = _entities.Find(entity.UserId);
            _context.Entry<UserProfile>(current).CurrentValues.SetValues(entity);
        }
    }
}