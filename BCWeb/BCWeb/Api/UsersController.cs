using BCModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Controllers.Api
{
    public class UsersController : ApiController
    {

        public IEnumerable<object> GetNewestCompanies()
        {

            using (BidChuckContext context = new BidChuckContext())
            {
                // get 10 most recently registered and published companies
                var companies = context.UserProfiles.Where(x => x.Email != "admin@bidchuck.com")
                                                    .OrderByDescending(x => x.UserId)
                                                    .Take(10)
                                                    .Select(x => new { Id = x.UserId, Company = x.CompanyName })
                                                    .ToArray();

                return companies;
            }
        }
    }
}
