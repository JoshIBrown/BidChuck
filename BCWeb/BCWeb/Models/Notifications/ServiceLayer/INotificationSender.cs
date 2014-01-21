using BCModel;
using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Notifications.ServiceLayer
{
    public interface INotificationSender
    {

        bool SendNotification(int recipientId, RecipientType recipientType, NotificationType notificationType, int entityId, EntityType entityType);
        IEnumerable<Invitation> GetInvitationsNotDeclined(int projectId, int sendingCompanyId);

        bool SendInviteResponse(int bidPackageId);
    }

    public enum RecipientType
    {
        user, company
    }
}
