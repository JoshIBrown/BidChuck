using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Bids.ViewModel
{
    public class CalculatedBid
    {
        public int BidPackageId { get; set; }
        public int ScopeId { get; set; }
        public int SentToId { get; set; }
        public decimal CalculatedAmount { get; set; }
    }
}