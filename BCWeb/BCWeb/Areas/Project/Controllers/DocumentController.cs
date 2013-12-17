using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.Documents.ServiceLayer;
using BCWeb.Areas.Project.Models.Documents.ViewModel;
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
    public class DocumentController : Controller
    {

        IProjectDocServiceLayer _service;
        IWebSecurityWrapper _security;

        public DocumentController(IProjectDocServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        //
        // GET: /Project/Document/

        public ActionResult Index(int projectId)
        {
            return View();
        }

        public ActionResult Create(int projectId)
        {
            UserProfile theUser = _service.GetUser(_security.GetUserId(User.Identity.Name));
            BCModel.Projects.Project theProject = _service.GetProject(projectId);

            ProjectDocEditModel viewModel = new ProjectDocEditModel();
            viewModel.ProjectId = projectId;
            viewModel.CompanyId = theUser.CompanyId;
            viewModel.ProjectTitle = theProject.Title;

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, HandleError]
        public ActionResult Create(ProjectDocEditModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ProjectDocument toCreate = new ProjectDocument
                {
                    CompanyId = viewModel.CompanyId,
                    Name = viewModel.Name,
                    Notes = viewModel.Notes,
                    ProjectId = viewModel.ProjectId,
                    Url = viewModel.Url
                };

                if (_service.Create(toCreate))
                {
                    return RedirectToAction("Details", new { id = toCreate.Id });
                }
                else
                {
                    Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                }
            }

            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            ProjectDocument doc = _service.Get(id);
            ProjectDocViewModel viewModel = new ProjectDocViewModel
            {
                Id = doc.Id,
                Name = doc.Name,
                Notes = doc.Notes,
                ProjectId = doc.ProjectId,
                ProjectTitle = doc.Project.Title,
                Url = doc.Url
            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            ProjectDocument doc = _service.Get(id);
            ProjectDocEditModel viewModel = new ProjectDocEditModel
            {
                CompanyId = doc.CompanyId,
                Id = doc.Id,
                Name = doc.Name,
                Notes = doc.Notes,
                ProjectId = doc.ProjectId,
                ProjectTitle = doc.Project.Title,
                Url = doc.Url
            };
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, HandleError]
        public ActionResult Edit(ProjectDocEditModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ProjectDocument toUpdate = _service.Get(viewModel.Id);

                toUpdate.Notes = viewModel.Notes;
                toUpdate.Name = viewModel.Name;
                toUpdate.Url = viewModel.Url;


                if (_service.Update(toUpdate))
                {
                    return RedirectToAction("Details", new { id = toUpdate.Id });
                }
                else
                {
                    Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                }
            }

            return View(viewModel);
        }
    }
}