using BCModel;
using BCWeb.Areas.Account.Models.Scopes.ServiceLayer;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    [Authorize]
    public class UserScopesController : ApiController
    {
         private IScopeServiceLayer _service;
        private IWebSecurityWrapper _security;

        public UserScopesController(IScopeServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            UserProfile theUser = _service.GetUser(id);

            if (theUser == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "user not found");
            }

            CompanyProfile theCompany = theUser.Company;
            IEnumerable<Scope> selectedScopes = theUser.Scopes.Select(s => s.Scope).ToArray();
            IEnumerable<ScopeMgmtViewModel> list = theCompany.Scopes
                .Select(s => new ScopeMgmtViewModel
                {

                }).ToArray();

            return request.CreateResponse(HttpStatusCode.OK, list);
        }

        public HttpResponseMessage Put(HttpRequestMessage request)
        {
            return request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            return request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        public HttpResponseMessage Delete(HttpRequestMessage request)
        {
            return request.CreateResponse(HttpStatusCode.NotImplemented);
        }
    }
}
