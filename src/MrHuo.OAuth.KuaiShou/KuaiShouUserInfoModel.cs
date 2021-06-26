using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.KuaiShou
{
    public class KuaiShouUserInfoModel : IUserInfoModel
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("head")]
        public string Avatar { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// 性别 "M：男性，“F”:女性，其他：未知
        /// </summary>
        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        /// <summary>
        /// 大头像地址(可能为空)
        /// </summary>
        [JsonPropertyName("bigHead")]
        public string BigHead { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        [JsonPropertyName("fan")]
        public int Fan { get; set; }

        /// <summary>
        /// 关注数
        /// </summary>
        [JsonPropertyName("follow")]
        public int Follow { get; set; }

        [JsonPropertyName("error_msg")]
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public string OpenId { get; set; }

    }
}
