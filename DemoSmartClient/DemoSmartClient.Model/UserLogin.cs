using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoSmartClient.Model
{
    public class UserLogin
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RegistrationID { get; set; }
    }
}
