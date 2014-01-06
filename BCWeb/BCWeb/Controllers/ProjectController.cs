using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.Documents.ViewModel;
using BCWeb.Helpers;
using BCWeb.Models;
using BCWeb.Models.Notifications.ServiceLayer;
using BCWeb.Models.Project.ServiceLayer;
using BCWeb.Models.Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
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
        private INotificationSender _notice;

        public ProjectController(IProjectServiceLayer service, IWebSecurityWrapper security, INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
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
                ProjectEditModel viewModel = new ProjectEditModel();
                viewModel.ArchitectId = user.CompanyId;
                viewModel.Architect = user.Company.CompanyName;
                viewModel.BidDateTime = DateTime.Now;
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
            ProjectEditModel viewModel = new ProjectEditModel();
            viewModel.ArchitectId = architectId;
            viewModel.Architect = archName;
            viewModel.Title = title;
            viewModel.Number = number;
            viewModel.BidDateTime = DateTime.Now;
            rePopViewModel(viewModel);
            return View("CreateStepTwo", viewModel);
        }

        [Authorize(Roles = "general_contractor,architect,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStepTwo(ProjectEditModel viewModel)
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
                        BidPackages = new List<BidPackage>(),
                        WalkThruDateTime = viewModel.WalkThruDateTime,
                        WalkThruStatus = viewModel.WalkThruStatus.Value,
                        HiddenFromSearch = viewModel.HiddenFromSearch,
                        InvitationOnly = viewModel.InvitationOnly
                    };

                    GeoLocator locator = new GeoLocator();

                    string state = _service.GetStates().Where(x => x.Id == viewModel.StateId).FirstOrDefault().Abbr;

                    if (viewModel.Address == null || viewModel.Address == string.Empty)
                    {
                        toCreate.GeoLocation = locator.GetFromCityStateZip(viewModel.City, state, viewModel.PostalCode);
                    }
                    else
                    {
                        toCreate.GeoLocation = locator.GetFromAddress(viewModel.Address, viewModel.City, state, viewModel.PostalCode);
                    }

                    // create master bid package
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

        private void rePopViewModel(ProjectEditModel viewModel)
        {

            viewModel.ProjectTypes = Util.CreateSelectListFromEnum(typeof(ProjectType), viewModel.ProjectType.ToString());
            viewModel.ProjectCategories = Util.CreateSelectListFromEnum(typeof(ProjectCategory), viewModel.ProjectCategory.ToString());
            viewModel.ConstructionTypes = _service.GetConstructionTypes().OrderBy(c => c.Name).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = c.Id == viewModel.ConstructionTypeId });
            viewModel.States = _service.GetStates().OrderBy(s => s.Abbr).Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString(), Selected = s.Id == viewModel.StateId });
            viewModel.BuildingTypes = _service.GetBuildingTypes();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Project theProject = _service.Get(id);

            // graceful error handling
            if (theProject == null)
            {
                return View("NotFound");
            }

            UserProfile user = _service.GetUserProfile(_security.GetUserId(User.Identity.Name));

            // if hidden from search, and user is not invited
            if (theProject.HiddenFromSearch && (User.IsInRole("subcontractor") || User.IsInRole("materials_vendor") || User.IsInRole("general_contractor")))
            {
                // check for invitations to project
                var invites = _service.GetRcvdInvitations(id, user.CompanyId);

                if (invites == null || invites.Count() == 0)
                {
                    return View("NotFound");
                }
            }


            // if invited sub, show bp invited to
            if (User.IsInRole("subcontractor") || User.IsInRole("materials_vendor"))
            {


                ProjectDetailsForSubAndVendViewModel sAndVViewModel = new ProjectDetailsForSubAndVendViewModel
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
                    ProjectCategory = theProject.ProjectCategory.ToDescription(),
                    InviteOnly = theProject.InvitationOnly,
                    HiddenFromSearch = theProject.HiddenFromSearch,
                    State = theProject.State.Abbr,
                    Title = theProject.Title
                };
                // get distinct list of scopes

                return View("SubAndVendDetails", sAndVViewModel);
            }

            // else user is not a sub or material vendor
            BidPackage masterBP = _service.GetMasterBidPackage(id);
            IEnumerable<ProjectDocListItem> docs = _service.GetDocuments(id, user.CompanyId)
                .Select(d => new ProjectDocListItem
                {
                    Id = d.Id,
                    Name = d.Name,
                    Notes = d.Notes,
                    Url = d.Url
                });

            ProjectViewModel gcViewModel = new ProjectViewModel
            {
                Address = theProject.Address,
                Architect = theProject.Architect.CompanyName,
                Number = theProject.Number,
                Owner = theProject.ClientId.HasValue ? theProject.Client.CompanyName : "",
                BidDateTime = theProject.BidDateTime.ToString("MM/dd/yyyy hh:mm tt"),
                BuildingType = theProject.BuildingType.Name,
                City = theProject.City,
                ConstructionType = theProject.ConstructionType.Name,
                Description = theProject.Description,
                ProjectId = theProject.Id,
                BidPackageId = masterBP.Id,
                PostalCode = theProject.PostalCode,
                ProjectType = theProject.ProjectType.ToDescription(),
                ProjectCategory = theProject.ProjectCategory.ToDescription(),
                State = theProject.State.Abbr,
                Title = theProject.Title,
                InviteOnly = theProject.InvitationOnly,
                HiddenFromSearch = theProject.HiddenFromSearch,
                WalkThruDate = theProject.WalkThruStatus == WalkThruStatus.WalkThruIncluded && theProject.WalkThruDateTime.HasValue ? theProject.WalkThruDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : theProject.WalkThruStatus.ToString()
            };

            if (docs != null)
            {
                gcViewModel.ProjectDocs = docs;
            }

            gcViewModel.SelectedScope = masterBP.Scopes
                .Select(s => new ProjectScopeListItem
                {
                    Id = s.ScopeId,
                    Description = s.Scope.CsiNumber + " " + s.Scope.Description,
                    parentId = s.Scope.ParentId
                }).ToList();

            if (user.Company.BusinessType == BusinessType.GeneralContractor)
            {
                var invitees = _service.GetSentInvitations(id, user.CompanyId);
                gcViewModel.Invited = invitees.Count();
                gcViewModel.AcceptedInvite = invitees.Where(i => i.AcceptedDate.HasValue).Count();
                gcViewModel.DeclinedInvite = invitees.Where(i => i.RejectedDate.HasValue).Count();
                gcViewModel.SubmittedBid = invitees.Where(i => i.AcceptedDate.HasValue && i.BidSentDate.HasValue).Count();
            }

            if (user.Company.BusinessType == BusinessType.Architect)
            {
                var invitees = masterBP.Invitees;
                gcViewModel.Invited = invitees.Count();
                gcViewModel.AcceptedInvite = invitees.Where(i => i.AcceptedDate.HasValue).Count();
                gcViewModel.DeclinedInvite = invitees.Where(i => i.RejectedDate.HasValue).Count();
                gcViewModel.SubmittedBid = invitees.Where(i => i.AcceptedDate.HasValue && i.BidSentDate.HasValue).Count();
            }

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
            ProjectEditModel viewModel = new ProjectEditModel
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
                WalkThruDateTime = raw.WalkThruDateTime,
                WalkThruStatus = raw.WalkThruStatus,
                Number = raw.Number,
                ProjectCategory = raw.ProjectCategory,
                ProjectType = raw.ProjectType,
                HiddenFromSearch = raw.HiddenFromSearch,
                InvitationOnly = raw.InvitationOnly
            };
            rePopViewModel(viewModel);
            return View("Edit", viewModel);
        }



        [Authorize(Roles = "architect,general_contractor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectEditModel viewModel)
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

                    toUpdate.Title = viewModel.Title;
                    toUpdate.Description = viewModel.Description;
                    toUpdate.Address = viewModel.Address;
                    toUpdate.BidDateTime = viewModel.BidDateTime;
                    toUpdate.WalkThruDateTime = viewModel.WalkThruDateTime;
                    toUpdate.WalkThruStatus = viewModel.WalkThruStatus.Value;
                    toUpdate.BuildingTypeId = viewModel.BuildingTypeId;
                    toUpdate.City = viewModel.City;
                    toUpdate.ConstructionTypeId = viewModel.ConstructionTypeId;
                    toUpdate.PostalCode = viewModel.PostalCode;
                    toUpdate.ProjectType = viewModel.ProjectType.Value;
                    toUpdate.ProjectCategory = viewModel.ProjectCategory.Value;
                    toUpdate.StateId = viewModel.StateId;
                    toUpdate.Number = viewModel.Number;
                    toUpdate.InvitationOnly = viewModel.InvitationOnly;
                    toUpdate.HiddenFromSearch = viewModel.HiddenFromSearch;

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

                        // send notifications to invited members
                        int[] invitees = _notice.GetInvitationsNotDeclined(toUpdate.Id, companyId).Select(s => s.SentToId).ToArray();

                        for (int i = 0; i < invitees.Length; i++)
                        {
                            _notice.SendNotification(invitees[i], RecipientType.company, NotificationType.ProjectChange, toUpdate.Id);
                        }

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
