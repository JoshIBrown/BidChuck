using BCModel.Projects;
using BCWeb.Areas.Project.Models.Bids.Repository;
using BCWeb.Areas.Project.Models.Bids.ViewModel;
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


        public bool SetBidDate(int bidPackageId, int companyId, DateTime dateTime)
        {
            try
            {
                Invitation invite = _repo.GetInvite(bidPackageId, companyId);
                invite.BidSentDate = dateTime;
                _repo.UpdateInvitation(invite);
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


        public bool SaveDraft(IEnumerable<BaseBid> baseBids, Dictionary<int, IEnumerable<ComputedBid>> computedBids)
        {
            try
            {
                foreach (BaseBid bb in baseBids)
                {
                    _repo.AddOrUpdateBaseBid(bb);
                }
                foreach (KeyValuePair<int, IEnumerable<ComputedBid>> kvp in computedBids)
                {
                    foreach (ComputedBid cb in kvp.Value)
                    {
                        _repo.AddOrUpdateComputedBid(cb);
                    }
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

        public bool SaveFinalBid(IEnumerable<BaseBid> baseBids, Dictionary<int, IEnumerable<ComputedBid>> computedBids, int companyId, DateTime sentDate)
        {
            try
            {
                Invitation invite;

                foreach (BaseBid bb in baseBids)
                {
                    _repo.AddOrUpdateBaseBid(bb);
                }
                foreach (KeyValuePair<int, IEnumerable<ComputedBid>> kvp in computedBids)
                {

                    // set bid sent date in the invite
                    invite = _repo.GetInvite(kvp.Key, companyId);
                    invite.BidSentDate = sentDate;
                    _repo.UpdateInvitation(invite);

                    foreach (ComputedBid cb in kvp.Value)
                    {
                        _repo.AddOrUpdateComputedBid(cb);
                    }
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


        public BCModel.Projects.BidPackage GetMasterBidPackage(int projectId)
        {
            return (from r in _repo.QueryBidPackages()
                    where r.ProjectId == projectId
                    && r.IsMaster
                    select r).SingleOrDefault();
        }


        public IEnumerable<ComputedBid> GetEnumerableComputedBidsByBidPackage(int bidPackageId)
        {
            return (from r in _repo.QueryComputedBid()
                    where r.BidPackageId == bidPackageId
                    select r).AsEnumerable();
        }


        public IEnumerable<BaseBid> GetBaseBidsForCompanies(int projectId, int[] invitedCompanyId)
        {
            return (from r in _repo.QueryBaseBid()
                    where r.ProjectId == projectId
                    && invitedCompanyId.Contains(r.SentToId)
                    select r).AsEnumerable();
        }


        public IEnumerable<Invitation> GetInvitationListForSender(int projectId, int companyId)
        {
            return (from r in _repo.QueryInvites()
                    where r.BidPackage.ProjectId == projectId
                    && r.BidPackage.CreatedById == companyId
                    select r).AsEnumerable();
        }


        public IEnumerable<BCModel.Projects.BidPackage> GetBidPackagesCreatedByCompanyForProject(int projectId, int companyId)
        {
            return (from r in _repo.QueryBidPackages()
                    where r.ProjectId == projectId
                    && r.CreatedById == companyId
                    && !r.IsMaster
                    select r).AsEnumerable();
        }


        public IEnumerable<Invitation> GetInvitesForBidPackage(int bidPackageId)
        {
            return (from r in _repo.QueryInvites()
                    where r.BidPackageId == bidPackageId
                    select r).AsEnumerable();
        }


        public IEnumerable<CalculatedBid> GetCalculatedBidOfBidPackageForCompany(int bidPackageId, int companyId)
        {
            return (from i in _repo.QueryInvites()
                    join cb in _repo.QueryComputedBid() on new { i.BidPackageId, i.SentToId } equals new { cb.BidPackageId, cb.SentToId }
                    join bp in _repo.QueryBidPackages() on i.BidPackageId equals bp.Id
                    join bb in _repo.QueryBaseBid() on new { bp.ProjectId, i.SentToId, cb.ScopeId } equals new { bb.ProjectId, bb.SentToId, bb.ScopeId }
                    where i.BidPackageId == bidPackageId
                    && i.SentToId == companyId
                    && i.BidSentDate.HasValue
                    && i.AcceptedDate.HasValue
                    select new CalculatedBid
                    {
                        BidPackageId = bp.Id,
                        ScopeId = cb.ScopeId,
                        SentToId = cb.SentToId,
                        CalculatedAmount = cb.RiskFactor.Value * bb.Amount
                    }).AsEnumerable();
        }


        public int[] GetCompanyIdsThatSubmittedBid(int bidPackageId)
        {
            return (from i in _repo.QueryInvites()
                    where i.BidPackageId == bidPackageId
                    && i.AcceptedDate.HasValue
                    && i.BidSentDate.HasValue
                    select i.SentToId).ToArray();
        }


        public IEnumerable<BCModel.CompanyProfile> GetCompaniessThatSubmittedBid(int bidPackageId)
        {
            return (from i in _repo.QueryInvites()
                    where i.BidPackageId == bidPackageId
                    && i.AcceptedDate.HasValue
                    && i.BidSentDate.HasValue
                    select i.SentTo).AsEnumerable();
        }


        public IEnumerable<Invitation> GetInvitesCompanyHasAccepted(int projectId, int companyId)
        {
            return (from r in _repo.QueryInvites()
                    where r.BidPackage.ProjectId == projectId
                    && r.SentToId == companyId
                    && r.AcceptedDate.HasValue
                    select r).ToList();
        }


        public IEnumerable<BCModel.Scope> GetCompanyScopesForProject(int projectId, int companyId)
        {
            return (from bp in _repo.QueryBidPackages()
                    join i in _repo.QueryInvites() on bp.Id equals i.BidPackageId
                    join s in _repo.QueryBidPackageScopes() on bp.Id equals s.BidPackageId
                    where bp.ProjectId == projectId
                    && i.SentToId == companyId
                    select s.Scope).Distinct().ToList();
        }
    }
}