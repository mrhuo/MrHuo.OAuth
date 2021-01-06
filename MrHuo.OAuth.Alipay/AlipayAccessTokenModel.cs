using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Alipay
{
    public class AlipayAccessTokenModel : IAccessTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
