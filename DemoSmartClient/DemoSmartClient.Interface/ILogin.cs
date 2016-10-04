using DemoSmartClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoSmartClient.Interface
{
    public interface ILogin
    {
        string CheckUserExists(string Username, string Password);
        int InsertLoginData(UserLogin LoginModel);
    }
}
