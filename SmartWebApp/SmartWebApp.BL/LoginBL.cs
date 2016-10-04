using SmartWebApp.Interface;
using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartWebApp.BL
{
    public class LoginBL
    {
        ILogin _ILogin;
        public LoginBL(ILogin ILogin)
        {
            _ILogin = ILogin;
        }

        public LoginViewResponse ValidateLoginUser(string Username, string Password)
        {
            return _ILogin.ValidateLoginUser(Username, Password);
        }
    }
}
