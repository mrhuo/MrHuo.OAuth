using System.Text.Json.Serialization;

namespace MrHuo.OAuth.QQ
{
    public class QQOpenIdModel
    {
        [JsonPropertyName("openid")]
        public string OpenId { get; set; }
    }
}
