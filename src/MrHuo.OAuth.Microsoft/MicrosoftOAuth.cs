using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MrHuo.OAuth.Microsoft
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow
    /// </summary>
    public class MicrosoftOAuth : OAuthLoginBase<MicrosoftUserInfoModel>
    {
        public MicrosoftOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
        protected override string AccessTokenUrl => "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        protected override string UserInfoUrl => "https://graph.microsoft.com/v1.0/me";

        public override async Task<MicrosoftUserInfoModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            var userInfoModel = await HttpRequestApi.GetAsync<MicrosoftUserInfoModel>(
                UserInfoUrl,
                null,
                new Dictionary<string, string>
                {
                    ["Authorization"] = "Bearer " + accessTokenModel.AccessToken
                }
            );
            if (userInfoModel.Error != null)
            {
                throw new Exception(userInfoModel.Error.Message);
            }
            userInfoModel.Name = userInfoModel.Name?.Replace(" ", "");
            return userInfoModel;
        }

    }
}
