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
        private IGenericServiceLayer<UserProfile> _serviceLayer;

        public UsersController(IGenericServiceLayer<UserProfile> serviceLayer)
        {
            _serviceLayer = serviceLayer;
        }

        public IEnumerable<object> GetNewestCompanies()
        {
            // get 10 most recently registered and published companies
            var companies = _serviceLayer.GetEnumerable(x => x.Published).OrderByDescending(x => x.UserId).Take(10).ToArray();

            return companies;
        }
    }
}
