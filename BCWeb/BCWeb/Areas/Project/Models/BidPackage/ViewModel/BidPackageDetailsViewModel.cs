using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.ViewModel
{
    public class BidPackageDetailsViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
        public string Architect { get; set; }
        public string CreatingCompany { get; set; }
        public string BidDateTime { get; set; }
        public string WalkThruDateTime { get; set; }
        public string Notes { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        
    }
}