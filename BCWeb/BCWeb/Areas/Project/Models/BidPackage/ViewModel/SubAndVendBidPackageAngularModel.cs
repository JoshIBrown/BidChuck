using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.ViewModel
{
    public class SubAndVendBidPackageAngularModel
    {
        public IEnumerable<KeyValuePair<int, string>> Scopes { get; set; }
        public IEnumerable<SubAndVendBidPackageListItem> BidPackages { get; set; }
    }
}