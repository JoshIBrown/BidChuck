using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Company.ViewModel
{
    public class CompanySearchResult
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}