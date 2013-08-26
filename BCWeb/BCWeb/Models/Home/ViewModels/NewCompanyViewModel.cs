using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Home.ViewModels
{
    public class NewCompanyViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
        public string[] Scopes { get; set; }
    }
}