using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.ViewModel
{
    public class ProjectBidPackagesViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<BidPackageListItemViewModel> BidPackages { get; set; }
    }
}