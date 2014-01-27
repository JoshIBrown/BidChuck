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
    public class CompanyScopesController : ApiController
    {
        private IScopeServiceLayer _service;
        private IWebSecurityWrapper _security;

        public CompanyScopesController(IScopeServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            CompanyProfile theCompany = _service.GetCompany(id);

            if (theCompany == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "company not found");
            }

            IEnumerable<Scope> selectedScopes = theCompany.Scopes.Select(s => s.Scope).ToArray();
            IEnumerable<ScopeMgmtViewModel> list = _service.GetEnumerable()
                .Select(s => new ScopeMgmtViewModel
                {
                    Checked = selectedScopes.Contains(s),
                    Description = s.Description,
                    Id = s.Id,
                    ParentId = s.ParentId,
                    CsiNumber = s.CsiNumber
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
