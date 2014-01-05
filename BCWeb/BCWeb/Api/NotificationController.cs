using BCModel;
using BCWeb.Models;
using BCWeb.Models.Notifications.ServiceLayer;
using BCWeb.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    [Authorize]
    public class NotificationController : ApiController
    {

        private INotificationServiceLayer _service;
        private IWebSecurityWrapper _security;

        public NotificationController(INotificationServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        private string buildNoticeLink(NotificationType noticeType, int projectId)
        {
            string result = "";
            return result;
        }

        /// <summary>
        /// /api/Notification/GetAll
        /// </summary>
        /// <returns></returns>
        public AllNoticesViewModel GetAll()
        {
            AllNoticesViewModel result = new AllNoticesViewModel();
            int userId = _security.GetUserId(User.Identity.Name);
            result.Notices = _service.GetLastSevenDays(userId)
                .Select(s => new NoticeListItem
                {
                    TimeStamp = s.LastEditTimestamp,
                    IsRead = s.Read,
                    Message = s.Message,
                    ProjectId = s.ProjectId
                })
                .ToArray();
            return result;
        }


        /// <summary>
        /// /api/Notification/GetNewest
        /// </summary>
        /// <returns>10 most recent notifications</returns>
        public IEnumerable<object> GetNewest()
        {
            throw new NotImplementedException();
            int userId = _security.GetUserId(User.Identity.Name);
            IEnumerable<Notification> notes = _service.GetMostRecentTen(userId);
        }

        private void markAsRead(object notification)
        {
            throw new NotImplementedException();
        }
    }
}
