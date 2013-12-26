using BCWeb.Areas.Project.Models.Invitations.Repository;
using System;
using System.Collections.Generic;
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


        public IEnumerable<BCModel.CompanyProfile> GetBestFitCompanies(int bpId, bool inNetworkOnly)
        {
            BCModel.Projects.BidPackage theBidPackage = _repo.GetBidPackage(bpId);

            // get companies who operate in the area of the project
            var foo = from c in _repo.QueryCompanies()
                      where c.GeoLocation.Distance(theBidPackage.Project.GeoLocation) <= c.OperatingDistance
                      select c;


                         
            throw new NotImplementedException();
        }
    }
}