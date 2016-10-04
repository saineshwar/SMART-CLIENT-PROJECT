using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SmartWebApp.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Username ")]
        [StringLength(30)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class LoginViewResponse
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RegistrationID { get; set; }
    }

}
