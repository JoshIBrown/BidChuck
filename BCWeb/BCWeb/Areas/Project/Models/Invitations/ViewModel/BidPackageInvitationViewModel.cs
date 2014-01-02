using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Invitations.ViewModel
{
    public class BidPackageInvitationViewModel
    {
        [Required]
        public int[] CompanyId { get; set; }
        [Required]
        public int BidPackageId { get; set; }
        
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
        public string BidPackageTitle { get; set; }
        public int BPScopeCount { get; set; }
    }
}