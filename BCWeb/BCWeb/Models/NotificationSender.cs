using BCModel;
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

        public bool SendNotification(int recipientId, BCModel.NotificationType notificationType, int projectId)
        {
            try
            {

                // find out if there is already a notification type for this project from today
                Notification existingNotice = (from r in _repo.Query()
                                               where r.NotificationType == notificationType      // notification type y
                                               && r.RecipientId == recipientId                     // sent to company z
                                               && r.ProjectId == projectId                       // for project x
                                               && !r.Read                                        // not read yet
                                               && r.LastEditTimestamp.Date == DateTime.Now.Date  // from today
                                               select r).SingleOrDefault();

                // if there is not an existing notice
                if (existingNotice != null)
                {
                    // pull the notice out of the list
                    existingNotice.Count = existingNotice.Count++;      // increase notice count
                    existingNotice.LastEditTimestamp = DateTime.Now;    // reset timestamp
                    _repo.Update(existingNotice);                       // update notice
                    _repo.Save();                                       // save changes
                    return true;
                }
                else // there is no existing notice
                {
                    Notification theNotice = new Notification
                        {
                            Count = 1,
                            LastEditTimestamp = DateTime.Now,
                            NotificationType = notificationType,
                            ProjectId = projectId,
                            Read = false,
                            RecipientId = recipientId
                        };                      // draft the notice
                    _repo.Create(theNotice);    // add to queue
                    _repo.Save();               // send/save
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}