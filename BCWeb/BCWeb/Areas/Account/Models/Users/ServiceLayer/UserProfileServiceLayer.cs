using BCModel;
using BCWeb.Areas.Account.Models.Users.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Users.ServiceLayer
{
    public class UserProfileServiceLayer : IUserProfileServiceLayer
    {
        private IUserProfileRepository _repo;

        public UserProfileServiceLayer(IUserProfileRepository repo)
        {
            _repo = repo;
        }


        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }


        public bool Create(UserProfile entity)
        {
            throw new NotImplementedException("Use web security to create a user");
        }

        public bool Update(UserProfile entity)
        {
            try
            {
                // TODO: validate before update
                _repo.Update(entity);
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Delete(UserProfile entity)
        {
            try
            {
                _repo.Delete(entity);
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Delete(params object[] key)
        {
            try
            {
                _repo.Delete(key);
                return true;
            }
            catch (Exception ex)
            {

                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public IEnumerable<UserProfile> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<UserProfile> GetEnumerable(System.Linq.Expressions.Expression<Func<UserProfile, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public UserProfile Get(params object[] key)
        {
            return _repo.Get(key);
        }

        public bool Exists(params object[] key)
        {
            return _repo.Get(key) == null;
        }

        public CompanyProfile GetCompany(int id)
        {
            return _repo.GetCompany(id);
        }


        public IEnumerable<CompanyProfile> GetEnumerableCompanies()
        {
            throw new NotImplementedException();
        }
    }
}