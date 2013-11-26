using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.Bids.ViewModel;
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

        BaseBid GetBaseBid(int projectId, int sentToId, int scopeId);
        IEnumerable<BaseBid> GetEnumerableBaseBid();
        IEnumerable<BaseBid> GetCompanyBaseBidsForProject(int companyId, int projectId);
        IEnumerable<BaseBid> GetBaseBidsForCompanies(int projectId, int[] invitedCompanyId);

        ComputedBid GetComputedBid(int bidPackageId, int sentToId, int scopeId);
        IEnumerable<ComputedBid> GetEnumerableComputedBid();
        IEnumerable<ComputedBid> GetCompanyComputedBidsForBidPackage(int bidPackageId, int companyId);
        IEnumerable<ComputedBid> GetEnumerableComputedBidsByBidPackage(int bidPackageId);

        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);
        IEnumerable<BCModel.CompanyProfile> GetCompaniessThatSubmittedBid(int bidPackageId);
        int[] GetCompanyIdsThatSubmittedBid(int bidPackageId);

        BCModel.Projects.Project GetProject(int id);
        BCModel.Projects.BidPackage GetBidPackage(int id);
        BCModel.Projects.Invitation GetInvite(int bidPackageId, int companyId);
        IEnumerable<Invitation> GetInvites(int projectId, int companyId);
        IEnumerable<Scope> GetBidPackageScopes(int bidPackageId);
        IEnumerable<Scope> GetCompanyScopesForProject(int projectId, int companyId);

        bool SetBidDate(int bidPackageId, int companyId, DateTime dateTime);

        bool SaveDraft(IEnumerable<BaseBid> baseBids, Dictionary<int, IEnumerable<ComputedBid>> computedBids);

        bool SaveFinalBid(IEnumerable<BaseBid> baseBids, Dictionary<int, IEnumerable<ComputedBid>> computedBids, int companyId, DateTime dateTime);

        Dictionary<string, string> ValidationDic { get; }

        BCModel.Projects.BidPackage GetMasterBidPackage(int projectId);

        IEnumerable<Invitation> GetInvitationListForSender(int projectId, int companyId);
        IEnumerable<Invitation> GetInvitesForBidPackage(int bidPackageId);
        IEnumerable<Invitation> GetInvitesCompanyHasAccepted(int projectId, int companyId);

        IEnumerable<BCModel.Projects.BidPackage> GetBidPackagesCreatedByCompanyForProject(int projectId, int companyId);

        IEnumerable<CalculatedBid> GetCalculatedBidOfBidPackageForCompany(int bidPackageId, int companyId);

    }
}
