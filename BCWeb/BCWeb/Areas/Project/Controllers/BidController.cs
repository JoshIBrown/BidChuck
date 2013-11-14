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

        [Authorize(Roles = "general_contractor,Administrator")]
        [HttpGet]
        [HandleError(ExceptionType = typeof(Exception))]
        public ActionResult ComposeGC(int projectId)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            ComposeGCViewModel viewModel = new ComposeGCViewModel();
            Invitation invite = _service.GetInvites(projectId, companyId).SingleOrDefault();

            // TODO: pull saved info from server. check if bid was already submitted

            if (invite != null)
            {
                viewModel.ProjectId = projectId;
                viewModel.ProjectName = _service.GetProject(projectId).Title;
                viewModel.BidPackageId = invite.BidPackageId;
                IEnumerable<BaseBid> baseBids = _service.GetCompanyBaseBidsForProject(companyId, projectId);
                if (baseBids == null || baseBids.Count() == 0)
                {
                    viewModel.BaseBids = _service.GetBidPackageScopes(invite.BidPackageId)
                        .Select(s => new BaseBidItem
                        {
                            ScopeDescription = s.CsiNumber + " " + s.Description,
                            ScopeId = s.Id
                        });
                }
                else
                {
                    viewModel.BaseBids = baseBids.Select(b => new BaseBidItem
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
        public ActionResult ComposeGC(ComposeGCViewModel viewModel)
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
                        break;
                    case "Submit":
                        //if (_service.SaveFinalBid(baseBids, computedBidDic, companyId, DateTime.Now))
                        //{
                        //}
                        break;
                    default:
                        break;
                }


                return View(viewModel);
            }
            else
            {
                return View(viewModel);
            }
        }

        [HttpGet, Authorize(Roles = "general_contractor,Administrator")]
        public ActionResult ReviewGC(int projectId)
        {
            return View();
        }

        [Authorize(Roles = "subcontractor,materials_vendor,Administrator")]
        [HttpGet]
        public ActionResult ComposeSV(int projectId)
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "subcontractor,materials_vendor,Administrator")]
        [HttpPost, ValidateAntiForgeryTokenAttribute]
        public ActionResult ComposeSV(ComposeSubVendViewModel viewModel)
        {
            throw new NotImplementedException();
        }


    }
}
