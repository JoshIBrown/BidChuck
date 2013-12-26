using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Account.Models.Company.ViewModel;
using BCWeb.Areas.Project.Models.Invitations.ServiceLayer;
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


        public IEnumerable<CompanyScopeItem> GetCompaniesToInvite(int bidPackageId)
        {
            IEnumerable<CompanyScopeItem> result = new List<CompanyScopeItem>();

            
            // exclude companies that have already been sent an invitation
            // if paid member, include all companies in operating radius
            // else only include members in social connection 

            return result;
        }

        [ValidateHttpAntiForgeryToken]
        public JQueryPostResult PostAccept([FromUri]int bidPackageId)
        {
            JQueryPostResult result = new JQueryPostResult();
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            Invitation invite = _service.Get(bidPackageId, companyId);

            if (invite.SentToId == companyId)
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
        public JQueryPostResult PostDecline([FromUri]int bidPackageId)
        {
            JQueryPostResult result = new JQueryPostResult();

            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            Invitation invite = _service.Get(bidPackageId, companyId);

            // make sure company responding is the same company that the invite is for
            if (invite.SentToId == companyId)
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
            if (_service.Get(bidPackage.Id, companyId) == null)
            {
                // assemble invite
                Invitation selfInvitation = new Invitation
                {

                    AcceptedDate = DateTime.Now,
                    BidPackage = bidPackage,
                    SentToId = companyId,
                    InvitationType = InvitationType.SelfInvite,
                    SentDate = DateTime.Now
                };

                // try and add invite to system.
                if (_service.Create(selfInvitation))
                {
                    result.success = true;
                    result.message = "joined project";
                    result.data = new { date = selfInvitation.AcceptedDate.Value.ToShortDateString() };
                }
                else
                {
                    result.success = false;
                    result.message = "unable to join project";
                }
            }
            else if (bidPackage.Invitees.Where(i => i.SentToId == companyId).Count() == 1)
            {
                var invite = _service.Get(bidPackageId, companyId);
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
        public JQueryPostResult PostLeave(int bidPackageId)
        {
            JQueryPostResult result = new JQueryPostResult();
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            Invitation invite = _service.Get(bidPackageId, companyId);


            if (invite.SentToId == companyId)
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
