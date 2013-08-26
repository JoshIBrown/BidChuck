using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Account.Models.Users.ViewModel
{
    public class NewUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Confirm Email")]
        [Compare("Email", ErrorMessage = "Email addresses do not match")]
        public string EmailConfirm { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}