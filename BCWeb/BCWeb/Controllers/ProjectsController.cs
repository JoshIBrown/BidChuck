using BCModel;
using BCModel.Projects;
using BCWeb.Helpers;
using BCWeb.Models;
using BCWeb.Models.Project.ServiceLayer;
using BCWeb.Models.Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        //
        // GET: /Projects/
        private IProjectServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ProjectsController(IProjectServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [Authorize(Roles = "general_contractor,architect,Administrator")]
        [HttpGet]
        public ActionResult Create()
        {
            EditProjectViewModel viewModel = new EditProjectViewModel();

            rePopViewModel(viewModel);
            return View("Create", viewModel);
        }

        [Authorize(Roles = "general_contractor,architect,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int userId = _security.GetUserId(User.Identity.Name);
                    int companyId = _service.GetUserProfile(userId).CompanyId;

                    // create project
                    BCModel.Projects.Project toCreate = new BCModel.Projects.Project
                    {
                        Address = viewModel.Address,
                        ArchitectId = companyId,
                        BidDateTime = viewModel.BidDateTime,
                        BuildingTypeId = viewModel.BuildingTypeId,
                        City = viewModel.City,
                        ConstructionTypeId = viewModel.ConstructionTypeId,
                        CreatedById = userId,
                        Description = viewModel.Description,
                        PostalCode = viewModel.PostalCode,
                        ProjectType = viewModel.ProjectType,
                        StateId = viewModel.StateId,
                        Title = viewModel.Title,
                        Scopes = new List<ProjectXScope>(),
                        BidPackages = new List<BidPackage>()
                    };
                    // create master bid package
                    BidPackage projectPackage = new BidPackage
                    {
                        IsMaster = true,
                        BidDateTime = toCreate.BidDateTime,
                        CreatedById = companyId,
                        Project = toCreate,
                        Scopes = new List<BidPackageXScope>()
                    };
                    // add bp to project
                    toCreate.BidPackages.Add(projectPackage);

                    // set selected scopes for bp and project
                    for (int i = 0; i < viewModel.SelectedScope.Count(); i++)
                    {
                        toCreate.Scopes.Add(new ProjectXScope { Project = toCreate, ScopeId = viewModel.SelectedScope.ElementAt(i) });
                        projectPackage.Scopes.Add(new BidPackageXScope { BidPackage = projectPackage, ScopeId = viewModel.SelectedScope.ElementAt(i) });
                    }

                    // add project to system
                    if (_service.Create(toCreate))
                    {
                        return RedirectToRoute("Default", new { controller = "Projects", action = "Details", id = toCreate.Id });
                    }
                    else
                    {
                        Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                        rePopViewModel(viewModel);
                        return View("Create", viewModel);
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("Exception", ex.Message);
                    rePopViewModel(viewModel);
                    return View("Create", viewModel);
                }
            }

            // modelstate is not valid
            rePopViewModel(viewModel);
            return View("Create", viewModel);
        }

        private void rePopViewModel(EditProjectViewModel viewModel)
        {


            viewModel.ConstructionTypes = _service.GetConstructionTypes().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id == viewModel.ConstructionTypeId });
            //viewModel.ProjectTypes = 
            viewModel.States = _service.GetStates().OrderBy(s => s.Abbr).Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString(), Selected = s.Id == viewModel.StateId });
            viewModel.BuildingTypes = _service.GetBuildingTypes();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var raw = _service.Get(id);
            ProjectDetailsViewModel viewModel = new ProjectDetailsViewModel
            {
                Address = raw.Address,
                BidDateTime = raw.BidDateTime,
                BuildingType = raw.BuildingType.Name,
                City = raw.City,
                ConstructionType = raw.ConstructionType.Name,
                Description = raw.Description,
                Id = raw.Id,
                PostalCode = raw.PostalCode,
                ProjectType = raw.ProjectType.ToDescription(),
                SelectedScope = raw.Scopes.Select(s => s.Scope.CsiNumber + " " + s.Scope.Description).OrderBy(s => s),
                State = raw.State.Abbr,
                Title = raw.Title
            };
            return View("Details", viewModel);
        }

        // GET: /Projects/Edit/3
        public ActionResult Edit(int id)
        {
            var raw = _service.Get(id);
            EditProjectViewModel viewModel = new EditProjectViewModel
            {
                Address = raw.Address,
                BidDateTime = raw.BidDateTime,
                BuildingTypeId = raw.BuildingTypeId,
                City = raw.City,
                ConstructionTypeId = raw.ConstructionTypeId,
                Description = raw.Description,
                Id = raw.Id,
                PostalCode = raw.PostalCode,
                StateId = raw.StateId,
                Title = raw.Title
            };
            viewModel.SelectedScope = raw.Scopes.Select(x => x.ScopeId).ToList();
            viewModel.States = _service.GetStates().OrderBy(s => s.Abbr).Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString(), Selected = s.Id == viewModel.StateId });
            viewModel.BuildingTypes = _service.GetBuildingTypes();
            viewModel.ConstructionTypes = _service.GetConstructionTypes().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id == viewModel.ConstructionTypeId });
            return View("Edit", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    int userId = _security.GetUserId(User.Identity.Name);
                    int companyId = _service.GetUserProfile(userId).CompanyId;

                    // get project
                    Project toUpdate = _service.Get(viewModel.Id);

                    // update project attributes
                    if (toUpdate.Title != viewModel.Title)
                        toUpdate.Title = viewModel.Title;

                    if (toUpdate.Description != viewModel.Description)
                        toUpdate.Description = viewModel.Description;

                    if (toUpdate.Address != viewModel.Address)
                        toUpdate.Address = viewModel.Address;

                    if (toUpdate.BidDateTime != viewModel.BidDateTime)
                        toUpdate.BidDateTime = viewModel.BidDateTime;

                    if (toUpdate.BuildingTypeId != viewModel.BuildingTypeId)
                        toUpdate.BuildingTypeId = viewModel.BuildingTypeId;

                    if (toUpdate.City != viewModel.City)
                        toUpdate.City = viewModel.City;

                    if (toUpdate.ConstructionTypeId != viewModel.ConstructionTypeId)
                        toUpdate.ConstructionTypeId = viewModel.ConstructionTypeId;

                    if (toUpdate.PostalCode != viewModel.PostalCode)
                        toUpdate.PostalCode = viewModel.PostalCode;

                    if (toUpdate.ProjectType != viewModel.ProjectType)
                        toUpdate.ProjectType = viewModel.ProjectType;

                    if (toUpdate.StateId != viewModel.StateId)
                        toUpdate.StateId = viewModel.StateId;

                    // update primary bid package
                    BidPackage ProjectPackage = toUpdate.BidPackages.Where(b => b.IsMaster).FirstOrDefault();

                    // if it doesn't exist for some reason, create it
                    if (ProjectPackage == null)
                    {
                        ProjectPackage = new BidPackage
                        {
                            BidDateTime = toUpdate.BidDateTime,
                            CreatedById = toUpdate.ArchitectId,
                            IsMaster = true,
                            ProjectId = toUpdate.Id
                        };
                        toUpdate.BidPackages = new List<BidPackage>();
                        toUpdate.BidPackages.Add(ProjectPackage);
                    }

                    // update scopes
                    List<ProjectXScope> prjScopes = toUpdate.Scopes.ToList();
                    List<BidPackageXScope> bpScopes;

                    // error correction
                    if (ProjectPackage.Scopes == null)
                    {
                        var temp = new List<BidPackageXScope>();
                        ProjectPackage.Scopes = new List<BidPackageXScope>();
                        for (int i = 0; i < prjScopes.Count(); i++)
                        {
                            ProjectPackage.Scopes.Add(new BidPackageXScope { BidPackage = ProjectPackage, BidPackageId = ProjectPackage.Id, ScopeId = prjScopes[i].ScopeId });
                        }
                        ProjectPackage.Scopes = temp;
                        bpScopes = temp;
                    }
                    else
                    {
                        bpScopes = ProjectPackage.Scopes.ToList();

                    }

                    // get scopes from new selection that are not in existing selection
                    var scopesToAdd = viewModel.SelectedScope.Where(x => !prjScopes.Select(s => s.ScopeId).Contains(x)).ToArray();

                    // get scopes in existing selection that are not in new selection
                    var prjScopesToRemove = prjScopes.Where(x => !viewModel.SelectedScope.Contains(x.ScopeId)).ToArray();
                    var bpScopesToRemove = bpScopes.Where(x => !viewModel.SelectedScope.Contains(x.ScopeId)).ToArray();

                    // add new selections
                    for (int i = 0; i < scopesToAdd.Count(); i++)
                    {
                        prjScopes.Add(new ProjectXScope { Project = toUpdate, ScopeId = scopesToAdd[i] });
                        bpScopes.Add(new BidPackageXScope { BidPackage = ProjectPackage, ScopeId = scopesToAdd[i] });
                    }

                    // remove scopes from project
                    for (int i = 0; i < prjScopesToRemove.Count(); i++)
                    {
                        prjScopes.Remove(prjScopesToRemove[i]); ;
                    }

                    // remove scope from master bid package
                    for (int i = 0; i < bpScopesToRemove.Count(); i++)
                    {
                        bpScopes.Remove(bpScopesToRemove[i]);
                    }

                    // add project to system
                    if (_service.Update(toUpdate))
                    {
                        return RedirectToRoute("Default", new { controller = "Projects", action = "Details", id = toUpdate.Id });
                    }
                    else
                    {
                        Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                        rePopViewModel(viewModel);
                        return View("Edit", viewModel);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                    rePopViewModel(viewModel);
                    return View("Edit", viewModel);
                }
            }
            else
            {
                //viewModel.SelectedScope = raw.Scopes.Select(x => x.ScopeId).ToList();
                rePopViewModel(viewModel);
                return View("Edit", viewModel);
            }
        }
    }
}
