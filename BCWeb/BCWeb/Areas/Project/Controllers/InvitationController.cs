using BCModel.Projects;
using BCWeb.Areas.Project.Models.Invitation.ServiceLayer;
using BCWeb.Areas.Project.Models.Invitation.ViewModel;
using BCWeb.Helpers;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Project.Controllers
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
        // GET: /Project/Invitation/

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Project/Invitation/Send/1
        public ActionResult Send(int id)
        {
            BidPackageInvitationViewModel viewModel = new BidPackageInvitationViewModel();
            viewModel.BidPackageId = id;

            return View("Send",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(BidPackageInvitationViewModel viewModel)
        {
            try
            {
                List<BidPackageXInvitee> invites = new List<BidPackageXInvitee>();
                foreach (var c in viewModel.CompanyId)
                {
                    invites.Add(new BidPackageXInvitee { BidPackageId = viewModel.BidPackageId, CompanyId = c, Sent = DateTime.Now, InviteStatus = InviteStatus.Sent });
                }

                if (_service.CreateRange(invites))
                {
                    int projectId = _service.GetBidPackage(viewModel.BidPackageId).ProjectId;
                    return RedirectToRoute("Default", new { controller = "Project", action = "Details", id = projectId });
                }
                else
                {
                    Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                    return View("Send", viewModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return View("Send", viewModel);
            }

        }
    }
}
