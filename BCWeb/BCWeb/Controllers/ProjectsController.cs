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
                    BCModel.Projects.Project toCreate = new BCModel.Projects.Project
                    {
                        Address = viewModel.Address,
                        ArchitectId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId,
                        BidDateTime = viewModel.BidDateTime,
                        BuildingTypeId = viewModel.BuildingTypeId,
                        City = viewModel.City,
                        ConstructionTypeId = viewModel.ConstructionTypeId,
                        CreatedById = _security.GetUserId(User.Identity.Name),
                        Description = viewModel.Description,
                        PostalCode = viewModel.PostalCode,
                        ProjectType = viewModel.ProjectType,
                        StateId = viewModel.StateId,
                        Title = viewModel.Title,
                        Scopes = new List<ProjectXScope>(),
                        BidPackages = new List<BidPackage>()
                    };


                    for (int i = 0; i < viewModel.SelectedScope.Count(); i++)
                    {
                        toCreate.Scopes.Add(new ProjectXScope { Project = toCreate, ScopeId = viewModel.SelectedScope.ElementAt(i) });
                    }

                    UserProfile user = _service.GetUserProfile(_security.GetUserId(User.Identity.Name));

                    toCreate.BidPackages.Add(new BidPackage
                    {
                        Project = toCreate,
                        CreatorId = user.CompanyId,
                        Scopes = new List<BidPackageXScope>()
                    });
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


            viewModel.ConstructionTypes = _service.GetConstructionTypes().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id == viewModel.ConstructionTypeId }); ;
            //viewModel.ProjectTypes = 
            viewModel.States = _service.GetStates().OrderBy(s => s.Abbr).Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString(), Selected = s.Id == viewModel.StateId }); ;
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

        public ActionResult Edit(int id)
        {
            var raw = _service.Get(id);
            EditProjectViewModel viewModel = new EditProjectViewModel
            {
            };
            return View();
        }
    }
}
