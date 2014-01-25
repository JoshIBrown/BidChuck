using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    [Authorize]
    public class ConnectionStatusController : ApiController
    {
        private ICompanyProfileServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ConnectionStatusController(ICompanyProfileServiceLayer service, IWebSecurityWrapper security)
        {
            _security = security;
            _service = service;
        }

        /// <summary>
        ///   /api/ConnectionStatus/123 
        ///   checks the connection status between the authenticated users company and the company id passed in
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(int companyId)
        {
            int currentCompanyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            ConnectionStatus status;
            if (currentCompanyId == companyId)
                status = ConnectionStatus.Self;
            else
                status = _service.GetConnectionStatus(currentCompanyId, companyId);

            return Request.CreateResponse(HttpStatusCode.OK, new { content = status.ToString() });
        }
    }
}
