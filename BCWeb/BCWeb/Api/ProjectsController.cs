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
    public class ProjectsController : ApiController
    {
        private IProjectServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ProjectsController(IProjectServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
        }

        public IEnumerable<ProjectListViewModel> GetMyCreated()
        {
            int userId = _security.GetUserId(User.Identity.Name);

            IEnumerable<ProjectListViewModel> list = _service.GetEnumerable(s => s.CreatorId == userId)
                .Select(s => new ProjectListViewModel
                {
                    Architect = s.Architect,
                    Id = s.Id,
                    Title = s.Title
                });

            return list;
        }
    }
}
