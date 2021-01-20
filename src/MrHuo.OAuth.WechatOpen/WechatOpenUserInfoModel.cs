using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.WechatOpen
{
    public class WechatOpenUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("nickname")]
        public string Name { get; set; }

        [JsonPropertyName("headimgurl")]
        public string Avatar { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("openid")]
        public string Openid { get; set; }

        [JsonPropertyName("sex")]
        public int Sex { get; set; }

        [JsonPropertyName("province")]
        public string Province { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        [JsonPropertyName("privilege")]
        public List<string> Privilege { get; set; }

        [JsonPropertyName("unionid")]
        public string UnionId { get; set; }

        [JsonPropertyName("errmsg")]
        public string ErrorMessage { get; set; }
    }
}
