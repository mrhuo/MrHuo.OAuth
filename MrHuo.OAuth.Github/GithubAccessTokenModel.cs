using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Github
{
    /// <summary>
    /// {"access_token":"xxxx","token_type":"bearer","scope":""}
    /// </summary>
    public class GithubAccessTokenModel : IAccessTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
