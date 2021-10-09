using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Corsac_Ticketing_System.Utilities
{
    public class Utilities
    {
        protected static MD5 _md5;

        public Utilities()
        {
            _md5 = MD5.Create();
        }

        public string GetMD5Hash(string input)
        {
            
            byte[] data = _md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            StringBuilder sBuilder = new StringBuilder();
            
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
             
            return sBuilder.ToString();
        }

        public bool VerifyMD5Hash(string input, string hash)
        {
             
            string hashOfInput = GetMD5Hash(input);
            
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return (0 == comparer.Compare(hashOfInput, hash));
            
        }

        public string GenerateReferenceId()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[10];
            var random = new Random();
            
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

           return new String(stringChars);
        }
    }
}