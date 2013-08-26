using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Account.ViewModel
{
    public class EditEmailViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Current Email")]
        [EmailAddress]
        public string CurrentEmail { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "New Email")]
        public string NewEmail { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Confirm New Email")]
        [Compare("NewEmail", ErrorMessage = "Email addresses do not match")]
        public string NewEmailConfirm { get; set; }

        [Required]
        public string Password { get; set; }
    }
}