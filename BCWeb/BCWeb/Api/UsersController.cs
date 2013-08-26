using BCModel;
using BCWeb.Models;
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

        public IEnumerable<object> GetNewestCompanies()
        {

            using (BidChuckContext context = new BidChuckContext())
            {
                // not sure if this will get too large
                string[] managers = Roles.GetUsersInRole("Manager");
                // get 10 most recently registered and published companies
                var companies = context.UserProfiles.Where(x => managers.Contains(x.Email))
                                                    .OrderByDescending(x => x.UserId)
                                                    .Take(10)
                                                    .Select(x => new { Id = x.UserId, Company = x.CompanyName })
                                                    .ToArray();

                return companies;
            }
        }
    }
}
