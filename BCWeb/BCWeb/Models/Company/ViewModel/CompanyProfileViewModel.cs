using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Company.ViewModel
{
    public class CompanyProfileViewModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public bool Subscribed { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public string BusinessType { get; set; }
        public string OperatingDistance { get; set; }

        public IEnumerable<string> WorkScopes { get; set; }
    }
}