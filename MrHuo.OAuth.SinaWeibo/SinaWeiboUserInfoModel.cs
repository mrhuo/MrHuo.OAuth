using System.Text.Json.Serialization;

namespace MrHuo.OAuth.SinaWeibo
{
    /// <summary>
    /// https://open.weibo.com/wiki/2/users/show
    /// </summary>
    public class SinaWeiboUserInfoModel: IUserInfoModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar_large")]
        public string Avatar { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }
    }
}
