using BCWeb.Models;
using BCWeb.Models.Contacts.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;

namespace BCWeb.Api
{
    [Authorize]
    public class BlackListController : ApiController
    {
        private IContactServiceLayer _service;
        private IWebSecurityWrapper _security;

        public BlackListController(IContactServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public HttpResponseMessage Get(HttpRequestMessage request, int blackListedCompany)
        {
            return request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Put(HttpRequestMessage request, int companyToBlackList, string note)
        {
            return request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Post(HttpRequestMessage request, int companyToBlackList, string note)
        {
            // if connected to company
            return request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Delete(HttpRequestMessage request, int companyToBlackList)
        {
            return request.CreateResponse(HttpStatusCode.NotImplemented);
        }
    }
}
