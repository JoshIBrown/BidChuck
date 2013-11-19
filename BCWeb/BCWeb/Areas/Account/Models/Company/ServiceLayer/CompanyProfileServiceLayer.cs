using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCWeb.Areas.Account.Models.Company.Repository;
using BCWeb.Helpers;
namespace BCWeb.Areas.Account.Models.Company.ServiceLayer
{
    public class CompanyProfileServiceLayer: ICompanyProfileServiceLayer
    {
        private ICompanyProfileRepository _repo;

        public CompanyProfileServiceLayer(ICompanyProfileRepository repo)
        {
            _repo = repo;
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.CompanyProfile entity)
        {
            try
            {
                _repo.Create(entity);
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

        public bool Update(BCModel.CompanyProfile entity)
        {
            try
            {
                _repo.Update(entity);
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

        public bool Delete(BCModel.CompanyProfile entity)
        {
            try
            {
                _repo.Delete(entity);
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

        public bool Delete(params object[] key)
        {
            try
            {
                _repo.Delete(key);
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

        public IEnumerable<BCModel.CompanyProfile> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.CompanyProfile, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public BCModel.CompanyProfile Get(params object[] key)
        {
            return _repo.Get(key);
        }

        public bool Exists(params object[] key)
        {
            return _repo.Get(key) == null;
        }

        public IEnumerable<BCModel.State> GetStates()
        {
            return _repo.QueryStates().AsEnumerable();
        }


        //public IEnumerable<BCModel.BusinessType> GetBusinessTypes()
        //{
        //    return _repo.QueryBusinessTypes().AsEnumerable();
        //}


        public IEnumerable<BCModel.UserProfile> GetUserProfiles()
        {
            return _repo.QueryUserProfiles().AsEnumerable();
        }

        public IEnumerable<BCModel.UserProfile> GetUserProfiles(System.Linq.Expressions.Expression<Func<BCModel.UserProfile, bool>> predicate)
        {
            return _repo.QueryUserProfiles().Where(predicate).AsEnumerable();
        }


        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _repo.GetUserProfile(id);
        }
    }
}