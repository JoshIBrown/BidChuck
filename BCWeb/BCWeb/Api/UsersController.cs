using BCModel;
using BCWeb.Areas.Account.Models.Users.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.GenericViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace BCWeb.Controllers.Api
{
    [Authorize]
    public class UsersController : ApiController
    {

        public IUserProfileServiceLayer _serviceLayer;
        public UsersController(IUserProfileServiceLayer service)
        {
            _serviceLayer = service;
        }


        [AllowAnonymous]
        public IEnumerable<UserProfile> GetUsers(string query)
        {

            // company name
            // email

            // TODO: search by zip and other stuff
            return _serviceLayer.GetEnumerable(x => x.Email.Contains(query) || x.Company.CompanyName.Contains(query)).ToList();
        }
    }
}
