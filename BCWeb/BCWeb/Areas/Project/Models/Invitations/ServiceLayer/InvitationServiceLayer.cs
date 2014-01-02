using BCWeb.Areas.Project.Models.Invitations.Repository;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Invitations.ServiceLayer
{
    public class InvitationServiceLayer : IInvitationServiceLayer
    {

        private IInvitationRepository _repo;

        public InvitationServiceLayer(IInvitationRepository repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }


        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _repo.GetUerProfile(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _repo.GetCompanyProfile(id);
        }

        public BCModel.Projects.BidPackage GetBidPackage(int id)
        {
            return _repo.GetBidPackage(id);
        }

        public BCModel.Projects.Project GetProject(int id)
        {
            return _repo.GetProject(id);
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.Projects.Invitation entity)
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
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Update(BCModel.Projects.Invitation entity)
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
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Delete(BCModel.Projects.Invitation entity)
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
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Delete(params object[] key)
        {
            return Delete(_repo.Get(key));
        }

        public IEnumerable<BCModel.Projects.Invitation> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.Invitation> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.Projects.Invitation, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public BCModel.Projects.Invitation Get(params object[] key)
        {
            return _repo.Get(key);
        }

        public bool Exists(params object[] key)
        {
            return _repo.Get(key) == null;
        }


        public bool CreateRange(IEnumerable<BCModel.Projects.Invitation> invitees)
        {
            try
            {
                foreach (var i in invitees)
                {
                    _repo.Create(i);
                }
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }


        public IEnumerable<BCModel.Projects.Invitation> GetEnumerableByBidPackage(int bpId)
        {
            return (from r in _repo.Query()
                    where r.BidPackageId == bpId
                    select r).AsEnumerable();
        }


        public List<BCModel.CompanyProfile> GetBestFitCompanies(int bpId, bool inNetworkOnly)
        {
            DbGeography projectLocation = _repo.GetBidPackage(bpId).Project.GeoLocation;

            // get deepest level scope
            var bpScopes = _repo.QueryBidPackageScopes().Where(s => s.BidPackageId == bpId && s.Scope.ParentId != null
                //&& (s.Scope.Children == null || s.Scope.Children.Count() == 0)
                ).Select(s => s.ScopeId);


            // companies that have already been invited.  used for exclusion clause.
            var companiesWithInvite = from i in _repo.Query()
                                      where i.BidPackageId == bpId
                                      select i.SentToId;

            // companies with scopes contained in bid package, and operating in the project area
            var companyScopes = (from s in _repo.QueryCompanyScopes()
                                 join c in _repo.QueryCompanies() on s.CompanyId equals c.Id
                                 // limit to scopes chosen for bid package
                                 where bpScopes.Contains(s.ScopeId)
                                     // limit to companies operating in the area of the project
                                 && (c.GeoLocation.Distance(projectLocation) * 0.00062137) <= c.OperatingDistance
                                     // exclude companies that have already been invited
                                 && !companiesWithInvite.Contains(c.Id)
                                 select c.Id);
            //// TODO:
            //// if paid member, include all companies in operating radius
            //// else only include members in social connection 
            var result = (from r in _repo.QueryCompanies()
                          where companyScopes.Contains(r.Id)
                          select r).ToList();

            return result;
        }
    }
}