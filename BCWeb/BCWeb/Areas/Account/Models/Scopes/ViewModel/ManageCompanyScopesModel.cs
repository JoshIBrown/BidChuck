using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Scopes.ViewModel
{
    public class ManageCompanyScopesModel
    {
        public int CompanyId { get; set; }
        public int[] SelectedScope { get; set; }
    }
}