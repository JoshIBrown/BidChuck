using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Account.ViewModel
{
    public class ManageDashboardViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string BusinessType { get; set; }
        public string OperatingRadius { get; set; }
        public bool Published { get; set; }

        public IEnumerable<string> Scopes { get; set; }
        public IEnumerable<MinionOverviewViewModel> Minions { get; set; }
    }

    public class MinionOverviewViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Confirmed { get; set; }
    }
}