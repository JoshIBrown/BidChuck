﻿using BCModel;
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

                    // if user is a GC, self-invite
                    if (_security.IsUserInRole("general_contractor"))
                    {
                        projectPackage.Invitees.Add(new BidPackageXInvitee { BidPackage = projectPackage, CompanyId = companyId, Sent = DateTime.Now });
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
            Project theProject = _service.Get(id);

            UserProfile user = _service.GetUserProfile(_security.GetUserId(User.Identity.Name));

            
            
            
            // if invited sub, show bp invited to
            if (User.IsInRole("subcontractor") || User.IsInRole("materials_vendor"))
            {
                IEnumerable<BidPackageXInvitee> invites = _service.GetInvitations(theProject.Id, user.CompanyId);
                IEnumerable<ProjectBPViewModel> bps = invites.Select(b => new ProjectBPViewModel
                {
                    Id = b.BidPackageId,
                    BidDateTime = b.BidPackage.BidDateTime.Value,
                    Description = b.BidPackage.Description,
                    SelectedScope = b.BidPackage.Scopes.Select(s => s.Scope.CsiNumber + " " + s.Scope.Description)
                });

                SubsAndVendProjectDetailsViewModel sAndVViewModel = new SubsAndVendProjectDetailsViewModel
                {
                    Address = theProject.Address,
                    Architect = theProject.Architect.CompanyName,
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
                    BidPackages = bps
                };


                return View("SubAndVendDetails", sAndVViewModel);
            }


            BidPackage masterBP = theProject.BidPackages.Where(b => b.IsMaster).FirstOrDefault();

            BPProjectDetailsViewModel gcViewModel = new BPProjectDetailsViewModel
            {
                Address = theProject.Address,
                Architect = theProject.Architect.CompanyName,
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
                SelectedScope = masterBP.Scopes.Select(s => s.Scope.CsiNumber + " " + s.Scope.Description).OrderBy(s => s),
                State = theProject.State.Abbr,
                Title = theProject.Title
            };
            return View("BPDetails", gcViewModel);
            

            //// if architect, show base level
            //ProjectDetailsViewModel viewModel = new ProjectDetailsViewModel
            //{
            //    Address = theProject.Address,
            //    Architect = theProject.Architect.CompanyName,
            //    Owner = theProject.ClientId.HasValue ? theProject.Client.CompanyName : "",
            //    BidDateTime = theProject.BidDateTime,
            //    BuildingType = theProject.BuildingType.Name,
            //    City = theProject.City,
            //    ConstructionType = theProject.ConstructionType.Name,
            //    Description = theProject.Description,
            //    Id = theProject.Id,
            //    PostalCode = theProject.PostalCode,
            //    ProjectType = theProject.ProjectType.ToDescription(),
            //    SelectedScope = theProject.Scopes.Select(s => s.Scope.CsiNumber + " " + s.Scope.Description).OrderBy(s => s),
            //    State = theProject.State.Abbr,
            //    Title = theProject.Title
            //};
            //return View("Details", viewModel);
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



        [Authorize(Roles = "architect,general_contractor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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