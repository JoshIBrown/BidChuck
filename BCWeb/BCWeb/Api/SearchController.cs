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

        public IEnumerable<SearchResultItem> GetCompanies([FromUri]string query, [FromUri]string city, [FromUri] string state, [FromUri] string postal, [FromUri]int distance, [FromUri] int[] scopeId)
        {
            SearchResultItem[] result = new SearchResultItem[0];

            if ((city == null || city == "") && (postal == null || postal == "") && (scopeId == null || scopeId.Length == 0))
            {
                result = _service.SearchCompanyProfiles(query)
                     .Select(s => new SearchResultItem
                     {
                         Text = s.CompanyName,
                         LinkPath = Url.Link("Default", new { controller = "Company", action = "Profile", id = s.Id })
                     })
                     .ToArray();
            }
            else if (scopeId == null || scopeId.Length == 0)
            {

                result = _service.SearchCompanyProfiles(query, city, state, postal, distance)
                     .Select(s => new SearchResultItem
                     {
                         Text = s.CompanyName,
                         LinkPath = Url.Link("Default", new { controller = "Company", action = "Profile", id = s.Id })
                     })
                     .ToArray();
            }
            return result;
        }
    }
}
