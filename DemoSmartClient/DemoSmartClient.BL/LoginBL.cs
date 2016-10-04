using DemoSmartClient.Interface;
using DemoSmartClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoSmartClient.BL
{
    public class LoginBL
    {
        ILogin _ILogin;
        public LoginBL(ILogin ILogin)
        {
            _ILogin = ILogin;
        }
        public string CheckUserExists(string Username, string Password)
        {
            return _ILogin.CheckUserExists(Username, Password);
        }
        public int InsertLoginData(UserLogin LoginModel)
        {
            return _ILogin.InsertLoginData(LoginModel);
        }
    }
}
