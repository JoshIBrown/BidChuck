using BCWeb.Areas.Project.Models.Bids.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Bids.ServiceLayer
{
    public class BidServiceLayer : IBidServiceLayer
    {
        private IBidRepository _repo;

        public BidServiceLayer(IBidRepository repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }

        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _repo.GetUserProfile(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _repo.GetCompanyProfile(id);
        }

        public BCModel.Projects.Project GetProject(int id)
        {
            return _repo.GetProject(id);
        }

        public BCModel.Projects.BidPackage GetBidPackage(int id)
        {
            return _repo.GetBidPackage(id);
        }

        public BCModel.Projects.Invitation GetInvite(int bidPackageId, int companyId)
        {
            return _repo.GetInvite(bidPackageId, companyId);
        }

        public IEnumerable<BCModel.Projects.Invitation> GetInvites(int projectId, int companyId)
        {
            return (from r in _repo.QueryInvites()
                    where r.BidPackage.ProjectId == projectId
                    && r.CompanyId == companyId
                    select r).ToList();
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.Projects.Bid entity)
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

        public bool Update(BCModel.Projects.Bid entity)
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

        public bool Delete(BCModel.Projects.Bid entity)
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

        public bool Delete(int id)
        {
            return Delete(_repo.Get(id));
        }

        public IEnumerable<BCModel.Projects.Bid> GetEnumerable()
        {
            return _repo.Query().ToList();
        }

        public IEnumerable<BCModel.Projects.Bid> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.Projects.Bid, bool>> predicate)
        {
            return _repo.Query().Where(predicate).ToList();
        }

        public BCModel.Projects.Bid Get(params object[] key)
        {
            return _repo.Get(key);
        }

        public bool Exists(int id)
        {
            return _repo.Get(id) == null;
        }


        public IEnumerable<BCModel.Scope> GetBidPackageScopes(int bidPackageId)
        {
            return _repo.GetBidPackage(bidPackageId).Scopes.Select(s => s.Scope).ToList();
        }
    }
}