using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCWeb.Areas.Account.Models.Company.Repository;
using BCWeb.Helpers;
using System.Data.Spatial;
using BCWeb.Models.Company;
using BCModel.Projects;
namespace BCWeb.Areas.Account.Models.Company.ServiceLayer
{
    public class CompanyProfileServiceLayer : ICompanyProfileServiceLayer
    {
        private ICompanyProfileRepository _repo;

        public CompanyProfileServiceLayer(ICompanyProfileRepository repo)
        {
            _repo = repo;
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.CompanyProfile entity)
        {
            try
            {
                _repo.Create(entity);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }

        public bool Update(BCModel.CompanyProfile entity)
        {
            try
            {
                _repo.Update(entity);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }

        public bool Delete(BCModel.CompanyProfile entity)
        {
            try
            {
                _repo.Delete(entity);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }

        public bool Delete(params object[] key)
        {
            try
            {
                _repo.Delete(key);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }

        public IEnumerable<BCModel.CompanyProfile> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.CompanyProfile, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public BCModel.CompanyProfile Get(params object[] key)
        {
            return _repo.Get(key);
        }

        public bool Exists(params object[] key)
        {
            return _repo.Get(key) == null;
        }

        public IEnumerable<BCModel.State> GetStates()
        {
            return _repo.QueryStates().AsEnumerable();
        }


        //public IEnumerable<BCModel.BusinessType> GetBusinessTypes()
        //{
        //    return _repo.QueryBusinessTypes().AsEnumerable();
        //}


        public IEnumerable<BCModel.UserProfile> GetUserProfiles()
        {
            return _repo.QueryUserProfiles().AsEnumerable();
        }

        public IEnumerable<BCModel.UserProfile> GetUserProfiles(System.Linq.Expressions.Expression<Func<BCModel.UserProfile, bool>> predicate)
        {
            return _repo.QueryUserProfiles().Where(predicate).AsEnumerable();
        }


        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _repo.GetUserProfile(id);
        }


        public IEnumerable<BCModel.CompanyProfile> GetEmptyLatLongList()
        {
            return (from r in _repo.Query()
                    where r.GeoLocation == null || r.GeoLocation == default(DbGeography)
                    select r).AsEnumerable();
        }


        public BCModel.State GetState(int id)
        {
            return _repo.QueryStates().Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query)
        {
            var results = from c in _repo.Query()
                          where c.CompanyName.Contains(query)
                          || c.City.Contains(query)
                          || c.PostalCode.Contains(query)
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BCModel.BusinessType[] types)
        {
            var results = from c in _repo.Query()
                          where types.Contains(c.BusinessType)
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BCModel.BusinessType[] types)
        {
            var results = from c in _repo.Query()
                          where types.Contains(c.BusinessType) &&
                          (c.CompanyName.Contains(query)
                          || c.City.Contains(query)
                          || c.PostalCode.Contains(query))
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, string city, string state, string postal, double distance)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);

            var results = from c in _repo.Query()
                          where c.CompanyName.Contains(query)
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= distance
                          select c;

            return results.AsEnumerable();
        }


        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BCModel.BusinessType[] types, string city, string state, string postal, double distance, int[] scopes)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);

            var results = from c in _repo.Query()
                          where c.CompanyName.Contains(query)
                          && c.Scopes.Any(s => scopes.Contains(s.ScopeId))
                          && types.Contains(c.BusinessType)
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= distance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, string city, string state, string postal, double distance, int[] scopes)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);

            var results = from c in _repo.Query()
                          where c.CompanyName.Contains(query)
                          && c.Scopes.Any(s => scopes.Contains(s.ScopeId))
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= distance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BCModel.BusinessType[] types, string city, string state, string postal, double distance, int[] scopes)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);

            var results = from c in _repo.Query()
                          where types.Contains(c.BusinessType)
                          && c.Scopes.Any(s => scopes.Contains(s.ScopeId))
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= distance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string city, string state, string postal, double distance, int[] scopes)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);

            var results = from c in _repo.Query()
                          where c.Scopes.Any(s => scopes.Contains(s.ScopeId))
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= distance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BCModel.BusinessType[] types, string city, string state, string postal, double distance)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);

            var results = from c in _repo.Query()
                          where c.CompanyName.Contains(query)
                          && types.Contains(c.BusinessType)
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= distance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BCModel.BusinessType[] types, string city, string state, string postal, double distance)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);

            var results = from c in _repo.Query()
                          where types.Contains(c.BusinessType)
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= distance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string city, string state, string postal, double distance)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);

            var results = from c in _repo.Query()
                          where (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= distance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BCModel.BusinessType[] types, int projectIdforLocation, int bidPackageIdforScopes)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = _repo.FindProject(projectIdforLocation).GeoLocation;

            int[] bpScopes = _repo.FindBidPackage(bidPackageIdforScopes).Scopes.Select(s => s.ScopeId).ToArray();

            var results = from c in _repo.Query()
                          where c.CompanyName.Contains(query)
                          && c.Scopes.Any(s => bpScopes.Contains(s.ScopeId))
                          && types.Contains(c.BusinessType)
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= c.OperatingDistance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, BCModel.BusinessType[] types, int projectIdforLocation)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = _repo.FindProject(projectIdforLocation).GeoLocation;

            var results = from c in _repo.Query()
                          where c.CompanyName.Contains(query)
                          && types.Contains(c.BusinessType)
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= c.OperatingDistance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(int projectIdforLocation, int bidPackageIdforScopes)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = _repo.FindProject(projectIdforLocation).GeoLocation;

            int[] bpScopes = _repo.FindBidPackage(bidPackageIdforScopes).Scopes.Select(s => s.ScopeId).ToArray();

            var results = from c in _repo.Query()
                          where c.Scopes.Any(s => bpScopes.Contains(s.ScopeId))
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= c.OperatingDistance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(int projectIdforLocation)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = _repo.FindProject(projectIdforLocation).GeoLocation;

            var results = from c in _repo.Query()
                          where (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= c.OperatingDistance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BCModel.BusinessType[] types, int projectIdforLocation, int bidPackageIdforScopes)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = _repo.FindProject(projectIdforLocation).GeoLocation;

            int[] bpScopes = _repo.FindBidPackage(bidPackageIdforScopes).Scopes.Select(s => s.ScopeId).ToArray();

            var results = from c in _repo.Query()
                          where c.Scopes.Any(s => bpScopes.Contains(s.ScopeId))
                          && types.Contains(c.BusinessType)
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= c.OperatingDistance
                          select c;

            return results.AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(BCModel.BusinessType[] types, int projectIdforLocation)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            // get a search point
            DbGeography searchPoint = _repo.FindProject(projectIdforLocation).GeoLocation;

            var results = from c in _repo.Query()
                          where types.Contains(c.BusinessType)
                          && (c.GeoLocation.Distance(searchPoint).Value * 0.00062137) <= c.OperatingDistance
                          select c;

            return results.AsEnumerable();
        }
    }
}