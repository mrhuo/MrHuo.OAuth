using System.Text.Json.Serialization;

namespace MrHuo.OAuth.XunLei
{
    /// <summary>
    /// http://dev.open-api-auth.xunlei.com/platform?m=Developer&op=docPage&segment=openapi
    /// </summary>
    public class XunLeiUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("picture")]
        public string Avatar { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorMessage { get; set; }
    }
}
