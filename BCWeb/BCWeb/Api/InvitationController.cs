using BCModel;
using BCWeb.Areas.Project.Models.Invitation.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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


    }
}
