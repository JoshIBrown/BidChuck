using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Bids.ViewModel
{
    public class ComposeGCViewModel
    {
        public int ProjectId { get; set; }
        public int BidPackageId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<object> Scopes { get; set; }
        public IEnumerable<object> BaseBids { get; set; }
    }
}