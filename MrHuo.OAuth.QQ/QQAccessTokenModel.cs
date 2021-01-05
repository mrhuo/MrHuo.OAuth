using System.Text.Json.Serialization;

namespace MrHuo.OAuth.QQ
{
    public class QQAccessTokenModel : IAccessTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
