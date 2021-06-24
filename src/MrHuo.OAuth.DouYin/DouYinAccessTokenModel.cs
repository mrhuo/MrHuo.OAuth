using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.DouYin
{
    public class DouYinAccessTokenModel: DefaultAccessTokenModel
    {
        [JsonPropertyName("open_id")]
        public string OpenId { get; set; }
        
        [JsonPropertyName("expires_in")]
        public new int ExpiresIn { get; set; }
    }
}
