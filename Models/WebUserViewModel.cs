using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RCTPL_WebProjects.Models
{
    public static class GlobalVar
    {
        public static Dictionary<string, string> RegionList { get; set; }
        public static Dictionary<string, string> BranchList { get; set; }
        public static List<string> UserTypeList { get; set; }
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
        [Display(Name = "Last Name")]
        public string lastname { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string firstname { get; set; }
        [Required]
        [Display(Name = "Middle Name")]
        public string middlename { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required]
        [Display(Name = "City")]
        public string city { get; set; }
        [Required]
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
       
        [Display(Name = "Branch Code")]
        public string bCode { get; set; }

        [Display(Name = "User Type")]
        public string userType { get; set; }
        
        public Dictionary<string, string> _list { get; set; }

        public Dictionary<string, string> _listRegion { get; set; }

        public List<string> _listUsrType { get; set; }

        public string errMessage { get; set; }
        public int isError { get; set; }
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



    //===========================================DTO============================================

    public class userDTO
    {
        public string region { get; set; }
        public string _dateV { get; set; }
        public string userName { get; set; }      
    }

    public class TBChDDTO
    {
        public string region { get; set; }
        //public string _dateP { get; set; }
        public string _id { get; set; }
        public decimal _total { get; set; }
    }

    public class orDTO
    {
        //public string colamt { get; set; }
        //public string basic_premium { get; set; }
        //public string doc_stamp { get; set; }
        //public string comp_fee { get; set; }
        //public string cert_fee { get; set; }
        //public string tax_amtd { get; set; }
        //public string e_vat { get; set; }
        //public string lgt { get; set; }
        //public string coi { get; set; }
        //public string colordte { get; set; }
        //public string colpayor { get; set; }
        //public string colno { get; set; }
        //public string clientcd { get; set; }
        //public string colpymtyp { get; set; }
        //public string colornum { get; set; }
        //public string usrid { get; set; }
        //public string with_tax { get; set; }
        //public string tax_amt { get; set; }
        //public string paddress { get; set; }
        //public string lastname { get; set; }
        //public string firstname { get; set; }
        //public string middlename { get; set; }
        public string cashier { get; set; }
        public string plateno { get; set; }
        public string bcdesc { get; set; }
        public string bcdsp { get; set; }
        public string company { get; set; }

        //upper left of Report
        public string papinNo { get; set; }
        public string clientAddress { get; set; }
        public string clientName { get; set; }
        public string assTin { get; set; }

        //upper right of Report
        public string orNo { get; set; }
        public string orDate { get; set; }
        public string paymentType { get; set; }

        //lower left of Report
        public decimal basicPremium { get; set; }
        public decimal eVat { get; set; }
        public decimal otherCharges { get; set; }

        public decimal TotalAmount { get; set; }
        //lower right of Report
        public string cashierAgent { get; set; }
    }
    
}