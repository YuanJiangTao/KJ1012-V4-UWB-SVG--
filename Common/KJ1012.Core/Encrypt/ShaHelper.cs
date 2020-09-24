using System.Security.Cryptography;
using System.Text;

namespace KJ1012.Core.Encrypt
{
    public class ShaHelper
    {
        public static string Sha1(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(bytes);
            StringBuilder pwd = new StringBuilder();
            foreach (byte bStr in hash)
            {
                pwd.Append(bStr.ToString("x2"));
            }
            return pwd.ToString();
        }
        public static string Sha256(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            var sha1 = SHA256.Create();
            var hash = sha1.ComputeHash(bytes);
            StringBuilder pwd = new StringBuilder();
            foreach (byte bStr in hash)
            {
                pwd.Append(bStr.ToString("x2"));
            }
            return pwd.ToString();
        }
    }
}
