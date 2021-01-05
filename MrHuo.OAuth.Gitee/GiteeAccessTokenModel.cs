using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Gitee
{
    public class GiteeAccessTokenModel : IAccessTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
