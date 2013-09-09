using BCModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Account.Models.Company.Repository
{
    public interface ICompanyProfileRepository : IGenericRepository<CompanyProfile>
    {
        IQueryable<State> QueryStates();
        IQueryable<BusinessType> QueryBusinessTypes();
        IQueryable<UserProfile> QueryUserProfiles();
    }
}
