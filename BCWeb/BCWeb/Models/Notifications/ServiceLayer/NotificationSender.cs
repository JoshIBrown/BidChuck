using BCModel;
using BCModel.Projects;
using BCWeb.Models.Notifications.Repository;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Notifications.ServiceLayer
{
    public class NotificationSender : INotificationSender
    {
        private INotificationRepository _repo;
        private const string _InvitationToBidMsg = "You have been invited to bid on {0}";                     // Invitation to Bid	You have been invited to bid on {{project name}}
        private const string _ResponseToInvatiationMsg = "You have new invitation responses for {0}";         // Invitation Response	You have new  invitation responses for {{project name}}
        private const string _InvitationRequestMsg = "Companies are requesting an invitation to {0}";         // Invitation Request Companies are requesting an invitation to {{project name}}
        private const string _BidSubmissionMsg = "You have received new bids for {0}";                        // Bid Submission	You have received new bids for {{project name}}
        private const string _ChangesToProjectMsg = "There are recent updates to the project: {0}";           // Project change	There are recent updates to the project: {{project name}}
        private const string _BidWinnerMsg = "Your company has won the bid for {0}";                          // Bid Winner	Your company has won the bid for {{project name}}
        private const string _ConnectionAccepted = "You are now connected to {0}";

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
        public bool SendNotification(int recipientId, RecipientType recipientType, BCModel.NotificationType notificationType, int entityId, EntityType entityType)
        {
            try
            {
                switch (recipientType)
                {
                    // if company, then loop through each of the companies users
                    case RecipientType.company:
                        List<Notification> existingNotices = (from r in _repo.Query()
                                                              where r.NotificationType == notificationType      // notification type y
                                                              && r.Recipient.CompanyId == recipientId                     // sent to company z
                                                              && r.EntityId == entityId                       // for project x
                                                              && r.EntityType == entityType
                                                              && !r.Read                                        // not read yet
                                                              && EntityFunctions.DiffDays(r.LastEditTimestamp, DateTime.Now).Value == 0  // from today
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
                            sendNewNotices(users.Where(x => !existingNotices.Select(e => e.RecipientId).Contains(x.UserId)).Select(s => s.UserId).ToArray(), notificationType, entityId, entityType);

                            // save changes
                            _repo.Save();
                            return true;
                        }
                        else // else no notices exist
                        {
                            // create new notices
                            sendNewNotices(users.Select(s => s.UserId).ToArray(), notificationType, entityId, entityType);
                            _repo.Save();
                            return true;
                        }

                    // if user, only do the single notice
                    case RecipientType.user:
                        // find out if there is already a notification type for this project from today
                        Notification existingNotice = (from r in _repo.Query()
                                                       where r.NotificationType == notificationType      // notification type y
                                                       && r.RecipientId == recipientId                     // sent to user z
                                                       && r.EntityId == entityId                       // for project x
                                                       && r.EntityType == entityType
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
                                EntityId = entityId,
                                EntityType = entityType,
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
        /// <param name="entityId">the project id</param>
        private void sendNewNotices(int[] userId, NotificationType notificationType, int entityId, EntityType entityType)
        {
            try
            {
                BCModel.Projects.Project theProject = _repo.GetProject(entityId);
                Notification newNotice;
                for (int i = 0; i < userId.Length; i++)
                {
                    newNotice = new Notification
                    {
                        Count = 1,
                        LastEditTimestamp = DateTime.Now,
                        NotificationType = notificationType,
                        EntityId = entityId,
                        EntityType = entityType,
                        RecipientId = userId[i],
                        Read = false
                    };

                    switch (notificationType)
                    {
                        case NotificationType.BidSubmitted:
                            newNotice.Message = string.Format(_BidSubmissionMsg, theProject.Title);
                            break;
                        case NotificationType.InvitationRequest:
                            newNotice.Message = string.Format(_InvitationRequestMsg, theProject.Title);
                            break;
                        case NotificationType.InvitationResponse:
                            newNotice.Message = string.Format(_ResponseToInvatiationMsg, theProject.Title);
                            break;
                        case NotificationType.InvitationToBid:
                            newNotice.Message = string.Format(_InvitationToBidMsg, theProject.Title);
                            break;
                        case NotificationType.ProjectChange:
                            newNotice.Message = string.Format(_ChangesToProjectMsg, theProject.Title);
                            break;
                    }

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

        public IEnumerable<Invitation> GetInvitationsNotDeclined(int projectId, int sendingCompanyId)
        {
            var result = (from i in _repo.QueryInvites()
                          join b in _repo.QueryBidPackages() on i.BidPackageId equals b.Id
                          where b.ProjectId == projectId
                          && b.CreatedById == sendingCompanyId
                          && (i.AcceptedDate.HasValue || (!i.AcceptedDate.HasValue && !i.RejectedDate.HasValue))
                          select i).AsEnumerable();

            return result;
        }


        public bool SendInviteResponse(int bidPackageId)
        {
            var bidPackage = _repo.FindBidPackage(bidPackageId);

            return SendNotification(bidPackage.CreatedById, RecipientType.company, NotificationType.InvitationResponse, bidPackage.ProjectId, EntityType.Project);
        }


    }
}