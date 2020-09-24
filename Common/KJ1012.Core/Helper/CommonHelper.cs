using Microsoft.AspNetCore.Http;
using KJ1012.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace KJ1012.Core.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// 创建时间截
        /// </summary>
        /// <param name="isUseMilliseconds">是否精确到毫秒</param>
        /// <returns></returns>
        public static string CreateTimeStamp(bool isUseMilliseconds = false)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            if (isUseMilliseconds)
            {
                return Convert.ToInt64(ts.TotalMilliseconds).ToString();
            }
            return Convert.ToInt32(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 创建时间截
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="isUseMilliseconds">是否精确到毫秒</param>
        /// <returns></returns>
        public static double GetTimeStamp(DateTime dateTime, bool isUseMilliseconds = false)
        {
            TimeSpan ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            if (isUseMilliseconds)
            {
                return ts.TotalMilliseconds;
            }
            return ts.TotalSeconds;
        }
        /// <summary>
        /// 分钟时间戳转换为时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToUtcDateTime(double timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return start.AddSeconds(timeStamp).AddHours(8);
        }
        /// <summary>
        /// 分钟时间戳转换为时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(double timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return start.AddSeconds(timeStamp);
        }
        /// <summary>
        /// 毫秒时间戳转换为时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime MillisecondsConvertToDateTime(double timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return start.AddMilliseconds(timeStamp);
        }
        /// <summary>
        /// 创建GUID
        /// </summary>
        /// <returns></returns>
        public static string CreateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 虚拟路径映射到物理路径
        /// </summary>
        /// <param name="path">需要映射的路径. E.g. "~/bin"</param>
        /// <returns>映射后的物理路径. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(string path)
        {
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(BaseDirectory ?? string.Empty, path);
        }
        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes">待转换字节数组</param> 
        /// <returns></returns> 
        public static string BytesToHexStr(params byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                foreach (byte t in bytes)
                {
                    returnStr += t.ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 十六进制字符串转换为字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string hexString)
        {
            if (hexString == null || hexString.Equals(""))
            {
                return null;
            }
            int length = hexString.Length / 2;
            if (hexString.Length % 2 != 0)
            {
                return null;
            }
            byte[] d = new byte[length];
            for (int i = 0; i < length; i++)
            {
                d[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return d;
        }
        /// <summary>
        /// 字节转换为GB2312字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToGb231(byte[] bytes)
        {
            return Encoding.GetEncoding("gb2312").GetString(bytes);
        }
        /// <summary>
        /// bg2313 字符串转换为字节数组
        /// </summary>
        /// <param name="bg2312"></param>
        /// <returns></returns>
        public static byte[] Gb2312StringToBytes(string bg2312)
        {
            return Encoding.GetEncoding("gb2312").GetBytes(bg2312);
        }

        /// <summary>
        /// 字节转整数
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BytesToInt(params byte[] bytes)
        {
            string hexString = BytesToHexStr(bytes);
            return HexParseInt(hexString);
        }
        /// <summary>
        /// 设置或获取应用程序路径
        /// </summary>
        internal static string BaseDirectory { get; set; }
        /// <summary>
        ///  Depth-first recursive delete, with handling for descendant directories open in Windows Explorer.
        /// </summary>
        /// <param name="path">Directory path</param>
        public static void DeleteDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(path);


            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }
        /// <summary>
        /// 十六进制字符串转换为十进制数据
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static int HexParseInt(string strHex)
        {
            int.TryParse(strHex, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out int result);
            return result;
        }
        /// <summary>
        /// 十六进制字符串转换为十进制数据
        /// </summary>
        /// <param name="strHexs"></param>
        /// <returns></returns>
        public static int HexParseInt(params string[] strHexs)
        {
            string strParse = string.Concat(strHexs);
            int.TryParse(strParse, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out int result);
            return result;
        }

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

        public static int MakeCheckUserId(int userId)
        {
            return 255 - userId;
        }
        /// <summary>
        /// int 转 二进制字符串
        /// </summary>
        /// <param name="value">需要转换的整数</param>
        /// <returns></returns>
        public static string IntToBit(int value)
        {
            return Convert.ToString(value, 2);
        }

        public static string StateToString(int state, Dictionary<string, string> dictionary)
        {
            if (state < 1)
            {
                if (dictionary.ContainsKey(state.ToString()))
                {
                    return dictionary[state.ToString()];
                }

                return string.Empty;
            }
            var bitString = IntToBit(state).Reverse().ToArray();
            var list = new List<string>();
            for (int i = 0; i < bitString.Length; i++)
            {
                if (bitString[i] == '1' && dictionary.ContainsKey((i + 1).ToString()))
                {
                    list.Add(dictionary[(i + 1).ToString()]);
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendJoin(';', list);
            return sb.ToString();
        }
        //public static byte MakeCheckByte(int userId)
        //{
        //    var value = 255 - userId;
        //    return (byte)value;
        //}
        //public static int MakeCheckInt(int userId)
        //{
        //    return 255 - userId;
        //}
        #region 时间格式处理

        public static string MinutesToHours(string minutes)
        {
            int.TryParse(minutes, out int totalMinutes);
            return MinutesToHours(totalMinutes);
        }
        public static string MinutesToHours(int minutes)
        {
            return $"{minutes / 60}小时{minutes % 60}分钟";
            //long.TryParse(minutes, out long totalMinutes);
            //TimeSpan timeSpan= TimeSpan.FromMinutes(totalMinutes);
            //return $"{timeSpan.Hours}小时{timeSpan.Minutes}分钟";
        }
        public static string SecondsToHours(int seconds)
        {
            return $"{seconds / 3600}时{seconds % 3600 / 60}分{seconds % 60}秒";
        }
        public static string MinutesToHours(DateTime? firstDateTime, DateTime? secondDateTime)
        {
            if (!firstDateTime.HasValue)
                return MinutesToHours(0);
            if (secondDateTime.HasValue)
            {
                return MinutesToHours((secondDateTime.Value - firstDateTime.Value).TotalMinutes.ToString("####"));
            }

            return MinutesToHours((DateTime.Now - firstDateTime.Value).TotalMinutes.ToString("####"));
        }
        #endregion

        #region 文件保存

        public static async Task<string> SaveFileAsync(IFormFile file, string savePath, string fileName = "")
        {
            if (file == null) throw new CustomException("文件不能为空");
            if (string.IsNullOrEmpty(savePath)) throw new CustomException("保存路径不能为空");
            string oldFileName = file.FileName;
            if (!(savePath.EndsWith("\\") || savePath.EndsWith("/")))
            {
                savePath = savePath + "\\";
            }
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Guid.NewGuid() + "." + oldFileName.Split(new[] { '.' }).Last();
            }
            else
            {
                if (!fileName.Any(r => r.Equals('.')))
                {
                    fileName = fileName + "." + oldFileName.Split(new[] { '.' }).Last();
                }
            }
            string fileFullName = savePath + fileName;
            using (FileStream fs = File.Create(fileFullName))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }
            return string.Concat(savePath, fileName);
        }


        #endregion

        public static string ReadJson(string filepath, Encoding encoding)
        {
            if (File.Exists(filepath))
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs, encoding))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath, string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            filePath = string.Concat(filePath, fileName);
            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }
                File.Delete(filePath);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        /// <summary>
        /// 图片上传 Base64解码
        /// </summary>
        /// <param name="dataURL">Base64数据</param>
        /// <param name="imgName">图片名字</param>
        /// <returns>返回一个相对路径</returns>
        public static bool DecodeBase64ToImage(string dataURL, string filePath)
        {
            string base64 = dataURL.Substring(dataURL.IndexOf(",") + 1);//将‘，’以前的多余字符串删除
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                byte[] arr = Convert.FromBase64String(base64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bitmap = new Bitmap(ms);
                bitmap.Save(filePath, ImageFormat.Jpeg);//保存到服务器路径
                ms.Close();//关闭当前流，并释放所有与之关联的资源
                bitmap.Dispose();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
