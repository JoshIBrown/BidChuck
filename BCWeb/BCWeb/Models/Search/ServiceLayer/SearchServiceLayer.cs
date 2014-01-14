using BCWeb.Models.Search.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Search.ServiceLayer
{
    public class SearchServiceLayer : ISearchServiceLayer
    {
        private ISearchRepository _repo;

        public SearchServiceLayer(ISearchRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<BCModel.Projects.Project> SearchProjects(string query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query)
        {
            var results = from c in _repo.QueryCompanyProfiles()
                          where c.CompanyName.Contains(query)
                          select c;

            return results.AsEnumerable();
        }
    }
}