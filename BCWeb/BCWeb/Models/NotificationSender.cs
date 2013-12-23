using BCWeb.Models.Notifications.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models
{
    public class NotificationSender : INotificationSender
    {
        private INotificationRepository _repo;
        public NotificationSender(INotificationRepository repo)
        {
            _repo = repo;
        }
        public bool SendNotification(int companyId, BCModel.NotificationType notificationType)
        {

            throw new NotImplementedException();
        }
    }
}