using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoSmartClient.CommonLib
{
    public static class ShareObject
    {
        private static string _CLientIDToken;

        public static string CLientIDToken
        {
            get { return _CLientIDToken; }
            set { _CLientIDToken = value; }
        }


        private static string _Username;

        public static string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
    }


    
    public static class NetConnectionChecker
    {
        private static bool _Connection;

        public static bool Connection
        {
            get { return _Connection; }
            set { _Connection = value; }
        }
    } 
}
