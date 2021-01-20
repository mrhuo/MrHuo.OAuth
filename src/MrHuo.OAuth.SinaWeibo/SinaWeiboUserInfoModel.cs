using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.SinaWeibo
{
    /// <summary>
    /// https://open.weibo.com/wiki/2/users/show
    /// </summary>
    public class SinaWeiboUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("idstr")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("avatar_large")]
        public string Avatar { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        private string gender = "";
        [JsonPropertyName("gender")]
        public string Gender
        {
            get
            {
                if (gender == "m")
                {
                    return "男";
                }
                if (gender == "f")
                {
                    return "女";
                }
                return "";
            }
            set
            {
                gender = value;
            }
        }


        [JsonPropertyName("followers_count")]
        public int FollowersCount { get; set; }

        [JsonPropertyName("friends_count")]
        public int FriendsCount { get; set; }

        [JsonPropertyName("created_at")]
        public string CreateAt { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }
    }
}
