using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MrHuo.OAuth.LinkedIn
{
    /// <summary>
    /// 文档：
    /// https://docs.microsoft.com/zh-cn/linkedin/shared/authentication/authorization-code-flow?context=linkedin/context
    /// Scopes: r_liteprofile%20r_emailaddress%20w_member_social
    /// Scopes设置地址：https://www.linkedin.com/developers/apps
    /// </summary>
    public class LinkedInOAuth : OAuthLoginBase<LinkedInUserInfoModel>
    {
        public LinkedInOAuth(OAuthConfig config) : base(config) { }

        protected override string AuthorizeUrl => "https://www.linkedin.com/oauth/v2/authorization";
        protected override string AccessTokenUrl => "https://www.linkedin.com/oauth/v2/accessToken";
        protected override string UserInfoUrl => "https://api.linkedin.com/v2/me";
        private string UserPictureInfoUrl = "https://api.linkedin.com/v2/me?projection=(id,profilePicture(displayImage~digitalmediaAsset:playableStreams))";

        public override async Task<LinkedInUserInfoModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            var userInfoModelApiResponse = await HttpRequestApi.GetAsync<LinkedInUserInfoModelResponse>(
                UserInfoUrl,
                null,
                new Dictionary<string, string>
                {
                    ["Authorization"] = "Bearer " + accessTokenModel.AccessToken
                }
            );
            if (userInfoModelApiResponse.ErrorMessage != null)
            {
                throw new Exception(userInfoModelApiResponse.ErrorMessage);
            }
            var avatarUrl = await GetLinkedInAvatarUrl(accessTokenModel);
            var userInfoModel = new LinkedInUserInfoModel(userInfoModelApiResponse)
            {
                Avatar = avatarUrl
            };
            return userInfoModel;
        }

        private async Task<string> GetLinkedInAvatarUrl(DefaultAccessTokenModel accessTokenModel)
        {
            var pictureInfo = await HttpRequestApi.GetAsync<LinkedInPictureInfoModelResponse>(
                UserPictureInfoUrl,
                null,
                new Dictionary<string, string>
                {
                    ["Authorization"] = "Bearer " + accessTokenModel.AccessToken
                }
            );
            return pictureInfo?.ProfilePicture?.DisplayImage?.Elements?.FirstOrDefault()?.Identifiers?.FirstOrDefault()?.Identifier;
        }
    }
}
