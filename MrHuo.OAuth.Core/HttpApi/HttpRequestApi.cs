using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MrHuo.OAuth
{
    public class HttpRequestApi
    {
        private const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";
        private static HttpClient CreateHttpClient()
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
                api = $"{api}{(api.Contains("?") ? "&" : "?")}{query.ToQueryString()}";
                var response = await httpClient.GetAsync(api);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<T> GetAsync<T>(string api, Dictionary<string, string> query = null, Dictionary<string, string> header = null)
        {
            var responseText = await GetStringAsync(api, query, header);
            Console.WriteLine($"GET [{api}], reponse=[{responseText}]");
            return JsonSerializer.Deserialize<T>(responseText);
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
                var response = await httpClient.PostAsync(api, new FormUrlEncodedContent(form));
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<T> PostAsync<T>(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            var responseText = await PostStringAsync(api, form, header);
            Console.WriteLine($"POST [{api}], reponse=[{responseText}]");
            return JsonSerializer.Deserialize<T>(responseText);
        }
    }
}
