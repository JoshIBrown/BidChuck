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
    public class NotificationsController : ApiController
    {

        private INotificationServiceLayer _service;
        private IWebSecurityWrapper _security;

        public NotificationsController(INotificationServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }



        /// <summary>
        /// /api/Notification/GetAll
        /// </summary>
        /// <returns>notifications from the last 7 days</returns>
       
        public AllNoticesViewModel Get()
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
                        Link = buildNoticeLink(s.NotificationType, s.EntityId, s.EntityType)
                    })
                    .ToArray();
            }


            return result;
        }

        private void markAsRead(object notification)
        {
            throw new NotImplementedException();
        }

        private string buildNoticeLink(NotificationType noticeType, int entityId, EntityType entityType)
        {
            string result = "";

            switch (entityType)
            {
                case EntityType.Project:
                    switch (noticeType)
                    {
                        case NotificationType.BidSubmitted:
                            result = string.Format("/Project/{0}/Bid/Received", entityId);
                            break;
                        case NotificationType.InvitationRequest:
                            result = string.Format("/Project/{0}/Invitation/Requests", entityId);
                            break;
                        case NotificationType.InvitationResponse:
                        case NotificationType.InvitationToBid:
                        case NotificationType.ProjectChange:
                            result = string.Format("/Project/Details/{0}", entityId);
                            break;
                    }
                    break;
                case EntityType.Company:
                    switch (noticeType)
                    {
                        case NotificationType.ConnectionAccepted:
                        case NotificationType.RequestToConnect:
                            result = string.Format("/Company/Profile/{0}", entityId);
                            break;
                    }
                    break;
            }

            return result;
        }
    }
}
