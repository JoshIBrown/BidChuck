using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    [Authorize]
    public class CompanyController : ApiController
    {
        private ICompanyProfileServiceLayer _serviceLayer;
        private IWebSecurityWrapper _security;

        public CompanyController(ICompanyProfileServiceLayer serviceLayer,IWebSecurityWrapper security)
        {
            _serviceLayer = serviceLayer;
            _security = security;
        }
        
        // /api/Company/GetNewest
        [AllowAnonymous]
        public IEnumerable<NewCompanyViewModel> GetNewest()
        {
            // get 10 most recently registered and published companies.  avoid delegates in results
            var companies = _serviceLayer.GetEnumerable()
                                                .OrderByDescending(x => x.Id)
                                                .Take(10)
                                                .Select(x => new NewCompanyViewModel
                                                {
                                                    Id = x.Id,
                                                    CompanyName = x.CompanyName,
                                                    BusinessType = x.BusinessType.Name,
                                                    Scopes = x.Scopes
                                                        .Where(s => !s.Scope.ParentId.HasValue).Select(s => s.Scope.CsiNumber.Substring(0, 2) + " " + s.Scope.Description)
                                                        .ToArray()
                                                })
                                                .ToArray();
            return companies;
        }

        // get newest group by scope
    }
}
