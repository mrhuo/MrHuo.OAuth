using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Github
{
    //{"access_token":"3474ad07aa1ccc2774e4bc0447408bd0b06e5d98","token_type":"bearer","scope":""}
    public class GithubAccessTokenModel: IAccessTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
