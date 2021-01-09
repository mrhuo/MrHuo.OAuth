using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Alipay
{
    public class AlipayAccessTokenModel : DefaultAccessTokenModel
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}
