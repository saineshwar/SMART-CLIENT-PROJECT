using SmartWebApp.CryptoLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SmartWebApp.Filters
{
    public class AuthoriseAPI : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                string keyValue = ConfigurationManager.AppSettings["keyValue"].ToString();
                string IVValue = ConfigurationManager.AppSettings["IVValue"].ToString();
                string SmartWeb_APIKEY = ConfigurationManager.AppSettings["APIKEY"].ToString();

                IEnumerable<string> tokenHeaders;
                if (actionContext.Request.Headers.TryGetValues("APIKEY", out tokenHeaders))
                {
                    string tokens = tokenHeaders.First();
                    string key = Encoding.UTF8.GetString(Convert.FromBase64String(tokens));
                    string[] parts = key.Split(new char[] { ':' });

                    if (tokens != null)
                    {
                        string Windows_APIKEY = parts[0]; //Hash 1 (Received in API request)
                        string hash2 = parts[1]; //Hash 2

                        //Hash 2 Decryption
                        string DecryHash1 = EncryptionDecryptorTripleDES.Decryption(hash2, keyValue, IVValue);
                        //Spliting Values
                        string[] datapart = DecryHash1.Split(new char[] { ':' });
                        // 1) Hash 2 Contains Username
                        string username = datapart[0];
                        // 2) Hash 2 Contains Ticks
                        long ticks = long.Parse(datapart[1]);     

                        DateTime currentdate = new DateTime(ticks);

                        //Comparing Current Date with date sent
                        if (currentdate.Date == DateTime.Now.Date)
                        {
                            //Hash 1 Decryption
                            string DecryAPIKEY = EncryptionDecryptorTripleDES.Decryption(Windows_APIKEY, keyValue, IVValue);

                            // DecryHash2 Contains ClientToken
                            if (string.Equals(DecryAPIKEY, SmartWeb_APIKEY, comparisonType: StringComparison.InvariantCulture) == true)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {    
                throw;
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Content = new StringContent("Not Valid Client!")
            };
        }
    }
}