using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Admin.Models.Companies
{
    public class CompanyProfileDetailsModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string BusinessType { get; set; }
        public string OperatingDistance { get; set; }
        public string Published { get; set; }
        public string Subscribed { get; set; }
        public Dictionary<int, string> Users { get; set; }

    }
}