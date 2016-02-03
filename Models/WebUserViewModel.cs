using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RCTPL_WebProjects.Models
{
    public class WebUserViewModel
    {
    }

    public class StrViewModel
    {
        public string message { get; set; }
        public string title { get; set; }
        public bool isError { get; set; }
    }

    public class RegViewModel
    {
        [Required]
        [Display(Name="Last Name")]
        public string lastname { get; set; }
        [Required]
        [Display(Name="First Name")]
        public string firstname { get; set; }
        [Display(Name="Middle Name")]
        public string middlename { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Region")]
        public string region { get; set; }
        [Required]
        [Display(Name = "Contact")]
        public string contactno { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }
        public string errMessage { get; set; }
        public bool isError { get; set; }
    }

    public class ForgotPasswdViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

    }


    public class changePasswdViewModel
    {
               
        public string _uname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}