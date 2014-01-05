using BCModel;
using BCWeb.Models.Notifications.Repository;
using System;
using System.Collections.Generic;
using System.Data.Objects;
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



        public bool MarkAsRead(Guid id)
        {
            try
            {
                Notification note = _repo.Get(id);
                note.Read = true;
                _repo.Update(note);
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

        public IEnumerable<Notification> GetList()
        {
            return _repo.Query().AsEnumerable();
        }


        public IEnumerable<Notification> GetMostRecentTen(int userId)
        {
            return _repo.Query()
                .Where(n => n.RecipientId == userId)
                .OrderByDescending(n => n.LastEditTimestamp)
                .Take(10)
                .AsEnumerable();
        }

        public IEnumerable<Notification> GetLastSevenDays(int userId)
        {
            return (from r in _repo.Query()
                    where EntityFunctions.DiffDays(DateTime.Now, r.LastEditTimestamp) < 8
                    orderby r.LastEditTimestamp descending
                    select r).AsEnumerable();
        }
    }
}