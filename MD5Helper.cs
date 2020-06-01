using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLib
{
    public class MD5Helper
    {

        public static string GetStringMD5(string content)
        {
            byte[] ContentBytes = Encoding.UTF8.GetBytes(content);

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] HashBytes = md5.ComputeHash(ContentBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < HashBytes.Length; i++)
                {
                    sb.Append(HashBytes[i].ToString("x2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}
