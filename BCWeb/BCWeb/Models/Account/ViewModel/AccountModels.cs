using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BCModel;

namespace BCWeb.Models
{
    public class LocalPasswordModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string PasswordResetToken { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
    }

    public class ResendConfirmationEmailModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

    public class RegisterModel
    {
        public IEnumerable<SelectListItem> States { get; set; }
        //public IEnumerable<SelectListItem> BusinessTypes { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        [RegularExpression("^[a-zA-Z0-9 _-]*")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Business Type")]
        //public int BusinessTypeId { get; set; }
        public BusinessType BusinessType { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddressAttribute]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^\w*")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^\w*")]
        public string LastName { get; set; }


        [Display(Name = "Address (optional)")]
        [RegularExpression(@"^[a-zA-Z\d\s-]+$")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2 (optional)")]
        [RegularExpression(@"^[a-zA-Z\d\s#-]*$")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        [RegularExpression("^[a-zA-Z -]*")]
        [Required]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Operating Distance")]
        public int OperatingDistance { get; set; }
    }

    public class SignInModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }
}
