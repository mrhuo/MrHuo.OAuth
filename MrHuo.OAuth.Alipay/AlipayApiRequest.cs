using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MrHuo.OAuth.Alipay
{
    public class AlipayApiRequest
    {
        private const string API_URL = "https://openapi.alipay.com/gateway.do";
        public string AppId { get; set; }
        public string PrivateRSAKey { get; set; }
        public string PublicRSAKey { get; set; }

        private async Task<T> InvokeAsync<T>(string httpMethod, string apiMethod, Dictionary<string, string> param = null)
        {
            if (param == null)
            {
                param = new Dictionary<string, string>();
            }
            //param.Add("biz_content", JsonSerializer.Serialize(param));
            param.Add("app_id", AppId);
            param.Add("method", apiMethod);
            param.Add("charset", "utf-8");
            param.Add("sign_type", "RSA2");
            param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            param.Add("version", "1.0");
            param.Add("sign", AlipaySignTool.Sign(param, PrivateRSAKey));
            foreach (var item in param)
            {
                Console.WriteLine($"InvokeAsync： [{item.Key}]=[{item.Value}]");
            }
            var orignalQueryString = string.Join("&", param.Where(p => p.Key != "sign").Select(p => $"{p.Key}={p.Value}").ToArray());
            using (var httpClient = HttpRequestApi.CreateHttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Referer", "https://alipay.com");
                httpClient.DefaultRequestHeaders.Add("User-Agent", HttpRequestApi.DEFAULT_USER_AGENT);
                httpClient.DefaultRequestHeaders.Add("accept", "application/json");
                HttpResponseMessage response = null;
                if (httpMethod == "get")
                {
                    var queryString = string.Join("&", param.Select(p => $"{p.Key}={p.Value}").ToArray());
                    var api = $"{API_URL}?{queryString}";
                    response = await httpClient.GetAsync(api);
                }
                else if (httpMethod == "post")
                {
                    response = await httpClient.PostAsync(API_URL, new FormUrlEncodedContent(param));
                }
                var responseText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API: responseText=[{responseText}]");
                return JsonSerializer.Deserialize<T>(responseText);
            }
        }

        public Task<T> GetAsync<T>(string apiMethod, Dictionary<string, string> param = null)
        {
            return InvokeAsync<T>("get", apiMethod, param);
        }

        public Task<T> PostAsync<T>(string apiMethod, Dictionary<string, string> param = null)
        {
            return InvokeAsync<T>("post", apiMethod, param);
        }
    }

    internal class AlipayApiResponse
    {
        [JsonPropertyName("alipay_system_oauth_token_response")]
        public AlipayAccessTokenModel AccessTokenResponse { get; set; }

        [JsonPropertyName("alipay_user_info_share_response")]
        public AlipayUserInfoModel AlipayUserInfoModel { get; set; }
    }
}
