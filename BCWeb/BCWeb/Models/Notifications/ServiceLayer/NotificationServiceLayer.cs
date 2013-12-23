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



        public bool MarkAsRead(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<BCModel.Notification> GetList()
        {
            throw new NotImplementedException();
        }
    }
}