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
    }
}