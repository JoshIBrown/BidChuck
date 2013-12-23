using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models
{
    public interface INotificationSender
    {

        bool SendNotification(int recipientId, RecipientType recipientType, NotificationType notificationType, int projectId);
    }

    public enum RecipientType
    {
        user, company
    }
}
