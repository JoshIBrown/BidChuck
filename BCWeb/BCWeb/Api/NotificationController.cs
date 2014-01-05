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

            switch (noticeType)
            {
                case NotificationType.BidSubmitted:
                    result = string.Format("/Project/{0}/Bid/Received", projectId);
                    break;
                case NotificationType.InvitationRequest:
                    result = string.Format("/Project/{0}/Invitation/Requests", projectId);
                    break;
                case NotificationType.InvitationResponse:
                    result = string.Format("/Project/Details/{0}", projectId);
                    break;
                case NotificationType.InvitationToBid:
                    result = string.Format("/Project/Details/{0}", projectId);
                    break;
                case NotificationType.ProjectChange:
                    result = string.Format("/Project/Details/{0}", projectId);
                    break;
            }

            return result;
        }

        /// <summary>
        /// /api/Notification/GetAll
        /// </summary>
        /// <returns>notifications from the last 7 days</returns>
        public AllNoticesViewModel GetAll()
        {
            AllNoticesViewModel result = new AllNoticesViewModel();

            int userId = _security.GetUserId(User.Identity.Name);

            IEnumerable<Notification> notices = _service.GetLastSevenDays(userId);

            List<DateTime> dates = notices.OrderByDescending(d => d.LastEditTimestamp).Select(s => s.LastEditTimestamp.Date).Distinct().ToList();

            result.DatePool = new NoticeDatePool[dates.Count];

            for (int d = 0; d < dates.Count; d++)
            {
                result.DatePool[d] = new NoticeDatePool();
                result.DatePool[d].Date = dates[d].ToShortDateString();
                result.DatePool[d].Notices = notices.Where(n => n.LastEditTimestamp.Date == dates[d].Date)
                    .OrderByDescending(n => n.LastEditTimestamp)
                    .Select(s => new NoticeListItem
                    {
                        TimeStamp = s.LastEditTimestamp,
                        IsRead = s.Read,
                        Message = s.Message,
                        ProjectId = s.ProjectId,
                        Link = buildNoticeLink(s.NotificationType, s.ProjectId)
                    })
                    .ToArray();
            }


            return result;
        }


        /// <summary>
        /// /api/Notification/GetNewest
        /// </summary>
        /// <returns>10 most recent notifications</returns>
        public NewestNoticesViewModel GetNewest()
        {
            NewestNoticesViewModel result = new NewestNoticesViewModel();

            int userId = _security.GetUserId(User.Identity.Name);

            IEnumerable<Notification> notices = _service.GetMostRecentTen(userId);

            result.Notices = notices
                .Select(s => new NoticeListItem
                {
                    TimeStamp = s.LastEditTimestamp,
                    IsRead = s.Read,
                    Message = s.Message,
                    ProjectId = s.ProjectId,
                    Link = buildNoticeLink(s.NotificationType, s.ProjectId)
                })
                .ToArray();

            result.UnreadCount = notices.Where(n => !n.Read).Count();

            return result;
        }

        private void markAsRead(object notification)
        {
            throw new NotImplementedException();
        }
    }
}
