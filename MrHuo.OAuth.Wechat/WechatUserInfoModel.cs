using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Wechat
{
    public class WechatUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar_url")]
        public string Avatar { get; set; }
    }
}
