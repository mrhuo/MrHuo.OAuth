using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Alipay
{
    public class AlipayAccessTokenModel : DefaultAccessTokenModel
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("error_response")]
        public string ErrorResponse { get; set; }

        [JsonPropertyName("sub_code")]
        public string SubCode { get; set; }

        [JsonPropertyName("sub_msg")]
        public string SubMsg { get; set; }
    }
}
