using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Huawei
{
    /// <summary>
    /// https://developer.huawei.com/consumer/cn/doc/30101
    /// https://developer.huawei.com/consumer/cn/doc/HMSCore-References-V5/account-obtain-token_hms_reference-0000001050048618-V5
    /// </summary>
    public class HuaweiAccessTokenModel : IAccessTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
