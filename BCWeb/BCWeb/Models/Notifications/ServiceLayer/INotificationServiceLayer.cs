using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Notifications.ServiceLayer
{
    public interface INotificationServiceLayer
    {
        Dictionary<string, string> ValidationDic
        {
            get;
        }

        bool MarkAsRead(Guid id);
        List<Notification> GetList();
    }
}
