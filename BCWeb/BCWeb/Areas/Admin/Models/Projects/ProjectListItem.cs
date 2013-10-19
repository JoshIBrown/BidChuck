using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Admin.Models.Projects
{
    public class ProjectListItem
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public DateTime BidDate { get; set; }
        public string State { get; set; }
        public string CreatedBy { get; set; }
        public string Architect { get; set; }
        public string BuildingType { get; set; }
        public string ConstructionType { get; set; }
        public string ProjectType { get; set; }
    }
}