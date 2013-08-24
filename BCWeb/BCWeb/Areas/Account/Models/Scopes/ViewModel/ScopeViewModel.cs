using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Scopes.ViewModel
{
    public class ScopeViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string CsiNumber { get; set; }
        public string Description { get; set; }
    }
}