using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MrHuo.OAuth
{
    public class HttpRequestApi
    {
        public const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";
        private static void DebugLog(string msg)
        {
            Console.WriteLine(msg);
        }

        public static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true
            };
            return new HttpClient(handler);
        }

        public static async Task<string> GetStringAsync(string api, Dictionary<string, string> query = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                if (query == null)
                {
                    query = new Dictionary<string, string>();
                }
                query.RemoveEmptyValueItems();
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
                api = $"{api}{(api.Contains("?") ? "&" : "?")}{query.ToQueryString()}";
                var response = await httpClient.GetAsync(api);
                var responseText = await response.Content.ReadAsStringAsync();
                DebugLog($"GET [{api}], reponse=[{responseText}]");
                return responseText;
            }
        }

        public static async Task<T> GetAsync<T>(string api, Dictionary<string, string> query = null, Dictionary<string, string> header = null)
        {
            return JsonSerializer.Deserialize<T>(await GetStringAsync(api, query, header));
        }

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

        public static async Task<T> PostAsync<T>(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            return JsonSerializer.Deserialize<T>(await PostStringAsync(api, form, header));
        }
    }
}
