
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace ETModel
{
    public static class MD5Tool
    {
        public static string GetMD5(string src)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(src);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            var retStr = BitConverter.ToString(md5data);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
        private static string CreateMd5Sign(Hashtable parameters)
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort();//排序，这是微信要求的
            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                sb.Append(k + "=" + v + "&");
            }
            sb.Append("key=" + "key");
            string sign = GetMD5(sb.ToString());
            return sign;
        }
    }
}
