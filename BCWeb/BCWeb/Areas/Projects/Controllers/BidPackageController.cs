using BCModel.Projects;
using BCWeb.Areas.Projects.Models.BidPackage.ServiceLayer;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Areas.Projects.Controllers
{
    public class BidPackageController : ApiController
    {
        private IBidPackageServiceLayer _service;
        private IWebSecurityWrapper _security;
        public BidPackageController(IBidPackageServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public IEnumerable<BidPackage> GetMyCompanyBidPackages()
        {
            var user = _service.GetUser(_security.GetUserId(User.Identity.Name));
            //var company = _service.GetCompany(user.CompanyId);
            var packages = _service.GetByCompany(user.CompanyId);

            return packages;
        }
    }
}
