using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Models.Project.ViewModel
{
    public class EditProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BidDateTime { get; set; }

        public DateTime? WalkThruDateTime { get; set; }
        public bool NoWalkThru { get; set; }
        public bool WalkThruTBD { get; set; }

        public string Address { get; set; }


        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public int StateId { get; set; }

        [Required]
        public int BuildingTypeId { get; set; }
        [Required]
        public ProjectType? ProjectType { get; set; }
        [Required]
        public ProjectCategory? ProjectCategory { get; set; }
        [Required]
        public int ConstructionTypeId { get; set; }

        public IEnumerable<SelectListItem> ProjectTypes { get; set; }
        public IEnumerable<SelectListItem> ProjectCategories { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> ConstructionTypes { get; set; }
        public IEnumerable<BuildingType> BuildingTypes { get; set; }
        public IEnumerable<int> SelectedScope { get; set; }

        public int ArchitectId { get; set; }

        public string Number { get; set; }

        public string Architect { get; set; }
    }
}