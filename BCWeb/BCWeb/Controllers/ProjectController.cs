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
    public class ProjectController : Controller
    {
        //
        // GET: /Projects/
        private IProjectServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ProjectController(IProjectServiceLayer service, IWebSecurityWrapper security)
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
            // check user role
            // if architect, go right to step two
            // if GC, go through dupe check process

            if (User.IsInRole("architect"))
            {
                var user = _service.GetUserProfile(_security.GetUserId(User.Identity.Name));
                EditProjectViewModel viewModel = new EditProjectViewModel();
                viewModel.ArchitectId = user.CompanyId;
                rePopViewModel(viewModel);
                return View("CreateStepTwo", viewModel);
            }
            else
            {
                return View("Create");
            }
        }

        [Authorize(Roles = "general_contractor,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DupeCheckViewModel viewModel)
        {
            // check for duplicate project
            if (viewModel.ArchitectId.HasValue)
            {
                List<Project> dupes = _service.FindDuplicate(viewModel.Title, viewModel.Number, viewModel.ArchitectId.Value).ToList();
                if (dupes.Count() > 0)
                {
                    return RedirectToRoute("Default", new { controller = "Project", action = "Duplicates", architectId = viewModel.ArchitectId, title = viewModel.Title, number = viewModel.Number });
                }
                else
                {
                    return RedirectToRoute("Default", new { controller = "Project", action = "CreateStepTwo", architectId = viewModel.ArchitectId, title = viewModel.Title, number = viewModel.Number });
                }


            }
            else // architect is not in the system.  let's make a record of them, and send them an invite.
            {
                return RedirectToRoute("Account_default", new { controller = "Company", action = "CreateArchitect", name = viewModel.Architect, title = viewModel.Title, number = viewModel.Number });
            }
        }

        [Authorize(Roles = "general_contractor,architect,Administrator")]
        [HttpGet]
        public ActionResult Duplicates(int architectId, string title, string number)
        {
            // build list of duplicates
            var dupes = _service.FindDuplicate(title, number, architectId)
                .Select(d => new ProjectListViewModel { Architect = d.Architect.CompanyName, Id = d.Id, Number = d.Number, Title = d.Title });
            DuplicatesViewModel viewModel = new DuplicatesViewModel();
            viewModel.Projects = dupes;
            viewModel.ArchitectId = architectId;
            viewModel.Number = number;
            viewModel.Title = title;

            return View(viewModel);
        }

        [Authorize(Roles = "general_contractor,architect,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duplicates(DuplicatesViewModel viewModel)
        {
            if (viewModel.btn != "")
            {
                // what button did the user click
                switch (viewModel.btn)
                {
                    case "Create New Project":
                        return RedirectToRoute("Default", new { controller = "Project", action = "CreateStepTwo", architectId = viewModel.ArchitectId, title = viewModel.Title, number = viewModel.Number });
                    case "Go Back":
                        return RedirectToRoute("Default", new { controller = "Project", action = "Create" });
                    default: // should never reach this.  need to fail more elegantly
                        var dupes = _service.FindDuplicate(viewModel.Title, viewModel.Number, viewModel.ArchitectId)
                            .Select(d => new ProjectListViewModel { Architect = d.Architect.CompanyName, Id = d.Id, Number = d.Number, Title = d.Title });
                        viewModel.Projects = dupes;
                        return View(viewModel);
                };
            }
            else // should never reach this
            {
                var dupes = _service.FindDuplicate(viewModel.Title, viewModel.Number, viewModel.ArchitectId)
                    .Select(d => new ProjectListViewModel { Architect = d.Architect.CompanyName, Id = d.Id, Number = d.Number, Title = d.Title });
                viewModel.Projects = dupes;
                return View(viewModel);
            }

        }

        [Authorize(Roles = "general_contractor,architect,Administrator")]
        [HttpGet]
        public ActionResult CreateStepTwo(int architectId, string title, string number)
        {
            string archName = _service.GetCompanyProfile(architectId).CompanyName;
            EditProjectViewModel viewModel = new EditProjectViewModel();
            viewModel.ArchitectId = architectId;
            viewModel.Architect = archName;
            viewModel.Title = title;
            viewModel.Number = number;
            rePopViewModel(viewModel);
            return View("CreateStepTwo", viewModel);
        }

        [Authorize(Roles = "general_contractor,architect,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStepTwo(EditProjectViewModel viewModel)
        {
            if (!viewModel.ProjectCategory.HasValue)
                ModelState.AddModelError("ProjectCategory", "Project Category is required");
            if (!viewModel.ProjectType.HasValue)
                ModelState.AddModelError("ProjectType", "Project Type is required");

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = _security.GetUserId(User.Identity.Name);
                    int companyId = _service.GetUserProfile(userId).CompanyId;

                    // create project
                    BCModel.Projects.Project toCreate = new BCModel.Projects.Project
                    {
                        Number = viewModel.Number,
                        ArchitectId = viewModel.ArchitectId,
                        Address = viewModel.Address,
                        BidDateTime = viewModel.BidDateTime,
                        BuildingTypeId = viewModel.BuildingTypeId,
                        City = viewModel.City,
                        ConstructionTypeId = viewModel.ConstructionTypeId,
                        CreatedById = userId,
                        Description = viewModel.Description,
                        PostalCode = viewModel.PostalCode,
                        ProjectType = viewModel.ProjectType.Value,
                        ProjectCategory = viewModel.ProjectCategory.Value,
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
                        Description = "Master Bid Package",
                        CreatedById = companyId,
                        Project = toCreate,
                        Scopes = new List<BidPackageXScope>(),
                        Invitees = new List<Invitation>()
                    };

                    // if user is a GC, self-invite
                    if (_security.IsUserInRole("general_contractor"))
                    {
                        projectPackage.Invitees.Add(new Invitation
                        {
                            BidPackage = projectPackage,
                            SentToId = companyId,
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
                        return RedirectToRoute("Default", new { controller = "Project", action = "Details", id = toCreate.Id });
                    }
                    else
                    {
                        Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                        rePopViewModel(viewModel);
                        return View("CreateStepTwo", viewModel);
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("Exception", ex.Message);
                    rePopViewModel(viewModel);
                    return View("CreateStepTwo", viewModel);
                }
            }

            // modelstate is not valid
            rePopViewModel(viewModel);
            return View("CreateStepTwo", viewModel);
        }

        private void rePopViewModel(EditProjectViewModel viewModel)
        {

            viewModel.ProjectTypes = Util.CreateSelectListFromEnum(typeof(ProjectType));
            viewModel.ProjectCategories = Util.CreateSelectListFromEnum(typeof(ProjectCategory));
            viewModel.ConstructionTypes = _service.GetConstructionTypes().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id == viewModel.ConstructionTypeId });
            //viewModel.ProjectTypes = 
            viewModel.States = _service.GetStates().OrderBy(s => s.Abbr).Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString(), Selected = s.Id == viewModel.StateId });
            viewModel.BuildingTypes = _service.GetBuildingTypes();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Project theProject = _service.Get(id);

            UserProfile user = _service.GetUserProfile(_security.GetUserId(User.Identity.Name));


            // FIXME:should redo logic to check if user's compnay is invited

            // if invited sub, show bp invited to
            if (User.IsInRole("subcontractor") || User.IsInRole("materials_vendor"))
            {
                IEnumerable<Invitation> invites = _service.GetInvitations(theProject.Id, user.CompanyId);


                Dictionary<int, string> bidDates = invites.ToDictionary(i => i.BidPackage.CreatedById, i => i.BidPackage.BidDateTime.ToShortDateString());
                Dictionary<int, IEnumerable<int>> scopeselection = _service.GetInvitationScopesByInvitingCompany(theProject.Id, user.CompanyId);
                Dictionary<int, string> inviters = _service.GetInvitatingCompanies(theProject.Id, user.CompanyId);
                Dictionary<int, string> scopes = _service.GetInvitationScopes(theProject.Id, user.CompanyId);
                Dictionary<int, bool?> inviteResponses = invites.ToDictionary(i => i.BidPackage.CreatedById, i => i.AcceptedDate.HasValue ? true : i.RejectedDate.HasValue ? false : default(bool?));

                SubsAndVendProjectDetailsViewModel sAndVViewModel = new SubsAndVendProjectDetailsViewModel
                {
                    Address = theProject.Address,
                    Architect = theProject.Architect.CompanyName,
                    Number = theProject.Number,
                    Owner = theProject.ClientId.HasValue ? theProject.Client.CompanyName : "",
                    BuildingType = theProject.BuildingType.Name,
                    City = theProject.City,
                    ConstructionType = theProject.ConstructionType.Name,
                    Description = theProject.Description,
                    ProjectId = theProject.Id,
                    PostalCode = theProject.PostalCode,
                    ProjectType = theProject.ProjectType.ToDescription(),
                    State = theProject.State.Abbr,
                    Title = theProject.Title,
                    Inviters = inviters,
                    Scopes = scopes,
                    ScopeSelection = scopeselection,
                    BidDate = bidDates
                };
                // get distinct list of scopes

                return View("SubAndVendDetails", sAndVViewModel);
            }

            // else user is not a sub or material vendor
            BidPackage masterBP = _service.GetMasterBidPackage(id);

            BPProjectDetailsViewModel gcViewModel = new BPProjectDetailsViewModel
            {
                Address = theProject.Address,
                Architect = theProject.Architect.CompanyName,
                Number = theProject.Number,
                Owner = theProject.ClientId.HasValue ? theProject.Client.CompanyName : "",
                BidDateTime = theProject.BidDateTime,
                BuildingType = theProject.BuildingType.Name,
                City = theProject.City,
                ConstructionType = theProject.ConstructionType.Name,
                Description = theProject.Description,
                ProjectId = theProject.Id,
                BidPackageId = masterBP.Id,
                PostalCode = theProject.PostalCode,
                ProjectType = theProject.ProjectType.ToDescription(),
                State = theProject.State.Abbr,
                Title = theProject.Title
            };

            gcViewModel.SelectedScope = masterBP.Scopes
                .Select(s => new ProjectScopeListItem
                {
                    Id = s.ScopeId,
                    Description = s.Scope.CsiNumber + " " + s.Scope.Description,
                    parentId = s.Scope.ParentId
                }).ToList();

            // invite is only relevant if user's comapny is a GC
            if (user.Company.BusinessType == BusinessType.GeneralContractor)
            {
                Invitation invite = masterBP.Invitees.Where(i => i.SentToId == user.CompanyId).FirstOrDefault();

                if (invite != null)
                {
                    gcViewModel.InviteType = invite.InvitationType;
                    gcViewModel.Accepted = invite.AcceptedDate.HasValue ? true
                        : invite.RejectedDate.HasValue ? false
                        : default(bool?);
                    gcViewModel.ResponseDate = invite.AcceptedDate.HasValue ? invite.AcceptedDate.Value
                        : invite.RejectedDate.HasValue ? invite.RejectedDate.Value
                        : default(DateTime?);
                }
            }

            return View(gcViewModel);

        }

        // GET: /Projects/Edit/3
        public ActionResult Edit(int id)
        {
            var raw = _service.Get(id);
            EditProjectViewModel viewModel = new EditProjectViewModel
            {
                Architect = raw.Architect.CompanyName,
                ArchitectId = raw.ArchitectId,
                Address = raw.Address,
                BidDateTime = raw.BidDateTime,
                BuildingTypeId = raw.BuildingTypeId,
                City = raw.City,
                ConstructionTypeId = raw.ConstructionTypeId,
                Description = raw.Description,
                Id = raw.Id,
                PostalCode = raw.PostalCode,
                StateId = raw.StateId,
                Title = raw.Title,
                Number = raw.Number
            };
            viewModel.SelectedScope = raw.Scopes.Select(x => x.ScopeId).ToList();
            viewModel.States = _service.GetStates().OrderBy(s => s.Abbr).Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString(), Selected = s.Id == viewModel.StateId });
            viewModel.BuildingTypes = _service.GetBuildingTypes();
            viewModel.ConstructionTypes = _service.GetConstructionTypes().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id == viewModel.ConstructionTypeId });
            return View("Edit", viewModel);
        }



        [Authorize(Roles = "architect,general_contractor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProjectViewModel viewModel)
        {
            if (!viewModel.ProjectCategory.HasValue)
                ModelState.AddModelError("ProjectCategory", "Project Category is required");
            if (!viewModel.ProjectType.HasValue)
                ModelState.AddModelError("ProjectType", "Project Type is required");

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

                    if (toUpdate.ProjectType != viewModel.ProjectType.Value)
                        toUpdate.ProjectType = viewModel.ProjectType.Value;

                    if (toUpdate.ProjectCategory != viewModel.ProjectCategory.Value)
                        toUpdate.ProjectCategory = viewModel.ProjectCategory.Value;

                    if (toUpdate.StateId != viewModel.StateId)
                        toUpdate.StateId = viewModel.StateId;

                    if (toUpdate.Number != viewModel.Number)
                        toUpdate.Number = viewModel.Number;

                    // update primary bid package
                    var ProjectPackage = toUpdate.BidPackages.Where(b => b.IsMaster).FirstOrDefault();

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
                    if (ProjectPackage.Scopes == null || ProjectPackage.Scopes.Count() == 0)
                    {
                        var temp = new List<BidPackageXScope>();

                        for (int i = 0; i < prjScopes.Count(); i++)
                        {
                            temp.Add(new BidPackageXScope { BidPackage = ProjectPackage, BidPackageId = ProjectPackage.Id, ScopeId = prjScopes[i].ScopeId });
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
                        toUpdate.Scopes.Add(new ProjectXScope { Project = toUpdate, ProjectId = toUpdate.Id, ScopeId = scopesToAdd[i] });
                        ProjectPackage.Scopes.Add(new BidPackageXScope { BidPackageId = ProjectPackage.Id, ScopeId = scopesToAdd[i] });
                    }

                    // remove scopes from project
                    for (int i = 0; i < prjScopesToRemove.Count(); i++)
                    {
                        toUpdate.Scopes.Remove(prjScopesToRemove[i]);
                    }

                    // remove scope from master bid package
                    for (int i = 0; i < bpScopesToRemove.Count(); i++)
                    {
                        ProjectPackage.Scopes.Remove(bpScopesToRemove[i]);
                    }

                    // add project to system
                    if (_service.Update(toUpdate))
                    {
                        return RedirectToRoute("Default", new { controller = "Project", action = "Details", id = toUpdate.Id });
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
                rePopViewModel(viewModel);
                return View("Edit", viewModel);
            }
        }
    }
}
