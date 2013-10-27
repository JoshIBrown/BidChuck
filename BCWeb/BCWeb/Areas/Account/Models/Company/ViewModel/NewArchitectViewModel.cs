using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Company.ViewModel
{
    public class NewArchitectViewModel
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Required]
        public string ContactFirstName { get; set; }
        [Required]
        public string ContactLastName { get; set; }

        public string ProjectNumber { get; set; }
        public string ProjectTitle { get; set; }
    }
}