using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Account.ServiceLayer
{
    public interface IUserProfileServiceLayer
    {
        Dictionary<string, string> ValidationDic { get; }
        bool CreateProfile(UserProfile profile);
        bool UpdateProfile(UserProfile profile);
        bool DeleteProfile(UserProfile profile);
        bool DeleteProfile(int id);
        IEnumerable<UserProfile> GetProfiles();
        IEnumerable<UserProfile> GetProfiles(Expression<Func<UserProfile, bool>> predicate);
        UserProfile GetProfile(int id);
        bool Exists(int id);

        IEnumerable<State> GetStates();
        IEnumerable<BusinessType> GetBusinessTypes();
    }
}
