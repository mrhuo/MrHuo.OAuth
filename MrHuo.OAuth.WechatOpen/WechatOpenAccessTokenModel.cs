using System.Text.Json.Serialization;

namespace MrHuo.OAuth.WechatOpen
{
    public class WechatOpenAccessTokenModel : DefaultAccessTokenModel
    {
        [JsonPropertyName("openid")]
        public string OpenId { get; set; }
    }
}
