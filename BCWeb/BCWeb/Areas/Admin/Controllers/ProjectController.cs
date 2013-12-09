using BCModel.Projects;
using BCWeb.Areas.Admin.Models.Projects;
using BCWeb.Helpers;
using BCWeb.Models.Project.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProjectController : Controller
    {
        private IProjectServiceLayer _service;

        public ProjectController(IProjectServiceLayer service)
        {
            _service = service;
        }


        //
        // GET: /Admin/Project/
        public ActionResult Index()
        {
            return View();
        }


        // GET: /Admin/Project/Create
        public ActionResult Create()
        {
            ProjectEditModel viewModel = new ProjectEditModel();

            viewModel.States = _service.GetStates().Select(s => new SelectListItem { Text = s.Abbr, Value = s.Abbr.ToString() });
            viewModel.ProjectTypes = Util.CreateSelectListFromEnum(typeof(ProjectType));
            viewModel.ProjectCategories = Util.CreateSelectListFromEnum(typeof(ProjectCategory));
            viewModel.ConstructionTypes = _service.GetConstructionTypes().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            viewModel.BuildingTypes = _service.GetBuildingTypes().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            viewModel.Architects = _service.GetArchitects().Select(a => new SelectListItem { Text = a.CompanyName + " - " + (a.StateId.HasValue ? a.State.Abbr : "N/A"), Value = a.Id.ToString() });
            viewModel.CreatedBy = _service.GetArchitectsAndGenContractors().Select(c => new SelectListItem { Text = c.CompanyName + " - " + (c.StateId.HasValue ? c.State.Abbr : "N/A"), Value = c.Id.ToString() });

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, HandleError]
        public ActionResult Create(ProjectEditModel viewModel)
        {
            BCModel.Projects.Project toAdd = new BCModel.Projects.Project();

            return View();
        }
    }
}
