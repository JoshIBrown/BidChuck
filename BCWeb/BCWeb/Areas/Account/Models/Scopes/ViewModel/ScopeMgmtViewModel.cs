using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Scopes.ViewModel
{
    public class ScopeMgmtViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
    }
}