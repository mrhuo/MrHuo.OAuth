using System.Text.Json.Serialization;

namespace MrHuo.OAuth.MeiTuan
{
    /// <summary>
    /// http://open.waimai.meituan.com/openapi_docs/oauth/#_7
    /// </summary>
    public class MeiTuanUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("nickname")]
        public string Name { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("openid")]
        public string OpenId { get; set; }

        /// <summary>
        /// 脱敏手机号
        /// </summary>
        [JsonPropertyName("desensitization_phone")]
        public string DesensitizationPhone { get; set; }

        [JsonPropertyName("erroe_msg")]
        public string ErrorMessage { get; set; }
    }
}
