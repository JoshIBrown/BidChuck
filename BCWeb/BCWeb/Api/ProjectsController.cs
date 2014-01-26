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

    public enum ProjectListType
    {
        MyCreated, Open, InvitedTo, IamArchitect
    }

    [Authorize]
    public class ProjectsController : ApiController
    {
        private IProjectServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ProjectsController(IProjectServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }





        public HttpResponseMessage Get(HttpRequestMessage request, ProjectListType type)
        {
            UserProfile theUser = _service.GetUserProfile(_security.GetUserId(User.Identity.Name));
            IEnumerable<ProjectListViewModel> list;

            switch (type)
            {
                case ProjectListType.IamArchitect:
                    list = _service.GetEnumerable(s => s.ArchitectId == theUser.CompanyId)
                         .Select(s => new ProjectListViewModel
                         {
                             Architect = s.Architect.CompanyName,
                             Id = s.Id,
                             Title = s.Title,
                             Number = s.Number
                         });
                    return request.CreateResponse(HttpStatusCode.OK, list);


                case ProjectListType.MyCreated:
                    list = _service.GetEnumerable(s => s.CreatedById == theUser.UserId)
                         .Select(s => new ProjectListViewModel
                         {
                             Architect = s.Architect.CompanyName,
                             Id = s.Id,
                             Title = s.Title,
                             Number = s.Number
                         });
                    return request.CreateResponse(HttpStatusCode.OK, list);


                case ProjectListType.InvitedTo:
                    // get invitations to project
                    IEnumerable<Invitation> invites = _service.GetInvitations(theUser.CompanyId);
                    // get the projects
                    var projects = invites.Select(i => i.BidPackage.Project).Distinct();
                    // convert to view model
                    list = projects.Select(p => new ProjectListViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Architect = p.Architect.CompanyName,
                        Number = p.Number
                    });
                    return request.CreateResponse(HttpStatusCode.OK, list);


                case ProjectListType.Open:
                    list = _service.GetActivePublicSearchable()
                        .Select(s => new ProjectListViewModel
                        {
                            Architect = s.Architect.CompanyName,
                            Id = s.Id,
                            Title = s.Title,
                            Number = s.Number
                        });
                    return request.CreateResponse(HttpStatusCode.OK, list);


                default:
                    return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [Authorize(Roles = "Administrator")]
        public DataTablesResponse Get(
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
        public HttpResponseMessage Post(HttpRequestMessage request, BCWeb.Models.Project.ViewModel.ProjectEditModel viewModel)
        {
            return request.CreateResponse(HttpStatusCode.NotImplemented);
        }
    }
}
