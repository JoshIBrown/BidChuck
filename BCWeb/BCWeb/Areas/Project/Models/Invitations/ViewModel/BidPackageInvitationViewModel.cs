using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Invitations.ViewModel
{
    public class BidPackageInvitationViewModel
    {
        public int[] CompanyId { get; set; }
        public int BidPackageId { get; set; }
        public string ProjectName { get; set; }
        public string BidPackageTitle { get; set; }
        public int BPScopeCount { get; set; }
    }
}