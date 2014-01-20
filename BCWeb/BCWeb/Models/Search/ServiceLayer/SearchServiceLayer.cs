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

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query)
        {
            var results = from c in _repo.QueryCompanyProfiles()
                          where c.CompanyName.Contains(query)
                          || c.City.Contains(query)
                          || c.PostalCode.Contains(query)
                          select c;

            return results.AsEnumerable();
        }


        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, string city, string state, string postal, double? distance)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);
            // determine how narrow our search params are




            var results = from c in _repo.QueryCompanyProfiles()
                          where c.CompanyName.Contains(query)
                          && c.GeoLocation.Distance(searchPoint) <= distance
                          select c;

            return results.AsEnumerable();
        }
    }
}