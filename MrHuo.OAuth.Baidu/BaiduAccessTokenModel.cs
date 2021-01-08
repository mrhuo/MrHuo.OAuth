using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Baidu
{
    /// <summary>
    /// http://developer.baidu.com/wiki/index.php?title=docs/oauth/authorization
    /// </summary>
    public class BaiduAccessTokenModel : DefaultAccessTokenModel
    {
        /// <summary>
        /// 基于http调用Open API时所需要的Session Key，其有效期与Access Token一致；
        /// </summary>
        [JsonPropertyName("session_key")]
        public string SessionKey { get; set; }

        /// <summary>
        /// 基于http调用Open API时计算参数签名用的签名密钥。
        /// </summary>
        [JsonPropertyName("session_secret")]
        public string SessionSecret { get; set; }
    }
}
