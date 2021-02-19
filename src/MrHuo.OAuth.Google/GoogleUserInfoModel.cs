using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Google
{
    public class GoogleUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("picture")]
        public string Avatar { get; set; }
        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }
        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }

        public string ErrorMessage { get; set; }
    }
}
