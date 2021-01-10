using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.OSChina
{
    public class OSChinaUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorMessage { get; set; }
    }
}
