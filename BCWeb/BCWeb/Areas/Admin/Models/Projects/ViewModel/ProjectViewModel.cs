using BCModel.Projects;
using BCWeb.Models.Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Admin.Models.Projects.ViewModel
{
    public class ProjectViewModel
    {
        public int Id { get; set; }


        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }

        public int ArchitectId { get; set; }
        public string Architect { get; set; }


        public string Title { get; set; }

        
        public string Number { get; set; }

        
        public string Description { get; set; }

        
        public string BidDateTime { get; set; }

        public string WalkThruDateTime { get; set; }
        
        public string WalkThruStatus { get; set; }

        public string Address { get; set; }
        
        public string City { get; set; }
        
        public string PostalCode { get; set; }
        
        public string State { get; set; }

        public string BuildingType { get; set; }
        
        public string ProjectType { get; set; }
      
        public string  ProjectCategory { get; set; }

        public string ConstructionType { get; set; }

        public IEnumerable<ProjectScopeListItem> SelectedScope { get; set; }
    }
}