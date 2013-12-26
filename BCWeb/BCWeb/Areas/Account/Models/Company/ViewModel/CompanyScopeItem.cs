using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Company.ViewModel
{
    public class CompanyScopeItem
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
        public IEnumerable<string> ScopesOfWork { get; set; }
    }
}