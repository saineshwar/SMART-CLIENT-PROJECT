using Newtonsoft.Json;
using SmartWebApp.BL;
using SmartWebApp.Concrete;
using SmartWebApp.CryptoLib;
using SmartWebApp.Filters;
using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SmartWebApp.Controllers
{
    public class AuthenticateUserController : ApiController
    {

        LoginBL LoginBL;

        public AuthenticateUserController()
        {
            LoginBL = new LoginBL(new LoginConcrete());
        }

     
        // POST api/authenticateuser
        [AuthoriseAPI]
        public string Post(HttpRequestMessage Request)
        {
            try
            {
                if (Request != null)
                {
                    var Responsedata = Request.Content.ReadAsStringAsync().Result;
                    string keyValue = ConfigurationManager.AppSettings["keyValue"].ToString();
                    string IVValue = ConfigurationManager.AppSettings["IVValue"].ToString();
                    string data = EncryptionDecryptorTripleDES.Decryption(Responsedata, keyValue, IVValue);
                    LoginViewModel objVM = new JavaScriptSerializer().Deserialize<LoginViewModel>(data);
                    var val = LoginBL.ValidateLoginUser(objVM.Username, objVM.Password);
                    string SerializeData = JsonConvert.SerializeObject(val);
                    byte[] buffer = EncryptionDecryptorTripleDES.Encryption(SerializeData, keyValue, IVValue);
                    return Convert.ToBase64String(buffer);
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception)
            {               
                throw;
            }
        }
    }
}
