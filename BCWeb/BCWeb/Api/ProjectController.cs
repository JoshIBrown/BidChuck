using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using BCWeb.Models.Project.ServiceLayer;
using BCWeb.Models.Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    Title = s.Title
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
                    Title = s.Title
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
                    Title = s.Title
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
    }
}
