using BCModel.Projects;
using BCWeb.Areas.Project.Models.BidPackage.ViewModel;
using BCWeb.Areas.Project.Models.Documents.ViewModel;
using BCWeb.Areas.Project.Models.Invitations.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.Notifications.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    public enum InvitationResponse
    {
        accept, decline
    }

    [Authorize]
    public class BidPackageInvitationsController : ApiController
    {
        private IInvitationServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;

        public BidPackageInvitationsController(IInvitationServiceLayer service, IWebSecurityWrapper security, INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
        }

        public HttpResponseMessage Get(HttpRequestMessage request, int projectId)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            SubAndVendBidPackageAngularModel viewModel = new SubAndVendBidPackageAngularModel();

            // FIXME
            // need to group by inviting company and bid date
            // this should handle auto-invite 3rd tier so that here are not 20 items for 1 company all with the same bid date

            viewModel.BidPackages = _service.GetBidPackagesByProjectAndInvitedCompany(projectId, companyId)
                .Select(x => new SubAndVendBidPackageListItem
                {
                    BidDateTime = x.UseProjectBidDateTime ? x.Project.BidDateTime.ToString() : x.BidDateTime.Value.ToString(), 
                    BidPackageId = x.Id,
                    InviteResponse = x.Invitees.Where(i => i.SentToId == companyId).FirstOrDefault().AcceptedDate.HasValue ? true : x.Invitees.Where(i => i.SentToId == companyId).FirstOrDefault().RejectedDate.HasValue ? false : default(bool?),
                    InvitingCompanyId = x.CreatedById,
                    InvitingCompanyName = x.CreatedBy.CompanyName,
                    SelectedScopes = x.Scopes.Select(s => s.ScopeId).ToArray(),
                    ProjectDocs = x.Project.ProjectDocuments.Where(p => p.CompanyId == x.CreatedById).AsEnumerable().Select(d => new ProjectDocLookupItem { Name = d.Name, Id = d.Id })
                }).ToArray();

            viewModel.Scopes = _service.GetInvitationScopes(projectId, companyId).ToArray();

            return request.CreateResponse(HttpStatusCode.OK, viewModel);
        }

        public HttpResponseMessage Put(HttpRequestMessage request, int projectId, int bidPackageId, InvitationResponse rsvp)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            Invitation invite = _service.Get(bidPackageId, companyId);

            if (invite == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "Invitation not found");
            }

            switch (rsvp)
            {
                case InvitationResponse.accept:
                    invite.AcceptedDate = DateTime.Now;
                    invite.RejectedDate = default(DateTime?);
                    break;
                case InvitationResponse.decline:
                    invite.RejectedDate = DateTime.Now;
                    invite.AcceptedDate = default(DateTime?);
                    break;
                default:
                    return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (_service.Update(invite))
            {
                _notice.SendInviteResponse(invite.BidPackageId);
                return request.CreateResponse(HttpStatusCode.OK,
                    new
                    {
                        date = invite.AcceptedDate.HasValue ?
                            invite.AcceptedDate.Value.ToShortDateString() :
                            invite.RejectedDate.Value.ToShortDateString()
                    });
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.Conflict);
            }
        }
    }
}
