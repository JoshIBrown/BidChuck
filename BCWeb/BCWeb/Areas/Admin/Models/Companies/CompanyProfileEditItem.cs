using BCModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Admin.Models.Companies
{
    public class CompanyProfileEditItem
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }


        public string PostalCode { get; set; }

        public int StateId { get; set; }
        
        public IEnumerable<SelectListItem> States  { get; set; }

        public string Phone { get; set; }



        public bool Published { get; set; }

        public int OperatingDistance { get; set; }

        //public string BusinessLicense { get; set; }

        public string Website { get; set; }

        [Required]
        public BusinessType? BusinessType { get; set; }
        public IEnumerable<SelectListItem> BusinessTypes { get; set; }

    }
}