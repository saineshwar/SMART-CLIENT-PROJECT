using SmartWebApp.Interface;
using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartWebApp.BL
{
    public class RegistrationBL
    {
        IRegistration _IRegistration;
        public RegistrationBL(IRegistration IRegistration)
        {
            _IRegistration = IRegistration;
        }

        public int CreateUser(Registration registration)
        {
           return _IRegistration.CreateUser(registration);
        }

        public bool ValidateUser(string Username)
        {
            return _IRegistration.ValidateUser(Username);
        }

    }
}
