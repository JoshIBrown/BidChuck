using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Project.Models.Bids.ServiceLayer
{
    public interface IBidServiceLayer
    {
        bool CreateBaseBid(BaseBid bid);
        bool UpdateBaseBid(BaseBid bid);
        bool DeleteBaseBid(BaseBid bid);
        BaseBid GetBaseBid(int projectId, int sentToId, int scopeId);
        IEnumerable<BaseBid> GetEnumerableBaseBid();
        IEnumerable<BaseBid> GetCompanyBaseBidsForProject(int companyId, int projectId);

        bool CreateComputedBid(ComputedBid bid);
        bool UpdateComputedBid(ComputedBid bid);
        bool DeleteComputedBid(ComputedBid bid);
        ComputedBid GetComputedBid(int bidPackageId, int sentToId, int scopeId);
        IEnumerable<ComputedBid> GetEnumerableComputedBid();
        IEnumerable<ComputedBid> GetCompanyComputedBidsForBidPackage(int bidPackageId, int companyId);

        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);
        BCModel.Projects.Project GetProject(int id);
        BCModel.Projects.BidPackage GetBidPackage(int id);
        BCModel.Projects.Invitation GetInvite(int bidPackageId, int companyId);
        IEnumerable<Invitation> GetInvites(int projectId, int companyId);
        IEnumerable<Scope> GetBidPackageScopes(int bidPackageId);
    }
}
