using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DemoSmartClient.CryptoLib
{
    public static class GenerateToken
    {
        /// <summary>
        /// Generating Token Logic
        /// </summary>
        /// <param name="username"></param>
        /// <param name="CLientIDToken"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static string CreateToken(string username, string CLientIDToken, long ticks)
        {
            try
            {
                string key = ConfigurationManager.AppSettings["keyValue"].ToString();
                string IV = ConfigurationManager.AppSettings["IVValue"].ToString();

                string hash = string.Join(":", new string[] { username, ticks.ToString() });
                string hashLeft = "";
                string hashRight = "";
                string encry = "";

                hashLeft = Convert.ToBase64String(EncryptionDecryptorTripleDES.Encryption(CLientIDToken, key, IV));
                encry = string.Join(":", new string[] { username, ticks.ToString() });
                hashRight = Convert.ToBase64String(EncryptionDecryptorTripleDES.Encryption(encry, key, IV));

                return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
