using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Admin.Models.Projects
{
    public class ProjectEditModel
    {
        public int Id { get; set; }

        [Required]
        public int CreatedById { get; set; }
        public IEnumerable<SelectListItem> CreatedBy { get; set; }

        [Required]
        public int ArchitectId { get; set; }
        public IEnumerable<SelectListItem> Architects { get; set; }


        //public int? ClientId { get; set; }
        //[ForeignKey("ClientId")]
        //[IgnoreDataMember]
        //public virtual CompanyProfile Client { get; set; }


        [Required]
        public string Title { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime BidDateTime { get; set; }

        public DateTime? WalkThruDateTime { get; set; }
        [Required]
        public WalkThruStatus? WalkThruStatus { get; set; }

        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public int StateId { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }


        public int BuildingTypeId { get; set; }
        public IEnumerable<SelectListItem> BuildingTypes { get; set; }

        public ProjectType? ProjectType { get; set; }
        public IEnumerable<SelectListItem> ProjectTypes { get; set; }

        public ProjectCategory? ProjectCategory { get; set; }
        public IEnumerable<SelectListItem> ProjectCategories { get; set; }

        public int ConstructionTypeId { get; set; }
        public IEnumerable<SelectListItem> ConstructionTypes { get; set; }


        public IEnumerable<int> SelectedScope { get; set; }
    }
}