using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoSmartClient.CommonLib
{
    public static class CheckInternetConnection
    {
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool CheckNet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
    }
}
