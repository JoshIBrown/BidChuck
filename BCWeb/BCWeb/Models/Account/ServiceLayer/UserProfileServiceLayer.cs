using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Account.ServiceLayer
{
    public class UserProfileServiceLayer : IGenericServiceLayer<UserProfile>
    {
        private IGenericRepository<UserProfile> _repo;

        public UserProfileServiceLayer(IGenericRepository<UserProfile> repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(UserProfile entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(UserProfile entity)
        {
            try
            {
                // TODO: validate before update
                _repo.Update(entity);
                _repo.Save();
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
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserProfile> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<UserProfile> GetEnumerable(System.Linq.Expressions.Expression<Func<UserProfile, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public UserProfile Get(int id)
        {
            return _repo.Get(id);
        }

        public bool Exists(int id)
        {
            return _repo.Exists(id);
        }
    }
}