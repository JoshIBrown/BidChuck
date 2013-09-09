using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BCWeb.Models.Account.ServiceLayer
{
    public interface IAccountServiceLayer
    {
        IEnumerable<UserProfile> GetUserProfiles();
        IEnumerable<UserProfile> GetUserProfiles(Expression<Func<UserProfile, bool>> predicate);
        IEnumerable<CompanyProfile> GetCompanyProfiles();
        IEnumerable<CompanyProfile> GetCompanyProfiles(Expression<Func<CompanyProfile, bool>> predicate);

        IEnumerable<State> GetStates();
        IEnumerable<BusinessType> GetBusinessTypes();

        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);

        bool UpdateUserProfile(UserProfile profile);
        bool CreateCompany(CompanyProfile company);
        Dictionary<string, string> ValidationDic { get; }
    }
}