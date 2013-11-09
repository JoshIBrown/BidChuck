using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.Invitation.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.GenericViewModel;
using BCWeb.Models.Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;

namespace BCWeb.Api
{
    [Authorize]
    public class InvitationController : ApiController
    {
        private IInvitationServiceLayer _service;
        private IWebSecurityWrapper _security;

        public InvitationController(IInvitationServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        [ValidateHttpAntiForgeryToken]
        public JQueryPostResult PostAccept([FromUri]int id)
        {
            JQueryPostResult result = new JQueryPostResult();

            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            BidPackageXInvitee invite = _service.Get(id);
            if (invite.CompanyId == companyId)
            {
                invite.AcceptedDate = DateTime.Now;
                if (invite.RejectedDate.HasValue)
                    invite.RejectedDate = default(DateTime?); // null out hte decline date
                if (_service.Update(invite))
                {
                    result.success = true;
                    result.message = "invitation accepted";
                    result.data = new { date = invite.AcceptedDate.Value.ToShortDateString() };
                }
                else
                {
                    result.success = false;
                    result.message = "unable to accept invitation";
                }
            }
            else // prevent company from accepting other companies invite
            {
                result.success = false;
                result.message = "invalid invitation";
            }

            return result;
        }

        [ValidateHttpAntiForgeryToken]
        public JQueryPostResult PostDecline([FromUri]int id)
        {
            JQueryPostResult result = new JQueryPostResult();

            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            BidPackageXInvitee invite = _service.Get(id);

            // make sure company responding is the same company that the invite is for
            if (invite.CompanyId == companyId)
            {
                invite.RejectedDate = DateTime.Now;

                if (invite.AcceptedDate.HasValue)
                    invite.AcceptedDate = default(DateTime?);

                if (_service.Update(invite))
                {
                    result.success = true;
                    result.message = "invitation declined";
                    result.data = new { date = invite.RejectedDate.Value.ToShortDateString() };
                }
                else
                {
                    result.success = false;
                    result.message = "unable to decline invitation";
                }
            }
            else // prevent company from accepting other companies invite
            {
                result.success = false;
                result.message = "invalid invitation";
            }

            return result;
        }

        [ValidateHttpAntiForgeryToken]
        public JQueryPostResult PostJoin(int bidPackageId)
        {
            JQueryPostResult result = new JQueryPostResult();
            BidPackage bidPackage = _service.GetBidPackage(bidPackageId);
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            // make sure an invited user didn't accidentally find this link and join the project again
            // or that they haven't already joined the project
            if (bidPackage.Invitees.Where(i => i.CompanyId == companyId).Count() == 0)
            {
                // assemble invite
                BidPackageXInvitee selfInvitation = new BidPackageXInvitee
                {

                    AcceptedDate = DateTime.Now,
                    BidPackage = bidPackage,
                    CompanyId = companyId,
                    InvitationType = InvitationType.SelfInvite,
                    SentDate = DateTime.Now
                };

                // try and add invite to system.
                if (_service.Create(selfInvitation))
                {
                    result.success = true;
                    result.message = "joined project";
                    result.data = new { date = selfInvitation.AcceptedDate.Value.ToShortDateString(), inviteId = selfInvitation.Id };
                }
                else
                {
                    result.success = false;
                    result.message = "unable to join project";
                }
            }
            else if (bidPackage.Invitees.Where(i => i.CompanyId == companyId).Count() == 1)
            {
                var invite = bidPackage.Invitees.Where(i => i.CompanyId == companyId).First();
                invite.RejectedDate = default(DateTime?);
                invite.AcceptedDate = DateTime.Now;

                if (_service.Update(invite))
                {
                    result.success = true;
                    result.message = "joined project";
                    result.data = new { date = invite.AcceptedDate.Value.ToShortDateString() };
                }
                else
                {
                    result.success = false;
                    result.message = "unable to join project";
                }
            }
            else
            {
                result.success = false;
                result.message = "company is already invited";
            }


            return result;
        }

        [ValidateHttpAntiForgeryToken]
        public JQueryPostResult PostLeave(int id)
        {
            JQueryPostResult result = new JQueryPostResult();
            BidPackageXInvitee invite = _service.Get(id);
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            if (invite.CompanyId == companyId)
            {
                invite.AcceptedDate = default(DateTime?);
                invite.RejectedDate = DateTime.Now;

                if (_service.Update(invite))
                {
                    result.message = "left project";
                    result.success = true;
                    result.data = new { date = invite.RejectedDate.Value.ToShortDateString() };
                }
                else
                {
                    result.success = false;
                    result.message = "unable to leave project";

                }
            }
            else
            {
                result.success = false;
                result.message = "invalid project"; // secretly means some how someone wound up posting the wrong invite id
            }


            return result;
        }

    }
}
