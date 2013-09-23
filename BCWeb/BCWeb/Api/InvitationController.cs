using BCWeb.Areas.Project.Models.Invitation.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    public class InvitationController : ApiController
    {
        private IInvitationServiceLayer _service;

        public InvitationController(IInvitationServiceLayer service)
        {
            _service = service;
        }
    }
}
