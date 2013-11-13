using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.Bids.ServiceLayer;
using BCWeb.Areas.Project.Models.Bids.ViewModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Project.Controllers
{
    [Authorize]
    public class BidController : Controller
    {
        private IBidServiceLayer _service;
        private IWebSecurityWrapper _security;

        public BidController(IBidServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }
        //
        // GET: /Project/Bid/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "general_contractor,subcontractor,materials_vendor,Administrator")]
        [HttpGet]
        [HandleError(ExceptionType = typeof(Exception))]
        public ActionResult Compose(int projectId)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            // if GC
            if (User.IsInRole("general_contractor"))
            {
                ComposeGCViewModel viewModel = new ComposeGCViewModel();
                Invitation invite = _service.GetInvites(projectId, companyId).SingleOrDefault();

                if (invite != null)
                {
                    viewModel.ProjectId = projectId;
                    viewModel.ProjectName = _service.GetProject(projectId).Title;
                    viewModel.BaseBids = _service.GetBidPackageScopes(invite.BidPackageId)
                        .Select(s => new BaseBidItem
                        {
                            ScopeDescription = s.CsiNumber + " " + s.Description,
                            ScopeId = s.Id
                        });
                    return View("ComposeGC", viewModel);
                }
                else
                {
                    throw new Exception("company does not have invite to this project");
                }
            }
            // else if sub or vendor
            if (User.IsInRole("subcontractor") || User.IsInRole("materials_vendor"))
            {
                return View("ComposeSubAndVend");
            }

            return View();
        }

        [Authorize(Roles = "general_contractor,subcontractor,materials_vendor,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Compose(ComposeGCViewModel viewModel)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            if (ModelState.IsValid)
            {

            }
            else
            {
                return View("ComposeGC", viewModel);
            }
            throw new NotImplementedException();
        }

    }
}
