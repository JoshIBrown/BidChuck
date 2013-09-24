using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.ViewModel
{
    public class BidPackageListItemViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string BidDateTime { get; set; }
        public int Invited { get; set; }
    }
}