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

            int companyId = _service.GetUerProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            BidPackageXInvitee invite = _service.Get(id);
            if (invite.CompanyId == companyId)
            {
                invite.AcceptedDate = DateTime.Now;
                if (_service.Update(invite))
                {
                    result.success = true;
                    result.message = "invitation accepted";
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

            int companyId = _service.GetUerProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            BidPackageXInvitee invite = _service.Get(id);
            if (invite.CompanyId == companyId)
            {
                invite.RejectedDate = DateTime.Now;
                if (_service.Update(invite))
                {
                    result.success = true;
                    result.message = "invitation declined";
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
    }
}
