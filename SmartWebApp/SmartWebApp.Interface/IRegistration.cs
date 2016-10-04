using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartWebApp.Interface
{
    public interface IRegistration
    {
        int CreateUser(Registration registration);
        bool ValidateUser(string Username);
    }
}
