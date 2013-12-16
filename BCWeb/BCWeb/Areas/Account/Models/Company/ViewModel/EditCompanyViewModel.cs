using BCModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Account.Models.Company.ViewModel
{
    public class EditCompanyViewModel
    {
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> BusinessTypes { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        [RegularExpression("^[a-zA-Z0-9 _-]*")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Business Type")]
        //public int BusinessTypeId { get; set; }
        public BusinessType BusinessType { get; set; }

        [Display(Name = "Address (optional)")]
        [RegularExpression(@"^[a-zA-Z\d\s]*$")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2 (optional)")]
        [RegularExpression(@"^[a-zA-Z\d\s#]*$")]
        public string Address2 { get; set; }

        [Display(Name = "City (optional)")]
        [RegularExpression("^[a-zA-Z -]*")]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "State")]
        [DataType(DataType.PostalCode)]
        public int? StateId { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Operating Distance")]
        public int OperatingDistance { get; set; }
    }
}