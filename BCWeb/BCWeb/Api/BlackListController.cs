using BCModel.SocialNetwork;
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
            
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            // if connected to company, delete connection
            ContactConnection conn = _service.GetNetworkConnection(companyId, companyToBlackList);

            if (conn != null)
            {
                if (!_service.RemoveNetworkConnection(conn))
                {
                    return request.CreateResponse(HttpStatusCode.InternalServerError, "cannot remove connection");
                }
            }

            // if already blacklisted
            var existingBlackList = _service.GetBlackListItem(companyId, companyToBlackList);

            if (existingBlackList != null)
            {
                return request.CreateResponse(HttpStatusCode.Conflict, "already blacklisted");
            }

            BlackList blackList = new BlackList { CompanyId = companyId, BlackListedCompanyId = companyToBlackList, BlackListDate = DateTime.Now, Notes = note };

            if (_service.BlackListCompany(blackList))
            {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, _service.ValidationDic);
            }
        }

        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Delete(HttpRequestMessage request, int companyToBlackList)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            BlackList blackList = _service.GetBlackListItem(companyId, companyToBlackList);

            // if not blacklisted
            if (blackList == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (_service.UnblackListCompany(blackList))
            {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, _service.ValidationDic);
            }
        }
    }
}
