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
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BusinessType[] types, string city, string state, string postal, double distance, int[] scopes);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, string city, string state, string postal, double distance, int[] scopes);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BusinessType[] types, string city, string state, string postal, double distance, int[] scopes);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string city, string state, string postal, double distance, int[] scopes);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BusinessType[] types, string city, string state, string postal, double distance);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, string city, string state, string postal, double distance);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BusinessType[] types, string city, string state, string postal, double distance);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string city, string state, string postal, double distance);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(int projectIdforLocation, int bidPackageIdforScopes);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(int projectIdforLocation);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BusinessType[] types, int projectIdforLocation, int bidPackageIdforScopes);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BusinessType[] types, int projectIdforLocation);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BusinessType[] types, int projectIdforLocation, int bidPackageIdforScopes);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BusinessType[] types, int projectIdforLocation);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BusinessType[] types);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BusinessType[] types);
        IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query);
        State GetState(int id);


    }
}
