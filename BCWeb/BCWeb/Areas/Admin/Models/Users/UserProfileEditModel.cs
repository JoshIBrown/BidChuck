using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Admin.Models.Users
{
    public class UserProfileEditModel
    {
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string JobTitle { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }
    }
}