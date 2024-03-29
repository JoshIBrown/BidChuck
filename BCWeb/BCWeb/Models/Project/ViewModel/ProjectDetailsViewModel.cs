﻿using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.ViewModel
{
    public class ProjectDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        public string Number { get; set; }

        public string Architect { get; set; }

        public string Owner { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Bid Date/Time")]
        public string BidDateTime { get; set; }

        public string WalkThru { get; set; }

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

    }
}