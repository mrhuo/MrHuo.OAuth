using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.KuaiShou
{
    public class KuaiShouAccessTokenModel : DefaultAccessTokenModel
    {
        /// <summary>
        /// 错误码。非1 表示失败
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }

        [JsonPropertyName("open_id")]
        public string OpenId { get; set; }

        [JsonPropertyName("expires_in")]
        public new int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }

        [JsonPropertyName("scopes")]
        public List<string> Scopes { get; set; }

        [JsonPropertyName("error_msg")]
        public String ErrorMsg { get; set; }
    }
}
