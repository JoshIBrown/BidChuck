using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Admin.Models.Projects.ViewModel;
using BCWeb.Helpers;
using BCWeb.Models;
using BCWeb.Models.GenericViewModel;
using BCWeb.Models.Project.ServiceLayer;
using BCWeb.Models.Project.ViewModel;
using BCWeb.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;

namespace BCWeb.Api
{
    [Authorize]
    public class ProjectController : ApiController
    {
        private IProjectServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ProjectController(IProjectServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }



        public IEnumerable<ProjectListViewModel> GetMyCreatedList()
        {
            int userId = _security.GetUserId(User.Identity.Name);

            IEnumerable<ProjectListViewModel> list = _service.GetEnumerable(s => s.CreatedById == userId)
                .Select(s => new ProjectListViewModel
                {
                    Architect = s.Architect.CompanyName,
                    Id = s.Id,
                    Title = s.Title,
                    Number = s.Number
                });

            return list;
        }

        public IEnumerable<ProjectListViewModel> GetByMyCompanyList()
        {
            int userId = _security.GetUserId(User.Identity.Name);
            UserProfile theUser = _service.GetUserProfile(userId);
            IEnumerable<ProjectListViewModel> list = _service.GetEnumerable(s => s.ArchitectId == theUser.CompanyId)
                .Select(s => new ProjectListViewModel
                {
                    Architect = s.Architect.CompanyName,
                    Id = s.Id,
                    Title = s.Title,
                    Number = s.Number
                });

            return list;
        }

        public IEnumerable<ProjectListViewModel> GetPublicList()
        {
            int userId = _security.GetUserId(User.Identity.Name);

            IEnumerable<ProjectListViewModel> list = _service.GetActivePublicSearchable()
                .Select(s => new ProjectListViewModel
                {
                    Architect = s.Architect.CompanyName,
                    Id = s.Id,
                    Title = s.Title,
                    Number = s.Number
                });

            return list;
        }

        public IEnumerable<ProjectListViewModel> GetProjectsInvitedToList()
        {
            UserProfile theUser = _service.GetUserProfile(_security.GetUserId(User.Identity.Name));
            IEnumerable<Invitation> invites = _service.GetInvitations(theUser.CompanyId);
            var projects = invites.Select(i => i.BidPackage.Project).Distinct();
            var viewModel = projects.Select(p => new ProjectListViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Architect = p.Architect.CompanyName,
                Number = p.Number
            });


            return viewModel;
        }

        [Authorize(Roles = "Administrator")]
        public DataTablesResponse GetDataTable(
            [FromUri]int iDisplayStart,
            [FromUri]int iDisplayLength,
            [FromUri]int iColumns,
            [FromUri]string sSearch,
            [FromUri]int iSortCol_0,
            [FromUri]string sSortDir_0,
            [FromUri]string sEcho)
        {
            DataTablesResponse response = new DataTablesResponse();

            Expression<Func<Project, bool>> predicate = m => true;
            if (sSearch != null && sSearch.Length > 0)
            {
                predicate = predicate.And(m => m.Architect.CompanyName.Contains(sSearch) ||
                    m.Title.Contains(sSearch) ||
                    m.Number.Contains(sSearch) ||
                    m.BuildingType.Name.Contains(sSearch) ||
                    m.ConstructionType.Name.Contains(sSearch));
            }

            Func<ProjectListItem, IComparable> orderBy;

            switch (iSortCol_0)
            {
                case 0: // id
                    orderBy = o => o.Id;
                    break;
                case 1: // number
                    orderBy = o => o.Number;
                    break;
                case 2: // title
                    orderBy = o => o.Title;
                    break;
                case 3: // bid date
                    orderBy = o => o.BidDate;
                    break;
                case 4: // state
                    orderBy = o => o.State;
                    break;
                case 5: // created by
                    orderBy = o => o.CreatedBy;
                    break;
                case 6: // architect
                    orderBy = o => o.Architect;
                    break;
                case 7: // project type
                    orderBy = o => o.ProjectType;
                    break;
                case 8: // construction type
                    orderBy = o => o.ConstructionType;
                    break;
                case 9: // building type
                    orderBy = o => o.BuildingType;
                    break;
                default:
                    orderBy = o => o.Id;
                    break;
            }

            int sEchoInt;
            if (int.TryParse(sEcho, out sEchoInt))
            {
                response.sEcho = sEcho;
            }
            else
            {
                throw new Exception("sEcho is invalid");
            }

            response.iTotalRecords = _service.GetEnumerable().Count();
            response.iTotalDisplayRecords = _service.GetEnumerable(predicate).Count();

            ProjectListItem[] data;


            switch (sSortDir_0)
            {
                case "asc":
                    data = _service.GetEnumerable(predicate)
                .Select(p => new ProjectListItem
                {
                    Id = p.Id,
                    Number = p.Number,
                    Title = p.Title,
                    BidDate = p.BidDateTime,
                    BuildingType = p.BuildingType.Name,
                    ConstructionType = p.ConstructionType.Name,
                    ProjectType = p.ProjectType.ToDescription(),
                    Architect = p.Architect.CompanyName,
                    CreatedBy = p.CreatedBy.LastName + ", " + p.CreatedBy.FirstName,
                    State = p.State.Abbr
                })
                .OrderBy(orderBy)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToArray();
                    break;
                case "desc":
                    data = _service.GetEnumerable(predicate)
                .Select(p => new ProjectListItem
                {
                    Id = p.Id,
                    Number = p.Number,
                    Title = p.Title,
                    BidDate = p.BidDateTime,
                    BuildingType = p.BuildingType.Name,
                    ConstructionType = p.ConstructionType.Name,
                    ProjectType = p.ProjectType.ToDescription(),
                    Architect = p.Architect.CompanyName,
                    CreatedBy = p.CreatedBy.LastName + ", " + p.CreatedBy.FirstName,
                    State = p.State.Abbr
                })
                .OrderByDescending(orderBy)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToArray();
                    break;
                default:
                    throw new Exception("invalid sort direction");
            }

            response.aaData = data;

            return response;
        }

        [Authorize(Roles = "general_contractor,architect"), ValidateHttpAntiForgeryToken]
        public JQueryPostResult PostCreate(BCWeb.Models.Project.ViewModel.ProjectEditModel viewModel)
        {
            JQueryPostResult result = new JQueryPostResult();
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
                    WalkThruStatus = viewModel.WalkThruStatus.Value
                };
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
                    result.success = true;
                    result.message = "project created";
                    result.data = new { id = toCreate.Id };
                }
                else
                {
                    result.success = false;
                    result.message = "project not created";
                    result.data = new { errors = _service.ValidationDic };
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = "exception caught";
                result.data = new { errors = ex.Message };
            }
            return result;
        }
    }
}
