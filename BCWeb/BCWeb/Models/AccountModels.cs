using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System;

namespace BCWeb.Models
{
    public class ExternalSignIn
    {
        public string Provider { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ProviderUserId { get; set; }
    }

    public class LocalPasswordModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
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

    public class RegisterExternalSignInModel
    {
        public string ExternalSignInData { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ResendConfirmationEmailModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Company Name")]
        [RegularExpression("^[a-zA-Z0-9 _-]*")]
        public string CompanyName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddressAttribute]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression("^[a-zA-Z]*")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression("^[a-zA-Z]*")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string Phone { get; set; }

        //[Required]
        //[Display(Name = "Pick a Username")]
        //[StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        //[RegularExpression("^[a-zA-Z0-9_]*")]
        //public string UserName { get; set; }
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

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }
    }

    //[Table("webpages_Membership")]
    //public class webpages_Membership
    //{
    //    [Key]
    //    public int UserId { get; set; }
    //    public Nullable<DateTime> CreateDate { get; set; }
    //    public string ConfirmationToken { get; set; }
    //    public Nullable<bool> IsConfirmed { get; set; }
    //    public Nullable<DateTime> LastPasswordFailureDate { get; set; }
    //    public int PasswordFailuresSinceLastSuccess { get; set; }
    //    public string Password { get; set; }
    //    public Nullable<DateTime> PasswordChangedDate { get; set; }
    //    public string PasswordSalt { get; set; }
    //    public string PasswordVerificationToken { get; set; }
    //    public Nullable<DateTime> PasswordVerificationTokenExpirationDate { get; set; }
    //}

    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        //public DbSet<webpages_Membership> webpages_Membership { get; set; }
    }
}