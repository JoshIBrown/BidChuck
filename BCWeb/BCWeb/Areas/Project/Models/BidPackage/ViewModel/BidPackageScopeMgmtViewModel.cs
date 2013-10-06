using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.ViewModel
{
    public class BidPackageScopeMgmtViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string CsiNumber { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
        public bool Templated { get; set; }
    }
}