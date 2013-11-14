using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.Bids.ServiceLayer;
using BCWeb.Areas.Project.Models.Bids.ViewModel;
using BCWeb.Helpers;
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

        [Authorize(Roles = "general_contractor,Administrator")]
        [HttpGet]
        [HandleError(ExceptionType = typeof(Exception))]
        public ActionResult ComposeGC(int projectId)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            Invitation invite = _service.GetInvites(projectId, companyId).SingleOrDefault();

            // block user from editing bid once submitted
            if (invite.BidSentDate.HasValue)
            {
                return RedirectToRoute("Project_default", new { controller = "Bid", action = "ReviewGC", projectId = projectId });
            }

            GCBidEditModel viewModel = new GCBidEditModel();

            // if user has an invite
            if (invite != null)
            {
                viewModel.ProjectId = projectId;
                viewModel.ProjectName = _service.GetProject(projectId).Title;
                viewModel.BidPackageId = invite.BidPackageId;
                IEnumerable<BaseBid> baseBids = _service.GetCompanyBaseBidsForProject(companyId, projectId);

                // if there isn't a saved draft
                if (baseBids == null || baseBids.Count() == 0)
                {
                    viewModel.BaseBids = _service.GetBidPackageScopes(invite.BidPackageId)
                        .Select(s => new BaseBidEditItem
                        {
                            ScopeDescription = s.CsiNumber + " " + s.Description,
                            ScopeId = s.Id
                        });
                }
                else
                {
                    viewModel.BaseBids = baseBids.Select(b => new BaseBidEditItem
                    {
                        ScopeDescription = b.Scope.CsiNumber + " " + b.Scope.Description,
                        ScopeId = b.ScopeId,
                        Amount = b.Amount
                    });
                }
                return View(viewModel);
            }
            else
            {
                throw new Exception("company does not have invite to this project");
            }
        }

        [Authorize(Roles = "general_contractor,Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ComposeGC(GCBidEditModel viewModel)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            if (ModelState.IsValid)
            {

                IEnumerable<BaseBid> baseBids = viewModel.BaseBids.Select(b => new BaseBid { Amount = b.Amount, ProjectId = viewModel.ProjectId, SentToId = companyId, ScopeId = b.ScopeId });
                IEnumerable<ComputedBid> computedBids = viewModel.BaseBids.Select(b => new ComputedBid { RiskFactor = 1.00m, BidPackageId = viewModel.BidPackageId, SentToId = companyId, ScopeId = b.ScopeId });

                Dictionary<int, IEnumerable<ComputedBid>> computedBidDic = new Dictionary<int, IEnumerable<ComputedBid>>();
                computedBidDic.Add(viewModel.BidPackageId, computedBids);

                switch (viewModel.btn)
                {
                    case "Save":
                        if (_service.SaveDraft(baseBids, computedBidDic))
                        {
                            return View(viewModel);
                        }
                        else
                        {
                            Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                            return View(viewModel);
                        }
                    case "Submit":
                        if (_service.SaveFinalBid(baseBids, computedBidDic, companyId, DateTime.Now))
                        {
                            return RedirectToRoute("Project_default", new { controlle = "Bid", action = "ReviewGC", projectId = viewModel.ProjectId });
                        }
                        else
                        {
                            Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                            return View(viewModel);
                        }
                    default:
                        throw new Exception("invalid action");
                }

            }
            else
            {
                return View(viewModel);
            }
        }

        [HttpGet, Authorize(Roles = "general_contractor,Administrator")]
        public ActionResult ReviewGC(int projectId)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            Invitation invite = _service.GetInvites(projectId, companyId).SingleOrDefault();
            GCBidViewModel viewModel = new GCBidViewModel();
            viewModel.ProjectId = invite.BidPackage.ProjectId;
            viewModel.BaseBids = _service.GetCompanyBaseBidsForProject(companyId, projectId).Select(t => new BaseBidViewItem { Amount = t.Amount, ScopeDescription = t.Scope.CsiNumber + " " + t.Scope.Description });
            return View(viewModel);
        }

        [Authorize(Roles = "subcontractor,materials_vendor,Administrator")]
        [HttpGet]
        public ActionResult ComposeSV(int projectId)
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "subcontractor,materials_vendor,Administrator")]
        [HttpPost, ValidateAntiForgeryTokenAttribute]
        public ActionResult ComposeSV(SubVendBidViewModel viewModel)
        {
            throw new NotImplementedException();
        }


    }
}
