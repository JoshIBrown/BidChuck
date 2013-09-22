using BCWeb.Areas.Projects.Models.Invitation.ServiceLayer;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Projects.Controllers
{
    public class InvitationController : Controller
    {
        private IInvitationServiceLayer _service;
        private IWebSecurityWrapper _security;
        private IEmailSender _email;

        public InvitationController(IInvitationServiceLayer service, IWebSecurityWrapper security, IEmailSender email)
        {
            _service = service;
            _security = security;
            _email = email;
        }

        //
        // GET: /Projects/Invitation/

        public ActionResult Index()
        {
            return View();
        }

    }
}
