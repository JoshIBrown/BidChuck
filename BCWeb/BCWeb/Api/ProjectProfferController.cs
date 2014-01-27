using BCModel.Projects;
using BCWeb.Areas.Project.Models.Invitations.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.GenericViewModel;
using BCWeb.Models.Notifications.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;

namespace BCWeb.Api
{
    public enum ProjectProfferResponse
    {
        join, leave
    }

    [Authorize(Roles = "general_contractor")]
    public class ProjectProfferController : ApiController
    {
        private IInvitationServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;

        public ProjectProfferController(IInvitationServiceLayer service, IWebSecurityWrapper security, INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
        }


        /// <summary>
        ///  this should only be executed by a general contractor.  subs aren't allowed to just join a project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Post(HttpRequestMessage request, int projectId)
        {
            Project theProject = _service.GetProject(projectId);

            if (theProject == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "project not found");
            }

            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            // get master bp because that is what gc's bid to.
            BidPackage bidPackage = theProject.BidPackages.Where(x => x.IsMaster).SingleOrDefault();

            Invitation selfInvite = _service.Get(bidPackage.Id, companyId);

            if (selfInvite != null && selfInvite.InvitationType == InvitationType.SentFromCreatedBy)
            {
                return request.CreateResponse(HttpStatusCode.Conflict, "company was invited. must respond to invitation.");
            }
            else if (selfInvite != null && selfInvite.InvitationType == InvitationType.SelfInvite && selfInvite.AcceptedDate.HasValue)
            {
                return request.CreateResponse(HttpStatusCode.Conflict, "company has already joined project.");
            }
            else if (selfInvite != null && selfInvite.InvitationType == InvitationType.SelfInvite && selfInvite.RejectedDate.HasValue)
            {
                selfInvite.RejectedDate = default(DateTime?);
                selfInvite.AcceptedDate = DateTime.Now;

                if (_service.Update(selfInvite))
                {
                    _notice.SendInviteResponse(bidPackage.Id);
                    return request.CreateResponse(HttpStatusCode.OK, new { date = selfInvite.AcceptedDate.Value.ToShortDateString() });
                }
            }
            else
            {
                selfInvite = new Invitation
                {
                    AcceptedDate = DateTime.Now,
                    BidPackageId = bidPackage.Id,
                    SentToId = companyId,
                    InvitationType = InvitationType.SelfInvite,
                    SentDate = DateTime.Now
                };

                if (_service.Create(selfInvite))
                {
                    _notice.SendInviteResponse(bidPackage.Id);
                    return request.CreateResponse(HttpStatusCode.OK, new { date = selfInvite.AcceptedDate.Value.ToShortDateString() });
                }
            }

            return request.CreateResponse(HttpStatusCode.InternalServerError);

        }


        /// <summary>
        /// update a proffer that has already been sent
        /// </summary>
        /// <param name="request"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Delete(HttpRequestMessage request, int projectId)
        {
            Project theProject = _service.GetProject(projectId);

            if (theProject == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "project not found");
            }

            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            // get master bp because that is what gc's bid to.
            BidPackage bidPackage = theProject.BidPackages.Where(x => x.IsMaster).SingleOrDefault();

            Invitation selfInvite = _service.Get(bidPackage.Id, companyId);

            if (selfInvite == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "you have not proffered to join this project");
            }

            selfInvite.RejectedDate = DateTime.Now;
            selfInvite.AcceptedDate = default(DateTime?);

            if (_service.Update(selfInvite))
            {
                _notice.SendInviteResponse(bidPackage.Id);
                return request.CreateResponse(HttpStatusCode.OK, new { date = selfInvite.RejectedDate.Value.ToShortDateString() });
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
