using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Bids.ViewModel
{
    public class GCBidViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<BaseBidViewItem> BaseBids { get; set; }
    }

    public class BaseBidViewItem
    {
        public string ScopeDescription { get; set; }
        public decimal Amount { get; set; }
    }
}