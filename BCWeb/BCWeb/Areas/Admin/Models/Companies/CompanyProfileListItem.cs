using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Admin.Models.Companies
{
    public class CompanyProfileListItem
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
        public bool Published { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Manager { get; set; }
    }
}