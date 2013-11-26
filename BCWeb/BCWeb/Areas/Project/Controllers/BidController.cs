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



        public ActionResult Received(int projectId)
        {
            var project = _service.GetProject(projectId);
            ReceivedBidViewModel viewModel = new ReceivedBidViewModel
            {
                ProjectId = projectId,
                ProjectTitle = project.Title
            };
            return View(viewModel);
        }

        [Authorize(Roles = "general_contractor,Administrator")]
        [HttpGet]
        [HandleError(ExceptionType = typeof(Exception))]
        public ActionResult ComposeGC(int projectId)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            // get invitation to project
            Invitation invite = _service.GetInvites(projectId, companyId).SingleOrDefault();

            // if passed bid date, block changes
            if (invite.BidPackage.BidDateTime < DateTime.Now)
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

                // FIXME: need to account for scope changes after a draft is saved
                // if there isn't a saved draft
                if (baseBids == null || baseBids.Count() == 0)
                {
                    viewModel.BaseBids = _service.GetBidPackageScopes(invite.BidPackageId)
                        .OrderBy(s => s.CsiNumber)
                        .Select(s => new BaseBidEditItem
                        {
                            ScopeDescription = s.CsiNumber + " " + s.Description,
                            ScopeId = s.Id
                        });
                }
                else
                {
                    viewModel.BaseBids = baseBids
                        .OrderBy(b => b.Scope.CsiNumber)
                        .Select(b => new BaseBidEditItem
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

                // get invitation to project
                Invitation invite = _service.GetInvites(viewModel.ProjectId, companyId).SingleOrDefault();

                // if passed bid date, block changes
                if (invite.BidPackage.BidDateTime < DateTime.Now)
                {
                    throw new Exception("passed bid date");
                }

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
            viewModel.ProjectTitle = invite.BidPackage.Project.Title;
            viewModel.BaseBids = _service.GetCompanyBaseBidsForProject(companyId, projectId)
                .OrderBy(s => s.Scope.CsiNumber)
                .Select(t => new BaseBidViewItem
                {
                    Amount = t.Amount,
                    ScopeDescription = t.Scope.CsiNumber + " " + t.Scope.Description
                });
            return View(viewModel);
        }

        [Authorize(Roles = "subcontractor,materials_vendor,Administrator")]
        [HttpGet]
        public ActionResult ComposeSV(int projectId)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            // get invitations company has accepted
            List<Invitation> invitations = _service.GetInvitesCompanyHasAccepted(projectId, companyId).ToList();

            SVBidEditModel viewModel = new SVBidEditModel();

            BCModel.Projects.Project theProject = _service.GetProject(projectId);
            viewModel.ProjectId = projectId;
            viewModel.ProjectName = theProject.Title;

            // get list of base bids if they've been saved
            IEnumerable<BaseBid> baseBids = _service.GetCompanyBaseBidsForProject(companyId, projectId);

            // get available scopes
            IEnumerable<Scope> companyScopes = _service.GetCompanyScopesForProject(projectId, companyId).OrderBy(o => o.CsiNumber);

            // FIXME: need to account for scope changes after a draft is saved
            // if there isn't a saved draft
            if (baseBids == null || baseBids.Count() == 0)
            {
                viewModel.BaseBids = companyScopes
                    .OrderBy(o => o.CsiNumber)
                    .Select(s => new BaseBidEditItem
                    {
                        ScopeDescription = s.CsiNumber + " " + s.Description,
                        ScopeId = s.Id
                    }).ToList();
            }
            else
            {
                viewModel.BaseBids = baseBids
                    .OrderBy(b => b.Scope.CsiNumber)
                    .Select(b => new BaseBidEditItem
                {
                    ScopeDescription = b.Scope.CsiNumber + " " + b.Scope.Description,
                    ScopeId = b.ScopeId,
                    Amount = b.Amount
                }).ToList();
            }

            // create bid packages
            List<SVBidPackageItem> bidPackages = new List<SVBidPackageItem>();
            SVBidPackageItem bidPackage;

            // loop through bid packages
            for (int b = 0; b < invitations.Count; b++)
            {
                bidPackage = new SVBidPackageItem();
                bidPackage.BidPacakgeId = invitations[b].BidPackageId;
                bidPackage.CompanyName = invitations[b].BidPackage.CreatedBy.CompanyName;

                // get computed bids
                bidPackage.ComputedBids = _service.GetCompanyComputedBidsForBidPackage(bidPackage.BidPacakgeId, companyId)
                    .OrderBy(o => o.Scope.CsiNumber)
                    .Select(c => new ComputedBidEditItem { RiskFactor = c.RiskFactor.HasValue ? c.RiskFactor.Value : 0.00m, ScopeId = c.ScopeId })
                    .ToList();

                // if no prior computed bids
                if (bidPackage.ComputedBids.Count == 0 && companyScopes.Count() > 0)
                {
                    // get bid package scopes, and assemble empty computed bids
                    bidPackage.ComputedBids = _service.GetBidPackageScopes(bidPackage.BidPacakgeId)
                        .OrderBy(s => s.CsiNumber)
                        .Select(s => new ComputedBidEditItem { ScopeId = s.Id })
                        .ToList();
                }

                bidPackages.Add(bidPackage);
            }
            viewModel.BidPackages = bidPackages;

            return View(viewModel);
        }

        [Authorize(Roles = "subcontractor,materials_vendor,Administrator")]
        [HttpPost, ValidateAntiForgeryTokenAttribute]
        public ActionResult ComposeSV(SVBidEditModel viewModel)
        {
            throw new NotImplementedException();
        }


    }
}
