using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SmartWebApp.CryptoLib
{
    public static class EncryptionDecryptorTripleDES
    {
        public static byte[] Encryption(string PlainText, string key, string IV)
        {
            try
            {
                TripleDES des = CreateDES(key, IV);
                ICryptoTransform ct = des.CreateEncryptor();
                byte[] input = Encoding.Unicode.GetBytes(PlainText);
                return ct.TransformFinalBlock(input, 0, input.Length);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string Decryption(string CypherText, string key, string IV)
        {

            try
            {
                byte[] b = Convert.FromBase64String(CypherText);
                TripleDES des = CreateDES(key, IV);
                ICryptoTransform ct = des.CreateDecryptor();
                byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
                return Encoding.Unicode.GetString(output);
            }
            catch (Exception)
            {

                throw;
            }

        }

        static TripleDES CreateDES(string key, string IV)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                TripleDES des = new TripleDESCryptoServiceProvider();
                des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
                des.IV = Encoding.ASCII.GetBytes(IV);
                return des;
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}