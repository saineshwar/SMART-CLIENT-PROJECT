using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace SmartWebApp.Model
{
public class Registration
{
 [Key]
 public long RegistrationID { get; set; }

 [Required(ErrorMessage = "Enter First Name")]
 [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
 public string FirstName { get; set; }

 [Required(ErrorMessage = "Enter Last Name")]
 public string Lastname { get; set; }

 [Required(ErrorMessage = "Select Gender")]
 public string Gender { get; set; }

 [Required(ErrorMessage = "Enter Username ")]
 [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
 [Remote("ValidateUser", "Register", ErrorMessage = "Username Already Exists")]
 public string Username { get; set; }

 [Required(ErrorMessage = "Enter Password")]
 [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
 [DataType(DataType.Password)]
 public string Password { get; set; }

 [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
 [Required(ErrorMessage = "Enter Confirm Password")]
 [DataType(DataType.Password)]
 public string ConfirmPassword { get; set; }

 [Required(ErrorMessage = "Enter Mobileno")]
 public string Mobileno { get; set; }

 [Required(ErrorMessage = "Enter EmailID")]
 [DataType(DataType.EmailAddress)]
 public string EmailID { get; set; }

}
}
