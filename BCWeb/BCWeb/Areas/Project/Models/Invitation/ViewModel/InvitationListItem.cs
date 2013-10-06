using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Invitation.ViewModel
{
    public class InvitationListItem
    {
        public int Id { get; set; }
        public int BidPackageId { get; set; }
        public int InvitedCompanyId { get; set; }
        public string CompanyName { get; set; }
        public string SentDate { get; set; }
        public string Status { get; set; }
        public int SortOrder { get; set; }
    }
}