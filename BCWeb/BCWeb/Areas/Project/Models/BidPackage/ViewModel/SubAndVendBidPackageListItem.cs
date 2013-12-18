using BCWeb.Areas.Project.Models.Documents.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.ViewModel
{
    public class SubAndVendBidPackageListItem
    {
        public int BidPackageId { get; set; }
        public int InvitingCompanyId { get; set; }
        public string InvitingCompanyName { get; set; }
        public string BidDateTime { get; set; }
        public IEnumerable<int> SelectedScopes { get; set; }
        public bool? InviteResponse { get; set; }
        public IEnumerable<ProjectDocLookupItem> ProjectDocs { get; set; }
    }


}