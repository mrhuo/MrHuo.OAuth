using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.DouYin
{
    public class DouYinUserInfoModel: IUserInfoModel
    {
        [JsonPropertyName("nickname")]
        public string Name { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("province")]
        public string Province { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// 类型: * `EAccountM` - 普通企业号 * `EAccountS` - 认证企业号 * `EAccountK` - 品牌企业号
        /// </summary>
        [JsonPropertyName("e_account_role")]
        public string EAccountRole { get; set; }

        [JsonPropertyName("gender")]
        public int gender { get; set; }

        [JsonPropertyName("open_id")]
        public string OpenId { get; set; }

        [JsonPropertyName("union_id")]
        public string UnionId { get; set; }

        [JsonPropertyName("description")]
        public string ErrorMessage { get; set; }
    }

}
