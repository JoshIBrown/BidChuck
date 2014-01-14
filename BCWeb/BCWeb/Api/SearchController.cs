using BCWeb.Models;
using BCWeb.Models.Search.ServiceLayer;
using BCWeb.Models.Search.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    [Authorize]
    public class SearchController : ApiController
    {
        private ISearchServiceLayer _service;
        private IWebSecurityWrapper _security;

        public SearchController(ISearchServiceLayer service, IWebSecurityWrapper security)
        {
            _security = security;
            _service = service;
        }

        public IEnumerable<SearchResultItem> GetCompanies([FromUri]string query)
        {
            SearchResultItem[] result = _service.SearchCompanyProfiles(query)
                .Select(s => new SearchResultItem
                {
                    Text = s.CompanyName,
                    LinkPath = Url.Link("Default", new { controller = "Company", action = "Profile", id = s.Id })
                })
                .ToArray();

            return result;
        }
    }
}
