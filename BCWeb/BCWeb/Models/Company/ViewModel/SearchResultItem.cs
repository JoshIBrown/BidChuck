using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Company.ViewModel
{
    public class CompanySearchResultItem
    {

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
        public bool BlackListed { get; set; }
        public Dictionary<int, string> ScopesOfWork { get; set; }
        public string LinkPath { get; set; }

    }
}