using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Wechat
{
    public class WechatAccessTokenModel : DefaultAccessTokenModel
    {
        [JsonPropertyName("openid")]
        public string OpenId { get; set; }
    }
}
