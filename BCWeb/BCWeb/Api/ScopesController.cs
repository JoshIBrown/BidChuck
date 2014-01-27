using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Account.Models.Scopes.ServiceLayer;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using BCWeb.Areas.Project.Models.BidPackage.ViewModel;
using BCWeb.Models;
using BCWeb.Models.GenericViewModel;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;
using WebMatrix.WebData;

namespace BCWeb.Controllers.Api
{
    [Authorize]
    public class ScopesController : ApiController
    {

        private IScopeServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ScopesController(IScopeServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var viewModel = _service.GetEnumerable().Select(s => new ScopeMgmtViewModel
            {
                Checked = false,
                Description = s.Description,
                Id = s.Id,
                ParentId = s.ParentId,
                CsiNumber = s.CsiNumber
            }).ToArray();

            return request.CreateResponse(HttpStatusCode.OK, viewModel);
        }
    }
}
