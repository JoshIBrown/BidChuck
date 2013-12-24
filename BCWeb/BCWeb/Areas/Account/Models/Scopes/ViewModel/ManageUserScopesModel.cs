using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Scopes.ViewModel
{
    public class ManageUserScopesModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int[] SelectedScope { get; set; }
    }
}