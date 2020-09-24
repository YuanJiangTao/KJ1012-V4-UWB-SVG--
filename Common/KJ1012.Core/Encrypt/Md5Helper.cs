using System.Security.Cryptography;
using System.Text;

namespace KJ1012.Core.Encrypt
{
    public class Md5Helper
    {
        /// <summary>
        /// MD5 - 32加密
        /// </summary>
        /// <param name="source">待加密字段</param>
        /// <returns></returns>
        public static string Md5(string source)
        {
            MD5 md5 = MD5.Create();
            byte[] btStr = Encoding.UTF8.GetBytes(source);
            byte[] hashStr = md5.ComputeHash(btStr);
            StringBuilder pwd = new StringBuilder();
            foreach (byte bStr in hashStr) { pwd.Append(bStr.ToString("x2")); }
            return pwd.ToString();
        }

        /// <summary>
        /// 加盐MD5 -32 加密
        /// </summary>
        /// <param name="source">待加密字段</param>
        /// <param name="salt">盐巴字段</param>
        /// <returns></returns>
        public static string Md5Salt(string source, string salt)
        {
            return string.IsNullOrEmpty(salt) ? Md5(source) : Md5(source + "『" + salt + "』");
        }
    }
}
