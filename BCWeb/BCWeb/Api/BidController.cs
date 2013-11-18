using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Project.Models.Bids.ServiceLayer;
using BCWeb.Areas.Project.Models.Bids.ViewModel;
using BCWeb.Models;
using BCWeb.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace BCWeb.Api
{
    public class BidController : ApiController
    {
        private IBidServiceLayer _service;
        private IWebSecurityWrapper _security;

        public BidController(IBidServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        //public object GetBidsForProject(int projectId, int page, int rowx)
        //{
        //    int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
        //    // get invites
        //    List<Invitation> invites = _service.GetInvitationListForSender(projectId, companyId).ToList();
        //    int[] bidPackageId = invites.Select(i => i.BidPackageId).Distinct().ToArray();

        //    // will be used to generate tabs for each bid package
        //    Dictionary<int, Scope[]> bidPackageScopeDic = new Dictionary<int, Scope[]>();
        //    for (int i = 0; i < bidPackageId.Length; i++)
        //    {
        //        bidPackageScopeDic.Add(bidPackageId[i], _service.GetBidPackageScopes(bidPackageId[i]).ToArray());
        //    }

        //    // get computed bids
        //    // get base bids


        //    return null;
        //}

        public IEnumerable<SelectListItem> GetBidPackagesForProject(int projectId)
        {
            CompanyProfile company = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).Company;

            List<SelectListItem> result = new List<SelectListItem>();
            if (company.BusinessType == BusinessType.Architect)
            {
                var masterBp = _service.GetMasterBidPackage(projectId);
                result.Add(new SelectListItem { Value = masterBp.Id.ToString(), Text = "" });

            }
            else
            {
                result = _service.GetBidPackagesCreatedByCompanyForProject(projectId, company.Id).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Description }).ToList();
            }
            return result;
        }

        public IEnumerable<BidSheetReviewItem> GetBidsToReviewForProject(int projectId)
        {
            CompanyProfile company = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).Company;

            List<BidSheetReviewItem> result = new List<BidSheetReviewItem>();
            List<CompanyBidReviewItem> companyBidList;
            List<ScopeBidReviewItem> scopeBidList;
            if (company.BusinessType == BusinessType.Architect)
            {
                BidPackage masterBp = _service.GetMasterBidPackage(projectId);                                      // get master bid package because user is an architect
                int[] biddingCompanyId = _service.GetCompanyIdsThatSubmittedBid(masterBp.Id);                       // create array of company's ids that have submitted a bid
                List<BaseBid> baseBids = _service.GetBaseBidsForCompanies(projectId, biddingCompanyId).ToList();    // get base bids using project id and invited companies


                companyBidList = new List<CompanyBidReviewItem>();      // instantiate a new list of company bids
                for (int i = 0; i < biddingCompanyId.Length; i++)       // loop through invited companies
                {
                    scopeBidList = new List<ScopeBidReviewItem>();      // create new list of scope bids for every company

                    scopeBidList.AddRange(baseBids.Where(z => z.SentToId == 0).Select(z => new ScopeBidReviewItem { ScopeId = z.ScopeId, BidAmount = z.Amount }));      // fill scope bid list with scopes and amounts for the invited company

                    companyBidList.Add(new CompanyBidReviewItem { CompanyId = biddingCompanyId[i], ScopeBids = scopeBidList });             // add company and their bid to the company bid list
                }
                result.Add(new BidSheetReviewItem { BidPackageId = masterBp.Id, CompanyBids = companyBidList });            // add bid package and it's bids to the result set
            }
            else  // else user is a gc, sub or vendor
            {
                int[] biddingCompanyId;
                List<BidPackage> bidPackages = _service.GetBidPackagesCreatedByCompanyForProject(projectId, company.Id).ToList();
                for (int i = 0; i < bidPackages.Count; i++)
                {

                    biddingCompanyId = _service.GetCompanyIdsThatSubmittedBid(bidPackages[i].Id);
                    companyBidList = new List<CompanyBidReviewItem>();
                    for (int j = 0; j < biddingCompanyId.Length; i++)
                    {
                        scopeBidList = _service.GetCalculatedBidOfBidPackageForCompany(bidPackages[i].Id, biddingCompanyId[j])
                            .Select(p => new ScopeBidReviewItem { BidAmount = p.CalculatedAmount, ScopeId = p.ScopeId })
                            .ToList();
                        // add company to bp list
                        companyBidList.Add(new CompanyBidReviewItem { CompanyId = biddingCompanyId[j], ScopeBids = scopeBidList });
                    }
                    // add bid package item to result
                    result.Add(new BidSheetReviewItem { BidPackageId = bidPackages[i].Id, CompanyBids = companyBidList });
                }
            }
            return result;
        }

        public BidSheetReviewItem GetBidsToReviewForBidPackage(int bidPackageId)
        {
            CompanyProfile company = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).Company;

            BidSheetReviewItem result = new BidSheetReviewItem();
            List<CompanyBidReviewItem> companyBidList;
            List<ScopeBidReviewItem> scopeBidList;

            BidPackage theBidPackge = _service.GetBidPackage(bidPackageId);
            result.BidPackageId = bidPackageId;
            int[] biddingCompanyId = _service.GetCompanyIdsThatSubmittedBid(bidPackageId);

            companyBidList = new List<CompanyBidReviewItem>();

            for (int i = 0; i < biddingCompanyId.Length; i++)
            {
                scopeBidList = _service.GetCalculatedBidOfBidPackageForCompany(bidPackageId, biddingCompanyId[i])
                    .Select(p => new ScopeBidReviewItem { BidAmount = p.CalculatedAmount, ScopeId = p.ScopeId })
                    .ToList();
                companyBidList.Add(new CompanyBidReviewItem { CompanyId = biddingCompanyId[i], ScopeBids = scopeBidList });
            }

            result.CompanyBids = companyBidList;

            return result;
        }

        public IEnumerable<LookupItem> GetScopesForBidPackages(int bidPackageId)
        {
            LookupItem[] result = _service.GetBidPackageScopes(bidPackageId).Select(p => new LookupItem { Id = p.Id, Value = p.CsiNumber + " " + p.Description }).ToArray();

            return result;
        }
    }
}
