using BCModel;
using BCWeb.Models;
using BCWeb.Models.Account.ServiceLayer;
using BCWeb.Models.Home.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace BCWeb.Controllers.Api
{
    public class UsersController : ApiController
    {

        public IUserProfileServiceLayer _serviceLayer;
        public UsersController(IUserProfileServiceLayer service)
        {
            _serviceLayer = service;
        }

        public IEnumerable<object> GetNewestCompanies()
        {
            // not sure if this will get too large
            string[] managers = Roles.GetUsersInRole("Manager");
            // get 10 most recently registered and published companies.  avoid delegates in results
            var companies = _serviceLayer.GetProfiles(x => !x.ManagerId.HasValue && managers.Contains(x.Email))
                                                .OrderByDescending(x => x.UserId)
                                                .Take(10)
                                                .Select(x => new NewCompanyViewModel
                                                    {
                                                        Id = x.UserId,
                                                        CompanyName = x.CompanyName,
                                                        BusinessType = x.BusinessTypeId.HasValue ? x.BusinessType.Name : "",
                                                        Scopes = x.Scopes
                                                            .Where(s => !s.ParentId.HasValue).Select(s => s.CsiNumber.Substring(0, 2) + " " + s.Description)
                                                            .ToArray()
                                                    })
                                                .ToArray();



            return companies;

        }
    }
}
