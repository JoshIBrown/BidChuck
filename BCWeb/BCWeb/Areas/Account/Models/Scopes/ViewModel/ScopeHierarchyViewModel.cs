using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Scopes.ViewModel
{
    public class ScopeHierarchyViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? Parent { get; set; }
        public IEnumerable<ScopeHierarchyViewModel> Children { get; set; }
    }
}