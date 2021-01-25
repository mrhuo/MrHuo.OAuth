using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MrHuo.OAuth
{
    /// <summary>
    /// HttpClient 实现的 API 请求工具类
    /// </summary>
    public class HttpRequestApi
    {
        /// <summary>
        /// 默认用户代理字符串
        /// </summary>
        public static string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";
        /// <summary>
        /// 日志开关
        /// </summary>
        public static bool EnableDebugLog = false;

        /// <summary>
        /// 内部记录日志
        /// </summary>
        /// <param name="msg"></param>
        private static void DebugLog(string msg)
        {
            if (EnableDebugLog)
            {
                Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// 创建 HttpClient
        /// </summary>
        /// <returns></returns>
        public static HttpClient CreateHttpClient()
        {
            return new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true
            });
        }

        /// <summary>
        /// 异步 GET 请求API，返回字符串
        /// </summary>
        /// <param name="api"></param>
        /// <param name="query"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string api, Dictionary<string, string> query = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                if (query == null)
                {
                    query = new Dictionary<string, string>();
                }
                query.RemoveEmptyValueItems();
                api = $"{api}{(api.Contains("?") ? "&" : "?")}{query.ToQueryString()}";
                DebugLog($"GET [{api}]");
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
                    DebugLog($"GET Header [{headerItem.Key}]=[{headerItem.Value}]");
                }
                var response = await httpClient.GetAsync(api);
                var responseText = await response.Content.ReadAsStringAsync();
                DebugLog($"GET [{api}], reponse=[{responseText}]");
                return responseText;
            }
        }

        /// <summary>
        /// 异步 GET 请求API，返回反序列化后的类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="query"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string api, Dictionary<string, string> query = null, Dictionary<string, string> header = null)
        {
            return JsonSerializer.Deserialize<T>(await GetStringAsync(api, query, header));
        }

        /// <summary>
        /// 异步 POST 请求API，返回字符串
        /// </summary>
        /// <param name="api"></param>
        /// <param name="form"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<string> PostStringAsync(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                if (form == null)
                {
                    form = new Dictionary<string, string>();
                }
                form.RemoveEmptyValueItems();
                DebugLog($"POST [{api}]");
                foreach (var item in form)
                {
                    DebugLog($"POST [{item.Key}]=[{item.Value}]");
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
                    DebugLog($"POST Header [{headerItem.Key}]=[{headerItem.Value}]");
                }
                var response = await httpClient.PostAsync(api, new FormUrlEncodedContent(form));
                var responseText = await response.Content.ReadAsStringAsync();
                DebugLog($"POST [{api}], reponse=[{responseText}]");
                return responseText;
            }
        }

        /// <summary>
        /// 异步 POST 请求API，并将 form 字段序列化为 json，放在请求体内。返回字符串
        /// </summary>
        /// <param name="api"></param>
        /// <param name="form"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<string> PostJsonBodyAsync(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                if (form == null)
                {
                    form = new Dictionary<string, string>();
                }
                form.RemoveEmptyValueItems();
                DebugLog($"POST [{api}]");
                var jsonBody = JsonSerializer.Serialize(form);
                DebugLog($"POST Body=[{jsonBody}]");
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
                    DebugLog($"POST Header [{headerItem.Key}]=[{headerItem.Value}]");
                }
                var response = await httpClient.PostAsync(api, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                var responseText = await response.Content.ReadAsStringAsync();
                DebugLog($"POST [{api}], reponse=[{responseText}]");
                return responseText;
            }
        }

        public static async Task<T> PostAsync<T>(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            return JsonSerializer.Deserialize<T>(await PostStringAsync(api, form, header));
        }

        public static async Task<T> PostJsonAsync<T>(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            return JsonSerializer.Deserialize<T>(await PostJsonBodyAsync(api, form, header));
        }
    }
}
