using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.LinkedIn
{
    public class LinkedInUserInfoModel : IUserInfoModel
    {
        internal LinkedInUserInfoModel(LinkedInUserInfoModelResponse response)
        {
            this.Name = response.LocalizedFirstName + response.LocalizedLastName;
            this.Id = response.Id;
            this.ErrorMessage = response.ErrorMessage;
        }

        public String Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// {"localizedLastName":"霍","profilePicture":{"displayImage":"urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q"},"firstName":{"localized":{"zh_CN":"小平"},"preferredLocale":{"country":"CN","language":"zh"}},"lastName":{"localized":{"zh_CN":"霍"},"preferredLocale":{"country":"CN","language":"zh"}},"id":"H18YYA6WpN","localizedFirstName":"小平"}
    /// </summary>
    class LinkedInUserInfoModelResponse
    {
        [JsonPropertyName("localizedLastName")]
        public String LocalizedLastName { get; set; }

        [JsonPropertyName("localizedFirstName")]
        public String LocalizedFirstName { get; set; }

        [JsonPropertyName("id")]
        public String Id { get; set; }

        [JsonPropertyName("profilePicture")]
        public LinkedInUserInfoModelProfilePicture ProfilePicture { get; set; }

        [JsonPropertyName("ErrorMessage")]
        public string ErrorMessage { get; set; }
    }

    class LinkedInUserInfoModelProfilePicture
    {
        [JsonPropertyName("displayImage")]
        public string DisplayImage { get; set; }
    }

    class Identifiers
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }
    }

    class Elements
    {
        [JsonPropertyName("identifiers")]
        public List<Identifiers> Identifiers { get; set; }
    }

    class DisplayImage
    {
        [JsonPropertyName("elements")]
        public List<Elements> Elements { get; set; }
    }

    class ProfilePicture
    {
        [JsonPropertyName("displayImage~")]
        public DisplayImage DisplayImage { get; set; }
    }

    /// <summary>
    /// {"profilePicture":{"displayImage":"urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q","displayImage~":{"paging":{"count":10,"start":0,"links":[]},"elements":[{"artifact":"urn:li:digitalmediaMediaArtifact:(urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q,urn:li:digitalmediaMediaArtifactClass:profile-displayphoto-shrink_100_100)","authorizationMethod":"PUBLIC","data":{"com.linkedin.digitalmedia.mediaartifact.StillImage":{"mediaType":"image/jpeg","rawCodecSpec":{"name":"jpeg","type":"image"},"displaySize":{"width":100.0,"uom":"PX","height":100.0},"storageSize":{"width":100,"height":100},"storageAspectRatio":{"widthAspect":1.0,"heightAspect":1.0,"formatted":"1.00:1.00"},"displayAspectRatio":{"widthAspect":1.0,"heightAspect":1.0,"formatted":"1.00:1.00"}}},"identifiers":[{"identifier":"https://media-exp1.licdn.com/dms/image/C5103AQHUjikgFLvT0Q/profile-displayphoto-shrink_100_100/0/1523258565148?e=1620259200&v=beta&t=uoLrxbV4RHSKWoN8s_0XVcDaM8Stoby-THO9zT_BuZw","index":0,"mediaType":"image/jpeg","file":"urn:li:digitalmediaFile:(urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q,urn:li:digitalmediaMediaArtifactClass:profile-displayphoto-shrink_100_100,0)","identifierType":"EXTERNAL_URL","identifierExpiresInSeconds":1620259200}]},{"artifact":"urn:li:digitalmediaMediaArtifact:(urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q,urn:li:digitalmediaMediaArtifactClass:profile-displayphoto-shrink_200_200)","authorizationMethod":"PUBLIC","data":{"com.linkedin.digitalmedia.mediaartifact.StillImage":{"mediaType":"image/jpeg","rawCodecSpec":{"name":"jpeg","type":"image"},"displaySize":{"width":200.0,"uom":"PX","height":200.0},"storageSize":{"width":200,"height":200},"storageAspectRatio":{"widthAspect":1.0,"heightAspect":1.0,"formatted":"1.00:1.00"},"displayAspectRatio":{"widthAspect":1.0,"heightAspect":1.0,"formatted":"1.00:1.00"}}},"identifiers":[{"identifier":"https://media-exp1.licdn.com/dms/image/C5103AQHUjikgFLvT0Q/profile-displayphoto-shrink_200_200/0/1523258565148?e=1620259200&v=beta&t=aaA23IX1Ptb2WUrkwNkxoKv2E6vWxgv0kDa-kTsOyII","index":0,"mediaType":"image/jpeg","file":"urn:li:digitalmediaFile:(urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q,urn:li:digitalmediaMediaArtifactClass:profile-displayphoto-shrink_200_200,0)","identifierType":"EXTERNAL_URL","identifierExpiresInSeconds":1620259200}]},{"artifact":"urn:li:digitalmediaMediaArtifact:(urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q,urn:li:digitalmediaMediaArtifactClass:profile-displayphoto-shrink_400_400)","authorizationMethod":"PUBLIC","data":{"com.linkedin.digitalmedia.mediaartifact.StillImage":{"mediaType":"image/jpeg","rawCodecSpec":{"name":"jpeg","type":"image"},"displaySize":{"width":400.0,"uom":"PX","height":400.0},"storageSize":{"width":400,"height":400},"storageAspectRatio":{"widthAspect":1.0,"heightAspect":1.0,"formatted":"1.00:1.00"},"displayAspectRatio":{"widthAspect":1.0,"heightAspect":1.0,"formatted":"1.00:1.00"}}},"identifiers":[{"identifier":"https://media-exp1.licdn.com/dms/image/C5103AQHUjikgFLvT0Q/profile-displayphoto-shrink_400_400/0/1523258565148?e=1620259200&v=beta&t=mEA7L_OQ5tVaHILIsAdDEa_oJS5epA91EXqcSujOUHY","index":0,"mediaType":"image/jpeg","file":"urn:li:digitalmediaFile:(urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q,urn:li:digitalmediaMediaArtifactClass:profile-displayphoto-shrink_400_400,0)","identifierType":"EXTERNAL_URL","identifierExpiresInSeconds":1620259200}]},{"artifact":"urn:li:digitalmediaMediaArtifact:(urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q,urn:li:digitalmediaMediaArtifactClass:profile-displayphoto-shrink_800_800)","authorizationMethod":"PUBLIC","data":{"com.linkedin.digitalmedia.mediaartifact.StillImage":{"mediaType":"image/jpeg","rawCodecSpec":{"name":"jpeg","type":"image"},"displaySize":{"width":800.0,"uom":"PX","height":800.0},"storageSize":{"width":800,"height":800},"storageAspectRatio":{"widthAspect":1.0,"heightAspect":1.0,"formatted":"1.00:1.00"},"displayAspectRatio":{"widthAspect":1.0,"heightAspect":1.0,"formatted":"1.00:1.00"}}},"identifiers":[{"identifier":"https://media-exp1.licdn.com/dms/image/C5103AQHUjikgFLvT0Q/profile-displayphoto-shrink_800_800/0/1523258565148?e=1620259200&v=beta&t=P2RvDA5Eio519bYhqu2g1by_3RTdPwoVJ7KT7V0w7GM","index":0,"mediaType":"image/jpeg","file":"urn:li:digitalmediaFile:(urn:li:digitalmediaAsset:C5103AQHUjikgFLvT0Q,urn:li:digitalmediaMediaArtifactClass:profile-displayphoto-shrink_800_800,0)","identifierType":"EXTERNAL_URL","identifierExpiresInSeconds":1620259200}]}]}},"id":"H18YYA6WpN"}
    /// </summary>
    class LinkedInPictureInfoModelResponse
    {
        [JsonPropertyName("profilePicture")]
        public ProfilePicture ProfilePicture { get; set; }
    }

}
