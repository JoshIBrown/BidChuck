using BCWeb.Models.Account.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Account.ServiceLayer
{
    public class AccountServiceLayer : IAccountServiceLayer
    {

        private IAccountRepository _repo;

        public AccountServiceLayer(IAccountRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<BCModel.UserProfile> GetUserProfiles()
        {
            return _repo.QueryUserProfiles().AsEnumerable();
        }

        public IEnumerable<BCModel.UserProfile> GetUserProfiles(System.Linq.Expressions.Expression<Func<BCModel.UserProfile, bool>> predicate)
        {
            return _repo.QueryUserProfiles().Where(predicate).AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> GetCompanyProfiles()
        {
            return _repo.QueryCompanyProfiles().AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> GetCompanyProfiles(System.Linq.Expressions.Expression<Func<BCModel.CompanyProfile, bool>> predicate)
        {
            return _repo.QueryCompanyProfiles().Where(predicate).AsEnumerable();
        }

        public IEnumerable<BCModel.State> GetStates()
        {
            return _repo.QueryStates().AsEnumerable();
        }

        public IEnumerable<BCModel.BusinessType> GetBusinessTypes()
        {
            return _repo.QueryBusinessTypes().AsEnumerable();
        }

        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _repo.GetUserProfile(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _repo.GetCompanyProfile(id);
        }

        public bool UpdateUserProfile(BCModel.UserProfile profile)
        {
            try
            {
                _repo.UpdateUserProfile(profile);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }

        public bool CreateCompany(BCModel.CompanyProfile company)
        {
            try
            {
                _repo.CreateCompany(company);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }


        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }
    }
}