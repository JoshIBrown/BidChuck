using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Account.Repository
{
    public interface IAccountRepository
    {
        IQueryable<UserProfile> QueryUserProfiles();
        IQueryable<CompanyProfile> QueryCompanyProfiles();
        IQueryable<State> QueryStates();
        //IQueryable<BusinessType> QueryBusinessTypes();

        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);

        void UpdateUserProfile(UserProfile profile);
        void CreateCompany(CompanyProfile company);
        void Save();
    }
}
