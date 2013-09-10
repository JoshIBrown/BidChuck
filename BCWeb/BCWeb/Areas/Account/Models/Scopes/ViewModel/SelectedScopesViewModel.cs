using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Scopes.ViewModel
{
    public class SelectedScopesViewModel
    {
        public string Type { get; set; }
        public string Ident { get; set; }
        public int[] Selected { get; set; }
    }
}