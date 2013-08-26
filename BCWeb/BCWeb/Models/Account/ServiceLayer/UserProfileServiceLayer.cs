using BCModel;
using BCWeb.Models.Account.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Account.ServiceLayer
{
    public class UserProfileServiceLayer : IUserProfileServiceLayer
    {
        private IUserProfileRepository _repo;

        public UserProfileServiceLayer(IUserProfileRepository repo)
        {
            _repo = repo;
        }

        public bool CreateProfile(UserProfile profile)
        {
            // TODO: validate before create
            try
            {
                _repo.CreateProfile(profile);
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool UpdateProfile(UserProfile profile)
        {
            try
            {
                // TODO: validate before update
                _repo.UpdateProfile(profile);
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool DeleteProfile(UserProfile profile)
        {
            try
            {
                _repo.DeleteProfile(profile);
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool DeleteProfile(int id)
        {
            try
            {
                _repo.DeleteProfile(id);
                return true;
            }
            catch (Exception ex)
            {
                
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public IEnumerable<UserProfile> GetProfiles()
        {
            return _repo.QueryProfiles().AsEnumerable();
        }

        public IEnumerable<UserProfile> GetProfiles(System.Linq.Expressions.Expression<Func<UserProfile, bool>> predicate)
        {
            return _repo.QueryProfiles().Where(predicate).AsEnumerable();
        }

        public IEnumerable<State> GetStates()
        {
            return _repo.QueryStates().AsEnumerable();
        }


        public UserProfile GetProfile(int id)
        {
            return _repo.GetProfile(id);
        }

        public bool Exists(int id)
        {
            return _repo.QueryProfiles().Where(x => x.UserId == id).Count() == 1;
        }

        public IEnumerable<BusinessType> GetBusinessTypes()
        {
            return _repo.QueryBusinessTypes().AsEnumerable();
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }
    }
}