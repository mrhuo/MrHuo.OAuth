using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using static MrHuo.OAuth.Facebook.FacebookUserModel;

namespace MrHuo.OAuth.Facebook
{

    public class FacebookUserModel : MrHuo.OAuth.IUserInfoModel
    {


        public class PictureInfo
        {
            public class PictureData
            {
                public int height { get; set; }
                public bool is_silhouette { get; set; }
                public string url { get; set; }
                public int width { get; set; }
            }
            public PictureData data { get; set; }
        }



        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }


        [JsonPropertyName("message")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("picture")]
        [JsonConverter(typeof(FacebookPictureConveter))]
        public PictureInfo Picture
        {
            get; set;
        }

        public string Avatar
        {
            get
            {
                if (Picture != null)
                {
                    return Picture.data.url;
                }
                else
                {
                    return "";
                }
            }

            set
            {
                if (Picture != null)
                {
                    Picture.data.url = value;
                }
            }
        }


    }
}
