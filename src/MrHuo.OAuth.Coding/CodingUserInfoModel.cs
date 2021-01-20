using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Coding
{
    /// <summary>
    /// https://help.coding.net/docs/open/start.html#oauth-get-user
    /// </summary>
    public class CodingUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("team")]
        public string Team { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("name_pinyin")]
        public string NamePinYin { get; set; }

        [JsonPropertyName("global_key")]
        public string GlobalKey { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        public string ErrorMessage { get; set; }
    }
}
