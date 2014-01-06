using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.Documents.ServiceLayer;
using BCWeb.Areas.Project.Models.Documents.ViewModel;
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
    [Authorize, HandleError]
    public class DocumentController : Controller
    {

        private IProjectDocServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;

        public DocumentController(IProjectDocServiceLayer service, IWebSecurityWrapper security,INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
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
            UserProfile theUser = _service.GetUser(_security.GetUserId(User.Identity.Name));

            if (ModelState.IsValid)
            {
                ProjectDocument toCreate = new ProjectDocument
                {
                    CompanyId = theUser.CompanyId,
                    Name = viewModel.Name,
                    Notes = viewModel.Notes,
                    ProjectId = viewModel.ProjectId,
                    Url = viewModel.Url
                };

                if (_service.Create(toCreate))
                {
                    // send notifications to invited members
                    int[] invitees =  _notice.GetInvitationsNotDeclined(viewModel.ProjectId, theUser.CompanyId).Select(s=>s.SentToId).ToArray();

                    for (int i = 0; i < invitees.Length; i++)
                    {
                        _notice.SendNotification(invitees[i], RecipientType.company, NotificationType.ProjectChange, viewModel.ProjectId);
                    }

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

            UserProfile theUser = _service.GetUser(_security.GetUserId(User.Identity.Name));

            if (doc.CompanyId != theUser.CompanyId && _security.IsUserInRole("general_contractor"))
            {
                throw new HttpException(403, "this is not your document");
            }

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

        [HandleError]
        public ActionResult Edit(int id)
        {
            ProjectDocument doc = _service.Get(id);
            UserProfile theUser = _service.GetUser(_security.GetUserId(User.Identity.Name));

            if (doc.CompanyId != theUser.CompanyId)
            {
                throw new HttpException(403, "this is not your document");
            }

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
                UserProfile theUser = _service.GetUser(_security.GetUserId(User.Identity.Name));
                ProjectDocument toUpdate = _service.Get(viewModel.Id);

                if (theUser.CompanyId != toUpdate.CompanyId)
                {
                    throw new HttpException(403, "this is not your document");
                }

                toUpdate.Notes = viewModel.Notes;
                toUpdate.Name = viewModel.Name;
                toUpdate.Url = viewModel.Url;


                if (_service.Update(toUpdate))
                {
                    // send notifications to invited members
                    int[] invitees = _notice.GetInvitationsNotDeclined(viewModel.ProjectId, theUser.CompanyId).Select(s => s.SentToId).ToArray();

                    for (int i = 0; i < invitees.Length; i++)
                    {
                        _notice.SendNotification(invitees[i], RecipientType.company, NotificationType.ProjectChange, viewModel.ProjectId);
                    }

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