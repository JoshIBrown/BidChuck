using BCModel.Projects;
using BCWeb.Areas.Project.Models.Documents.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.ViewModel
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }
        public int BidPackageId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        public string Number { get; set; }

        public string Architect { get; set; }

        public string Owner { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Bid Date/Time")]
        public string BidDateTime { get; set; }

        public string WalkThruDate { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        public string State { get; set; }

        [Display(Name = "BuildingType")]
        public string BuildingType { get; set; }
        [Display(Name = "Project Type")]
        public string ProjectType { get; set; }
        [Display(Name = "Construction Type")]
        public string ConstructionType { get; set; }

        public bool? Accepted { get; set; }
        public DateTime? ResponseDate { get; set; }
        public DateTime? BidSentDate { get; set; }

        [Display(Name = "Selected Scopes")]
        public IEnumerable<ProjectScopeListItem> SelectedScope { get; set; }

        public IEnumerable<ProjectDocListItem> ProjectDocs { get; set; }

        public InvitationType? InviteType { get; set; }

        public int? Invited { get; set; }
        public int? AcceptedInvite { get; set; }
        public int? DeclinedInvite { get; set; }
        public int? SubmittedBid { get; set; }
    }
}