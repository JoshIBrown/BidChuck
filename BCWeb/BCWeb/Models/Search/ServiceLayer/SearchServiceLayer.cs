using BCWeb.Models.Search.Repository;
using BCWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Spatial;

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


        public IEnumerable<BCModel.State> GetStates()
        {
            return _repo.GetStates().AsEnumerable();
        }
    }
}