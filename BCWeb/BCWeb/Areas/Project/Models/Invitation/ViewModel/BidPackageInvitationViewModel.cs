using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Invitation.ViewModel
{
    public class BidPackageInvitationViewModel
    {
        public IEnumerable<int> CompanyId { get; set; }
        public int BidPackageId { get; set; }
    }
}