using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Account.Repository
{
    public interface IUserProfileRepository
    {
        UserProfile GetProfile(int id);
        void CreateProfile(UserProfile profile);
        void UpdateProfile(UserProfile profile);
        void DeleteProfile(UserProfile profile);
        void DeleteProfile(int id);
        IQueryable<UserProfile> QueryProfiles();

        IQueryable<State> QueryStates();
        IQueryable<BusinessType> QueryBusinessTypes();
    }
}
