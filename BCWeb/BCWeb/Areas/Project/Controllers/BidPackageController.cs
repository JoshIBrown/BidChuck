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

        public ActionResult Index()
        {
            return View();
        }


        // GET: /Projects/BidPackage/Create
        [Authorize(Roles = "general_contractor")]
        public ActionResult Create(int projectId)
        {
            EditBidPackageViewModel viewModel = new EditBidPackageViewModel();
            viewModel.ProjectId = projectId;
            viewModel.TemplateId = _service.GetProject(projectId).BidPackages.Where(b => b.IsMaster).FirstOrDefault().Id;
            return View("Create", viewModel);
        }

        //
        // GET: /Projects/BidPackage/Create
        [Authorize(Roles = "subcontractor,materials_vendor")]
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
                                TemplateBidPackageId = viewModel.TemplateId
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
            return View("Details");
        }
    }
}
