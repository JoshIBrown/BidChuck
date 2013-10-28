using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Admin.Models.Projects;
using BCWeb.Helpers;
using BCWeb.Models;
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

        public IEnumerable<ProjectListViewModel> GetMyCreated()
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

        public IEnumerable<ProjectListViewModel> GetByMyCompany()
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

        public IEnumerable<ProjectListViewModel> GetPublic()
        {
            int userId = _security.GetUserId(User.Identity.Name);

            IEnumerable<ProjectListViewModel> list = _service.GetEnumerable(x => x.ProjectType == ProjectType.Federal || x.ProjectType == ProjectType.Local || x.ProjectType == ProjectType.State)
                .Select(s => new ProjectListViewModel
                {
                    Architect = s.Architect.CompanyName,
                    Id = s.Id,
                    Title = s.Title,
                    Number = s.Number
                });

            return list;
        }

        public IEnumerable<ProjectListViewModel> GetProjectsInvitedTo()
        {
            UserProfile theUser = _service.GetUserProfile(_security.GetUserId(User.Identity.Name));
            IEnumerable<BidPackageXInvitee> invites = _service.GetInvitations(theUser.CompanyId);
            var projects = invites.Select(i => i.BidPackage.Project).Distinct();
            var viewModel = projects.Select(p => new ProjectListViewModel { Id = p.Id, Title = p.Title, Architect = p.Architect.CompanyName });


            return viewModel;
        }

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
    }
}
