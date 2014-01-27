using BCModel;
using BCModel.Projects;
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
    public class BidPackageScopesController : ApiController
    {
        private IScopeServiceLayer _service;
        private IWebSecurityWrapper _security;

        public BidPackageScopesController(IScopeServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public HttpResponseMessage Get(HttpRequestMessage request, int id, bool idIsTemplate)
        {
            IEnumerable<ScopeMgmtViewModel> list;
            IEnumerable<Scope> availableScope;
            IEnumerable<Scope> selectedScope;

            BidPackage theBp = _service.GetBidPackage(id);

            if (theBp == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "Bid Package not found");
            }

            if (idIsTemplate)
            {
                selectedScope = new Scope[0];

                list = theBp.Scopes.Select(s => new ScopeMgmtViewModel
                {
                    Checked = false,
                    Description = s.Scope.Description,
                    Id = s.ScopeId,
                    ParentId = s.Scope.ParentId,
                    CsiNumber = s.Scope.CsiNumber
                }).ToArray();

                return request.CreateResponse(HttpStatusCode.OK, list);
            }
            else
            {
                selectedScope = theBp.Scopes.Select(s => s.Scope).ToArray();

                list = theBp.TemplateBidPackage.Scopes.Select(s => new ScopeMgmtViewModel
                {
                    Checked = selectedScope.Contains(s.Scope),
                    Description = s.Scope.Description,
                    Id = s.ScopeId,
                    ParentId = s.Scope.ParentId,
                    CsiNumber = s.Scope.CsiNumber
                }).ToArray();

                return request.CreateResponse(HttpStatusCode.OK, list);
            }
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
