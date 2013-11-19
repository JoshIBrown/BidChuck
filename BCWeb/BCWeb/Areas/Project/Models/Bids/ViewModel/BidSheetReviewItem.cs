using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Bids.ViewModel
{
    public class BidSheetReviewItem
    {
        public int BidPackageId { get; set; }
        public string BidPackageDesc { get; set; }
        public IEnumerable<CompanyBidReviewItem> CompanyBids { get; set; }
    }

    public class CompanyBidReviewItem
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<ScopeBidReviewItem> ScopeBids { get; set; }
    }

    public class ScopeBidReviewItem
    {
        public int ScopeId { get; set; }
        public decimal BidAmount { get; set; }
    }
}