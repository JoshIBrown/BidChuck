using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.ViewModel
{
    public class ProjectBPViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        [Display(Name = "Bid Date/Time")]
        public DateTime BidDateTime { get; set; }

        [Display(Name = "Walk Through Date/Time")]
        public DateTime? SiteVisitDateTime { get; set; }

        [Display(Name = "Selected Scopes")]
        public IEnumerable<string> SelectedScope { get; set; }
    }
}