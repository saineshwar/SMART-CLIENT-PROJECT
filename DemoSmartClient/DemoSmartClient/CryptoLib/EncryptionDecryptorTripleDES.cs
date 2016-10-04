using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DemoSmartClient.CryptoLib
{
    public static class EncryptionDecryptorTripleDES
    {
        public static byte[] Encryption(string PlainText, string key, string IV)
        {

            TripleDES des = CreateDES(key, IV);
            ICryptoTransform ct = des.CreateEncryptor();
            byte[] input = Encoding.Unicode.GetBytes(PlainText);
            return ct.TransformFinalBlock(input, 0, input.Length);
        }

        static TripleDES CreateDES(string key, string IV)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = Encoding.ASCII.GetBytes(IV);
            return des;
        }

        public static string Decryption(string CypherText, string key, string IV)
        {

            byte[] b = Convert.FromBase64String(CypherText);
            TripleDES des = CreateDES(key, IV);
            ICryptoTransform ct = des.CreateDecryptor();
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            return Encoding.Unicode.GetString(output);

        }
    }
}
