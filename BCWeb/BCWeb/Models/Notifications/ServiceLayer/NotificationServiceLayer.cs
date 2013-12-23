using BCWeb.Models.Notifications.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Notifications.ServiceLayer
{
    public class NotificationServiceLayer : INotificationServiceLayer
    {
        private INotificationRepository _repo;

        public NotificationServiceLayer(INotificationRepository repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.Notification entity)
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
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Update(BCModel.Notification entity)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Delete(BCModel.Notification entity)
        {
            try
            {
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
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public IEnumerable<BCModel.Notification> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<BCModel.Notification> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.Notification, bool>> predicate)
        {
            return _repo.Query().Where(predicate);
        }

        public BCModel.Notification Get(params object[] key)
        {
            return _repo.Get(key);
        }

        public bool Exists(params object[] key)
        {
            return _repo.Get(key) == null;
        }
    }
}