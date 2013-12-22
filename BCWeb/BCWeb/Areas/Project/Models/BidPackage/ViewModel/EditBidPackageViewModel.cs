using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.ViewModel
{
    public class EditBidPackageViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public int[] SelectedScope { get; set; }
        public bool UseProjectBidDate { get; set; }
        public DateTime? BidDateTime { get; set; }
        public bool UseProjectWalkThru { get; set; }
        public DateTime? WalkThruDateTime { get; set; }
        public WalkThruStatus? WalkThruStatus { get; set; }
        public string Notes { get; set; }

        public int? TemplateId { get; set; }
    }
}