using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Admin.Models.Users
{
    public class UserProfileDetailsModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public List<string> Roles { get; set; }
        public string IsManager { get; set; }
        public string IsConfirmed { get; set; }
    }
}