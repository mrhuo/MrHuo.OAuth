using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace MrHuo.OAuth
{
    /// <summary>
    /// 通用的 API 请求类，封装了 GET/POST 请求，接口返回字符串
    /// </summary>
    public class API
    {
        private const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";
       
        /// <summary>
        /// 发起 GET 请求
        /// </summary>
        /// <param name="api"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string Get(string api, Dictionary<string, string> header = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (header == null)
                {
                    header = new Dictionary<string, string>();
                }
                if (!header.ContainsKey("accept"))
                {
                    header.Add("accept", "application/json");
                }
                if (!header.ContainsKey("User-Agent"))
                {
                    header.Add("User-Agent", DEFAULT_USER_AGENT);
                }
                foreach (var headerItem in header)
                {
                    httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                }
                var response = httpClient.GetAsync(api).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// 发起 POST 请求
        /// </summary>
        /// <param name="api"></param>
        /// <param name="form"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string Post(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (form == null)
                {
                    form = new Dictionary<string, string>();
                }
                if (header == null)
                {
                    header = new Dictionary<string, string>();
                }
                if (!header.ContainsKey("accept"))
                {
                    header.Add("accept", "application/json");
                }
                if (!header.ContainsKey("User-Agent"))
                {
                    header.Add("User-Agent", DEFAULT_USER_AGENT);
                }
                foreach (var headerItem in header)
                {
                    httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                }
                var response = httpClient.PostAsync(api, new FormUrlEncodedContent(form)).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
