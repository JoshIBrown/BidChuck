using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.BidPackage.ServiceLayer;
using BCWeb.Areas.Project.Models.BidPackage.ViewModel;
using BCWeb.Helpers;
using BCWeb.Models;
using BCWeb.Models.Notifications.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Project.Controllers
{
    public class BidPackageController : Controller
    {

        private IBidPackageServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;

        public BidPackageController(IBidPackageServiceLayer service, IWebSecurityWrapper security, INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
        }


        //
        // GET: /Projects/BidPackage/

        public ActionResult Index(int projectId)
        {
            UserProfile user = _service.GetUser(_security.GetUserId(User.Identity.Name));

            IEnumerable<BidPackageListItemViewModel> bps = _service.GetEnumerableByProjectAndCreatingCompany(projectId, user.CompanyId)
                .Select(bp => new BidPackageListItemViewModel
                {
                    BidDateTime = bp.UseProjectBidDateTime ? bp.Project.BidDateTime.ToString() : bp.BidDateTime.ToString(),
                    Description = bp.Description,
                    Id = bp.Id,
                    Invited = bp.Invitees == null ? 0 : bp.Invitees.Count(),
                    Accepted = bp.Invitees == null ? 0 : bp.Invitees.Where(i => i.AcceptedDate.HasValue).Count(),
                    Declined = bp.Invitees == null ? 0 : bp.Invitees.Where(i => i.RejectedDate.HasValue).Count()
                });

            BCModel.Projects.Project project = _service.GetProject(projectId);
            ProjectBidPackagesViewModel viewModel = new ProjectBidPackagesViewModel { ProjectId = projectId, ProjectName = project.Title, BidPackages = bps, };

            return View("Index", viewModel);
        }

        //
        // GET: /Projects/BidPackage/Create
        [Authorize(Roles = "general_contractor,subcontractor,materials_vendor")]
        public ActionResult Create(int projectId, int templateId)
        {
            EditBidPackageViewModel viewModel = new EditBidPackageViewModel();
            viewModel.ProjectId = projectId;
            viewModel.TemplateId = templateId;
            viewModel.BidDateTime = DateTime.Now;
            viewModel.UseProjectBidDate = true;
            viewModel.UseProjectWalkThru = true;
            return View("Create", viewModel);
        }

        [HttpPost]
        public ActionResult Create(EditBidPackageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int userId = _security.GetUserId(User.Identity.Name);
                int companyId = _service.GetUser(userId).CompanyId;

                try
                {
                    BidPackage toCreate = new BidPackage
                            {
                                BidDateTime = viewModel.BidDateTime,
                                CreatedById = companyId,
                                Description = viewModel.Description,
                                IsMaster = false,
                                ProjectId = viewModel.ProjectId,
                                TemplateBidPackageId = viewModel.TemplateId,
                                Notes = viewModel.Notes
                            };

                    toCreate.Scopes = new List<BidPackageXScope>();

                    for (int i = 0; i < viewModel.SelectedScope.Count(); i++)
                    {
                        toCreate.Scopes.Add(new BidPackageXScope { BidPackage = toCreate, ScopeId = viewModel.SelectedScope.ElementAt(i) });
                    }

                    if (_service.Create(toCreate))
                    {
                        return RedirectToRoute("Project_default", new { controller = "BidPackage", action = "Details", id = toCreate.Id });
                    }
                    else
                    {
                        Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                        return View("Create", viewModel);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                    return View("Create", viewModel);
                }

            }
            else
            {
                return View("Create", viewModel);
            }
        }

        public ActionResult Details(int id)
        {
            var raw = _service.Get(id);
            BidPackageDetailsViewModel viewModel = new BidPackageDetailsViewModel
            {
                Architect = raw.Project.Architect.CompanyName,
                BidDateTime = raw.UseProjectBidDateTime ? raw.Project.BidDateTime.ToString() : raw.BidDateTime.Value.ToString(),
                CreatingCompany = raw.CreatedBy.CompanyName,
                Description = raw.Description,
                Id = raw.Id,
                Notes = raw.Notes,
                ProjectId = raw.ProjectId,
                ProjectName = raw.Project.Title,
                WalkThruDateTime = raw.UseProjectWalkThruDateTime ?
                    raw.Project.WalkThruStatus == WalkThruStatus.WalkThruIncluded ?
                        raw.Project.WalkThruDateTime.Value.ToString() : raw.Project.WalkThruStatus.ToString() :
                    raw.WalkThruStatus.HasValue ?
                    raw.WalkThruStatus.Value == WalkThruStatus.WalkThruIncluded ?
                        raw.WalkThruDateTime.Value.ToString() : raw.WalkThruStatus.Value.ToString() : "Unspecified"
            };

            viewModel.Scopes = raw.Scopes.Select(s => s.Scope.CsiNumber + " " + s.Scope.Description).OrderBy(s => s).ToList();

            return View("Details", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var raw = _service.Get(id);
            EditBidPackageViewModel viewModel = new EditBidPackageViewModel
            {
                Description = raw.Description,
                Id = raw.Id,
                Notes = raw.Notes,
                ProjectId = raw.ProjectId,
                TemplateId = raw.TemplateBidPackageId,
                UseProjectWalkThru = raw.UseProjectWalkThruDateTime,
                UseProjectBidDate = raw.UseProjectBidDateTime,
                BidDateTime = raw.UseProjectBidDateTime ? default(DateTime?) : raw.BidDateTime.Value,
                WalkThruStatus = raw.UseProjectWalkThruDateTime ? default(WalkThruStatus?) : raw.WalkThruStatus.HasValue ? raw.WalkThruStatus.Value : default(WalkThruStatus?),
                WalkThruDateTime = raw.WalkThruStatus.HasValue && raw.WalkThruStatus.Value == WalkThruStatus.WalkThruIncluded ? raw.WalkThruDateTime.Value : default(DateTime?)
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBidPackageViewModel viewModel)
        {
            if (!viewModel.UseProjectBidDate && !viewModel.BidDateTime.HasValue)
                ModelState.AddModelError("BidDateTime", "Pleae provide a Bid Date and Time if not use the Project's Bid Date and Time");

            if (!viewModel.UseProjectWalkThru && !viewModel.WalkThruStatus.HasValue)
                ModelState.AddModelError("WalkThruStatus", "Please Select a Walk Through option if not using Project's Walk Through settings");

            if (!viewModel.UseProjectWalkThru &&
                viewModel.WalkThruStatus.HasValue &&
                viewModel.WalkThruStatus.Value == WalkThruStatus.WalkThruIncluded &&
                !viewModel.WalkThruDateTime.HasValue)
                ModelState.AddModelError("WalkThruDateTime", "Please set a Walk Through Date and Time");


            if (ModelState.IsValid)
            {
                int userId = _security.GetUserId(User.Identity.Name);
                int companyId = _service.GetUser(userId).CompanyId;

                try
                {
                    BidPackage toUpdate = _service.Get(viewModel.Id);

                    if (!viewModel.UseProjectBidDate && viewModel.BidDateTime.HasValue)
                        toUpdate.BidDateTime = viewModel.BidDateTime.Value;
                    else
                        toUpdate.BidDateTime = default(DateTime?);

                    if (!viewModel.UseProjectWalkThru && viewModel.WalkThruStatus.HasValue)
                        toUpdate.WalkThruStatus = viewModel.WalkThruStatus.Value;
                    else
                        toUpdate.WalkThruStatus = default(WalkThruStatus?);

                    if (!viewModel.UseProjectWalkThru &&
                        viewModel.WalkThruStatus.HasValue &&
                        viewModel.WalkThruStatus.Value == WalkThruStatus.WalkThruIncluded &&
                        viewModel.WalkThruDateTime.HasValue)
                        toUpdate.WalkThruDateTime = viewModel.WalkThruDateTime.Value;
                    else
                    {
                        toUpdate.WalkThruDateTime = default(DateTime?);
                    }

                    toUpdate.UseProjectBidDateTime = viewModel.UseProjectBidDate;
                    toUpdate.UseProjectWalkThruDateTime = viewModel.UseProjectWalkThru;
                    toUpdate.Description = viewModel.Description;
                    toUpdate.Notes = viewModel.Notes;

                    // get scopes that have been added
                    IEnumerable<int> toAdd = viewModel.SelectedScope.Where(s => !toUpdate.Scopes.Select(x => x.ScopeId).Contains(s));

                    // get scopes that have been removed
                    IEnumerable<BidPackageXScope> toRemove = toUpdate.Scopes.Where(s => !viewModel.SelectedScope.Contains(s.ScopeId));


                    // add new scopes
                    for (int i = 0; i < toAdd.Count(); i++)
                    {
                        toUpdate.Scopes.Add(new BidPackageXScope { BidPackage = toUpdate, ScopeId = toAdd.ElementAt(i) });
                    }

                    // remove scopes
                    for (int i = 0; i < toRemove.Count(); i++)
                    {
                        toUpdate.Scopes.Remove(toRemove.ElementAt(i));
                    }

                    // try to update db record
                    if (_service.Update(toUpdate))
                    {

                        // notify invited companies
                        int[] invitees = _notice.GetInvitationsNotDeclined(toUpdate.ProjectId, companyId).Select(i => i.SentToId).ToArray();

                        for (int i = 0; i < invitees.Length; i++)
                        {
                            _notice.SendNotification(invitees[i], RecipientType.company, NotificationType.ProjectChange, toUpdate.ProjectId);
                        }
                        return RedirectToRoute("Project_default", new { controller = "BidPackage", action = "Details", id = toUpdate.Id });
                    }
                    else
                    {
                        Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                        return View("Edit", viewModel);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                    return View("Edit", viewModel);
                }

            }
            else
            {
                return View("Edit", viewModel);
            }
        }
    }
}
