using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Admin.Models.Projects;
using BCWeb.Helpers;
using BCWeb.Models.Project.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
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

            viewModel.States = _service.GetStates().Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString() });
            viewModel.ProjectTypes = Util.CreateSelectListFromEnum(typeof(ProjectType));
            viewModel.ProjectCategories = Util.CreateSelectListFromEnum(typeof(ProjectCategory));
            viewModel.ConstructionTypes = _service.GetConstructionTypes().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            viewModel.BuildingTypes = _service.GetBuildingTypes().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            viewModel.Architects = _service.GetArchitects().Select(a => new SelectListItem { Text = a.CompanyName + " - " + (a.StateId.HasValue ? a.State.Abbr : "N/A"), Value = a.Id.ToString() });
            viewModel.CreatedBy = _service.GetArchitectsAndGenContractorUsers().OrderBy(u => u.Company.CompanyName).Select(u => new SelectListItem { Text = u.Company.CompanyName + " --- " + u.Email, Value = u.UserId.ToString() });

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, HandleError]
        public ActionResult Create(ProjectEditModel viewModel)
        {
            if (!viewModel.ProjectCategory.HasValue)
                ModelState.AddModelError("ProjectCategory", "Project Category is required");
            if (!viewModel.ProjectType.HasValue)
                ModelState.AddModelError("ProjectType", "Project Type is required");

            if (ModelState.IsValid)
            {
                BCModel.Projects.Project toCreate = new BCModel.Projects.Project
                {
                    Address = viewModel.Address,
                    ArchitectId = viewModel.ArchitectId,
                    BidDateTime = viewModel.BidDateTime,
                    BuildingTypeId = viewModel.BuildingTypeId,
                    City = viewModel.City,
                    ConstructionTypeId = viewModel.ConstructionTypeId,
                    CreatedById = viewModel.CreatedById,
                    Description = viewModel.Description,
                    Number = viewModel.Number,
                    PostalCode = viewModel.PostalCode,
                    ProjectCategory = viewModel.ProjectCategory.Value,
                    ProjectType = viewModel.ProjectType.Value,
                    StateId = viewModel.StateId,
                    Title = viewModel.Title,
                    BidPackages = new List<BidPackage>(),
                    Scopes = new List<ProjectXScope>(),
                    WalkThruDateTime = viewModel.WalkThruDateTime,
                    WalkThruStatus = viewModel.WalkThruStatus.Value
                };

                GeoLocator locator = new GeoLocator();
                State projectState = _service.GetState(viewModel.StateId);

                if (viewModel.Address == null && viewModel.City == null && viewModel.StateId != null && viewModel.PostalCode != null)
                {
                    locator.GetFromStateZip(projectState.Abbr, viewModel.PostalCode, (abc) =>
                    {
                        if (abc.statusCode == 200
                            && abc.resourceSets != null
                            && abc.resourceSets.Count == 1
                            && abc.resourceSets[0].estimatedTotal == 1)
                        {
                            var lat = abc.resourceSets[0].resources[0].point.coordinates[0];
                            var lng = abc.resourceSets[0].resources[0].point.coordinates[1];
                            toCreate.GeoLocation = DbGeography.FromText(string.Format("POINT({1} {0})", lat, lng));
                        }
                    });
                }
                else if ((viewModel.Address == null || viewModel.Address == string.Empty) && viewModel.StateId != null && viewModel.PostalCode != null)
                {
                    locator.GetFromCityStateZip(viewModel.City, projectState.Abbr, viewModel.PostalCode, (abc) =>
                    {
                        if (abc.statusCode == 200
                            && abc.resourceSets != null
                            && abc.resourceSets.Count == 1
                            && abc.resourceSets[0].estimatedTotal == 1)
                        {
                            var lat = abc.resourceSets[0].resources[0].point.coordinates[0];
                            var lng = abc.resourceSets[0].resources[0].point.coordinates[1];
                            toCreate.GeoLocation = DbGeography.FromText(string.Format("POINT({1} {0})", lat, lng));
                        }
                    });
                }
                else if ((viewModel.Address != null && viewModel.Address != string.Empty) && (viewModel.City != null && viewModel.City != string.Empty) && viewModel.StateId != null && viewModel.PostalCode != null)
                {
                    locator.GetFromAddress(viewModel.Address, viewModel.City, projectState.Abbr, viewModel.PostalCode, (abc) =>
                    {
                        if (abc.statusCode == 200
                            && abc.resourceSets != null
                            && abc.resourceSets.Count == 1
                            && abc.resourceSets[0].estimatedTotal == 1)
                        {
                            var lat = abc.resourceSets[0].resources[0].point.coordinates[0];
                            var lng = abc.resourceSets[0].resources[0].point.coordinates[1];
                            toCreate.GeoLocation = DbGeography.FromText(string.Format("POINT({1} {0})", lat, lng));
                        }
                    });
                }


                BidPackage projectPackage = new BidPackage
                {
                    IsMaster = true,
                    BidDateTime = toCreate.BidDateTime,
                    Description = "Master Bid Package",
                    CreatedById = viewModel.ArchitectId,
                    Project = toCreate,
                    Scopes = new List<BidPackageXScope>(),
                    Invitees = new List<Invitation>()
                };


                UserProfile createdBy = _service.GetUserProfile(viewModel.CreatedById);

                // if createdby is a GC, self-invite
                if (createdBy.Company.BusinessType == BusinessType.GeneralContractor)
                {
                    projectPackage.Invitees.Add(new Invitation
                    {
                        BidPackage = projectPackage,
                        SentToId = viewModel.CreatedById,
                        SentDate = DateTime.Now,
                        AcceptedDate = DateTime.Now,
                        InvitationType = InvitationType.SentFromCreatedBy
                    });
                }

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
                    return RedirectToAction("Index");
                }
                else
                {
                    Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                }

            }
            rePopVieModel(viewModel);
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var raw = _service.Get(id);
            ProjectEditModel viewModel = new ProjectEditModel { };
            rePopVieModel(viewModel);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectEditModel viewModel)
        {
            return View();
        }

        private void rePopVieModel(ProjectEditModel viewModel)
        {
            viewModel.States = _service.GetStates().Select(s => new SelectListItem { Text = s.Abbr, Value = s.Abbr.ToString(), Selected = s.Id == viewModel.StateId });
            viewModel.ProjectTypes = Util.CreateSelectListFromEnum(typeof(ProjectType), viewModel.ProjectType.ToString());
            viewModel.ProjectCategories = Util.CreateSelectListFromEnum(typeof(ProjectCategory), viewModel.ProjectCategory.ToString());
            viewModel.ConstructionTypes = _service.GetConstructionTypes().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id == viewModel.ConstructionTypeId });
            viewModel.BuildingTypes = _service.GetBuildingTypes().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString(), Selected = b.Id == viewModel.BuildingTypeId });
            viewModel.Architects = _service.GetArchitects().Select(a => new SelectListItem { Text = a.CompanyName + " - " + (a.StateId.HasValue ? a.State.Abbr : "N/A"), Value = a.Id.ToString(), Selected = a.Id == viewModel.ArchitectId });
            viewModel.CreatedBy = _service.GetArchitectsAndGenContractorUsers().OrderBy(u => u.Company.CompanyName).Select(u => new SelectListItem { Text = u.Company.CompanyName + " --- " + u.Email, Value = u.UserId.ToString(), Selected = viewModel.CreatedById == u.UserId });
        }
    }
}