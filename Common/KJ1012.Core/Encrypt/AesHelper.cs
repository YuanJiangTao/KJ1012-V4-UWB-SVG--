using System.Security.Cryptography;
using System.Text;
using KJ1012.Core.Helper;

namespace KJ1012.Core.Encrypt
{
    public class AesHelper
    {


        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(string content, string key)
        {
            var encryptStr = Encoding.UTF8.GetBytes(content);
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                using (var encryptor = aesAlg.CreateEncryptor())
                {
                    var bytes = encryptor.TransformFinalBlock(encryptStr, 0, encryptStr.Length);
                    return CommonHelper.BytesToHexStr(bytes);
                }
            }
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string content, string key)
        {
            var encryptKey = CommonHelper.HexStringToBytes(content);
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                using (var decryptor = aesAlg.CreateDecryptor())
                {
                    var bytes = decryptor.TransformFinalBlock(encryptKey, 0, encryptKey.Length);
                    return Encoding.UTF8.GetString(bytes);
                }
            }
        }
    }
}
