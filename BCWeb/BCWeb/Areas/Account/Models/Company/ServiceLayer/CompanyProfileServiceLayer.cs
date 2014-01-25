using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCWeb.Areas.Account.Models.Company.Repository;
using BCWeb.Helpers;
using System.Data.Spatial;
using BCWeb.Models.Company;
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


        public IEnumerable<BCModel.CompanyProfile> SearchCompanyProfiles(string query, string city, string state, string postal, double? distance)
        {
            // call up the locator
            GeoLocator locator = new GeoLocator();

            DbGeography searchPoint = locator.GetFromAddress("", city, state, postal);
            // determine how narrow our search params are




            var results = from c in _repo.Query()
                          where c.CompanyName.Contains(query)
                          && c.GeoLocation.Distance(searchPoint) <= distance
                          select c;

            return results.AsEnumerable();
        }


        public ConnectionStatus GetConnectionStatus(int currentCompany, int queriedCompany)
        {
            var connection = _repo.QueryNetworkConnections()
                .Where(x => (x.LeftId == currentCompany && x.RightId == queriedCompany) || (x.RightId == currentCompany && x.LeftId == queriedCompany))
                .SingleOrDefault();

            if (connection != null)
                return ConnectionStatus.Connected;

            var sentInvite = _repo.QueryConnectionRequests()
                .Where(x => x.SenderId == currentCompany && x.RecipientId == queriedCompany && !x.AcceptDate.HasValue && !x.DeclineDate.HasValue)
                .SingleOrDefault();

            if (sentInvite != null)
                return ConnectionStatus.InvitationSent;

            var recvdInvite = _repo.QueryConnectionRequests()
                .Where(x => x.RecipientId == currentCompany && x.SenderId == queriedCompany && !x.AcceptDate.HasValue && !x.DeclineDate.HasValue)
                .SingleOrDefault();

            if (recvdInvite != null)
                return ConnectionStatus.InvitationPending;

            var blackList = _repo.QueryBlackLists()
                .Where(x => (x.BlackListedCompanyId == currentCompany && x.CompanyId == queriedCompany) || (x.CompanyId == currentCompany && x.BlackListedCompanyId == queriedCompany))
                .FirstOrDefault();

            if (blackList != null)
                return ConnectionStatus.BlackListed;

            if (currentCompany == queriedCompany)
                return ConnectionStatus.Self;

            return ConnectionStatus.NotConnected;

        }
    }
}