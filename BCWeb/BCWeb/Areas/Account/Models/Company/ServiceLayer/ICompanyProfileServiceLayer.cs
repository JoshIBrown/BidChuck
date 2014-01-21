using BCModel;
using BCWeb.Models;
using BCWeb.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Account.Models.Company.ServiceLayer
{
    public interface ICompanyProfileServiceLayer : IGenericServiceLayer<CompanyProfile>
    {
        IEnumerable<State> GetStates();
        //IEnumerable<BusinessType> GetBusinessTypes();
        IEnumerable<UserProfile> GetUserProfiles();
        IEnumerable<UserProfile> GetUserProfiles(Expression<Func<UserProfile, bool>> predicate);
        UserProfile GetUserProfile(int id);

        IEnumerable<CompanyProfile> GetEmptyLatLongList();
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, string city, string state, string postal, double? distance);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query);
        State GetState(int id);

        ConnectionStatus GetConnectionStatus(int currentCompany, int queriedCompany);
    }
}
