using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartWebApp.Interface
{
    public interface ILogin
    {
         LoginViewResponse ValidateLoginUser(string Username, string Password);
    }
}
