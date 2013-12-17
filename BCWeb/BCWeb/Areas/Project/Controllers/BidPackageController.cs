using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.BidPackage.ServiceLayer;
using BCWeb.Areas.Project.Models.BidPackage.ViewModel;
using BCWeb.Helpers;
using BCWeb.Models;
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

        public BidPackageController(IBidPackageServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }


        //
        // GET: /Projects/BidPackage/

        public ActionResult Index(int projectId)
        {
            UserProfile user = _service.GetUser(_security.GetUserId(User.Identity.Name));

            IEnumerable<BidPackageListItemViewModel> bps = _service.GetEnumerableByProjectAndCreatingCompany(projectId, user.CompanyId)
                .Select(bp => new BidPackageListItemViewModel
                {
                    BidDateTime = bp.BidDateTime.ToString(),
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
                BidDateTime = raw.BidDateTime.ToString(),
                CreatingCompany = raw.CreatedBy.CompanyName,
                Description = raw.Description,
                Id = raw.Id,
                Notes = raw.Notes,
                ProjectId = raw.ProjectId,
                ProjectName = raw.Project.Title,
                WalkThruDateTime = ""
            };

            viewModel.Scopes = raw.Scopes.Select(s => s.Scope.CsiNumber + " " + s.Scope.Description).OrderBy(s => s).ToList();

            return View("Details", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var raw = _service.Get(id);
            EditBidPackageViewModel viewModel = new EditBidPackageViewModel
            {
                //BidDateTime = raw.BidDateTime, // FIXME
                Description = raw.Description,
                Id = raw.Id,
                Notes = raw.Notes,
                ProjectId = raw.ProjectId,
                TemplateId = raw.TemplateBidPackageId
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBidPackageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int userId = _security.GetUserId(User.Identity.Name);
                int companyId = _service.GetUser(userId).CompanyId;

                try
                {
                    BidPackage toUpdate = _service.Get(viewModel.Id);

                    toUpdate.BidDateTime = viewModel.BidDateTime;
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
