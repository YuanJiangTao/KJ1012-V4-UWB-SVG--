using KJ1012.Core.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KJ1012.Core.Helper
{
    public class HttpPostHelper
    {
        public static async Task<(bool Success, string Message)> HttpPost(string postUrl, string param)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, error) => true;
                //执行登录验证
                HttpClient httpClient = new HttpClient();
                //设置Http的正文            
                HttpContent httpContent = new StringContent(param);
                //设置Http的内容标头            
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
                {
                    //设置Http的内容标头的字符            
                    CharSet = "utf-8"
                };

                var response = await httpClient.PostAsync(postUrl, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var responseResult = JsonConvert.DeserializeObject<ResponseResult>(result);
                    var isSuccess = responseResult.Status == "Success";
                    return (isSuccess, responseResult.Message);
                }
                else
                {
                    return (false, $"请求错误码：{response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public static async Task<(bool Success, string Message)> HttpFiles(
            string postUrl, List<string> photoUrls, string param,
            string filePath, bool isSendEnd, bool isDeleteOld = false)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, error) => true;
                //执行登录验证
                HttpClient httpClient = new HttpClient();
                var httpContent = new MultipartFormDataContent();
                //添加字符串参数，参数名为qq
                httpContent.Add(new StringContent(param), "data");
                //数据发送完成
                httpContent.Add(new StringContent(isSendEnd.ToString()), "isSendEnd");
                //是否删除旧数据
                httpContent.Add(new StringContent(isDeleteOld.ToString()), "isDeleteOld");
                foreach (var item in photoUrls)
                {
                    var tempFilePath = filePath + item;
                    //添加文件参数，参数名为files，文件名为123.png
                    httpContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(tempFilePath)), "files", item);
                }
                var response = await httpClient.PostAsync(postUrl, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var responseResult = JsonConvert.DeserializeObject<ResponseResult>(result);
                    var isSuccess = responseResult.Status == "Success";
                    return (isSuccess, responseResult.Message);
                }
                else
                {
                    return (false, $"请求错误码：{response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public static async Task<(bool Success, string Message)> HttpPublishMemberSingle(
            string postUrl, string photoUrl, string param, string photoName, bool isAddCache)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, error) => true;
                //执行登录验证
                HttpClient httpClient = new HttpClient();
                var httpContent = new MultipartFormDataContent();
                //添加字符串参数，参数名为qq
                httpContent.Add(new StringContent(param), "data");
                httpContent.Add(new StringContent(isAddCache.ToString()), "isAddCache");
                //添加文件参数，参数名为files，文件名为123.png
                if (File.Exists(photoUrl))
                {
                    httpContent.Add(new ByteArrayContent(File.ReadAllBytes(photoUrl)), "files", photoName);
                }
                var response = await httpClient.PostAsync(postUrl, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var responseResult = JsonConvert.DeserializeObject<ResponseResult>(result);
                    var isSuccess = responseResult.Status == "Success";
                    return (isSuccess, responseResult.Message);
                }
                else
                {
                    return (false, $"请求错误码：{response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
