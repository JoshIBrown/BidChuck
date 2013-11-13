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
                    && r.SentToId == companyId
                    select r).ToList();
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }




        public IEnumerable<BCModel.Scope> GetBidPackageScopes(int bidPackageId)
        {
            return _repo.GetBidPackage(bidPackageId).Scopes.Select(s => s.Scope).ToList();
        }

        public bool CreateBaseBid(BCModel.Projects.BaseBid bid)
        {
            try
            {
                _repo.CreateBaseBid(bid);
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

        public bool UpdateBaseBid(BCModel.Projects.BaseBid bid)
        {
            try
            {
                _repo.UpdateBaseBid(bid);
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

        public bool DeleteBaseBid(BCModel.Projects.BaseBid bid)
        {
            try
            {
                _repo.DeleteBaseBid(bid);
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

        public BCModel.Projects.BaseBid GetBaseBid(int projectId, int sentToId, int scopeId)
        {
            return _repo.GetBaseBid(projectId, sentToId, scopeId);
        }

        public IEnumerable<BCModel.Projects.BaseBid> GetEnumerableBaseBid()
        {
            return _repo.QueryBaseBid().ToList();
        }

        public IEnumerable<BCModel.Projects.BaseBid> GetCompanyBaseBidsForProject(int companyId, int projectId)
        {
            return (from r in _repo.QueryBaseBid()
                    where r.SentToId == companyId
                    && r.ProjectId == projectId
                    select r).ToList();
        }

        public bool CreateComputedBid(BCModel.Projects.ComputedBid bid)
        {
            try
            {
                _repo.CreateComputedBid(bid);
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

        public bool UpdateComputedBid(BCModel.Projects.ComputedBid bid)
        {
            try
            {
                _repo.UpdateComputedBid(bid);
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

        public bool DeleteComputedBid(BCModel.Projects.ComputedBid bid)
        {
            try
            {
                _repo.DeleteComputedBid(bid);
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

        public BCModel.Projects.ComputedBid GetComputedBid(int bidPackageId, int sentToId, int scopeId)
        {
            return _repo.GetComputedBid(bidPackageId, sentToId, scopeId);
        }

        public IEnumerable<BCModel.Projects.ComputedBid> GetEnumerableComputedBid()
        {
            return _repo.QueryComputedBid().ToList();
        }

        public IEnumerable<BCModel.Projects.ComputedBid> GetCompanyComputedBidsForBidPackage(int bidPackageId, int companyId)
        {
            return (from r in _repo.QueryComputedBid()
                    where r.BidPackageId == bidPackageId
                    && r.SentToId == companyId
                    select r).ToList();
        }
    }
}