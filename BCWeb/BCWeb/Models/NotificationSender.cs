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

        /// <summary>
        /// sends an internal notification to a user
        /// </summary>
        /// <param name="recipientId">userid</param>
        /// <param name="notificationType">type of notification</param>
        /// <param name="projectId">the project the notification is for</param>
        /// <returns></returns>
        public bool SendNotification(int recipientId, RecipientType recipientType, BCModel.NotificationType notificationType, int projectId)
        {
            try
            {
                switch (recipientType)
                {
                    case RecipientType.company:  // if company, then loop through each of the companies users
                        List<Notification> existingNotices = (from r in _repo.Query()
                                                              where r.NotificationType == notificationType      // notification type y
                                                              && r.Recipient.CompanyId == recipientId                     // sent to company z
                                                              && r.ProjectId == projectId                       // for project x
                                                              && !r.Read                                        // not read yet
                                                              && r.LastEditTimestamp.Date == DateTime.Now.Date  // from today
                                                              select r).ToList();

                        List<UserProfile> users = _repo.QueryUserProfiles().Where(x => x.CompanyId == recipientId).ToList();

                        if (existingNotices.Count == users.Count) // if there is an unread notice for each user of comany z
                        {
                            appendExistingNotices(existingNotices.ToArray());
                            _repo.Save(); // save changes
                            return true;
                        }
                        else if (existingNotices.Count > 0 && existingNotices.Count < users.Count) // if some users have read the notice already
                        {
                            // update unread
                            appendExistingNotices(existingNotices.ToArray());

                            // create new notices
                            sendNewNotices(users.Where(x => !existingNotices.Select(e => e.RecipientId).Contains(x.UserId)).Select(s => s.UserId).ToArray(), notificationType, projectId);

                            // save changes
                            _repo.Save();
                            return true;
                        }
                        else // else no notices exist
                        {
                            // create new notices
                            sendNewNotices(users.Select(s => s.UserId).ToArray(), notificationType, projectId);
                            _repo.Save();
                            return true;
                        }
                    case RecipientType.user:    // if user, only do the single notice
                        // find out if there is already a notification type for this project from today
                        Notification existingNotice = (from r in _repo.Query()
                                                       where r.NotificationType == notificationType      // notification type y
                                                       && r.RecipientId == recipientId                     // sent to user z
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
                    default:
                        throw new ArgumentException("Unknown notification type");
                }


            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// DRY.  loops through user ids creating them in the repository. does not save changes to the repo
        /// </summary>
        /// <param name="userId">array of user id's</param>
        /// <param name="notificationType">type of notification</param>
        /// <param name="projectId">the project id</param>
        private void sendNewNotices(int[] userId, NotificationType notificationType, int projectId)
        {
            try
            {

                Notification newNotice;
                for (int i = 0; i < userId.Length; i++)
                {
                    newNotice = new Notification
                    {
                        Count = 1,
                        LastEditTimestamp = DateTime.Now,
                        NotificationType = notificationType,
                        ProjectId = projectId,
                        RecipientId = userId[i],
                        Read = false
                    };
                    _repo.Create(newNotice);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// DRY. updates existing notices. does not save changes to the repository
        /// </summary>
        /// <param name="notices"></param>
        private void appendExistingNotices(Notification[] notices)
        {
            try
            {
                for (int i = 0; i < notices.Length; i++)
                {
                    notices[i].LastEditTimestamp = DateTime.Now;    // reset timestamp
                    notices[i].Count += 1;                          // increase count of changes
                    _repo.Update(notices[i]);                       // update record
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}