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

        [HttpGet]
        public ActionResult Create()
        {
            EditProjectViewModel viewModel = new EditProjectViewModel();

            viewModel.ConstructionTypes = _service.GetConstructionTypes().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }); ;
            viewModel.ProjectTypes = _service.GetProjectTypes().OrderBy(p => p.Name).Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
            viewModel.States = _service.GetStates().OrderBy(s => s.Abbr).Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString() }); ;
            viewModel.BuildingTypes = _service.GetBuildingTypes();
            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditProjectViewModel viewModel)
        {
            return View("Create");
        }
    }
}
