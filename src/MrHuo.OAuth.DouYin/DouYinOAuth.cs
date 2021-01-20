using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MrHuo.OAuth.DouYin
{
    /// <summary>
    /// https://open.douyin.com/platform/doc/6848834666171009035
    /// </summary>
    public class DouYinOAuth : OAuthLoginBase<DouYinAccessTokenModel, DouYinUserInfoModel>
    {
        public DouYinOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://open.douyin.com/platform/oauth/connect/";
        protected override string AccessTokenUrl => "https://open.douyin.com/oauth/access_token/";
        protected override string UserInfoUrl => "https://open.douyin.com/oauth/userinfo/";

        protected override Dictionary<string, string> BuildAuthorizeParams(string state)
        {
            return new Dictionary<string, string>()
            {
                ["response_type"] = "code",
                ["client_key"] = $"{oauthConfig.AppId}",
                ["redirect_uri"] = $"{oauthConfig.RedirectUri}",
                ["scope"] = $"{oauthConfig.Scope}",
                ["state"] = $"{state}"
            };
        }

        protected override Dictionary<string, string> BuildGetAccessTokenParams(Dictionary<string, string> authorizeCallbackParams)
        {
            return new Dictionary<string, string>()
            {
                ["grant_type"] = "authorization_code",
                ["code"] = $"{authorizeCallbackParams["code"]}",
                ["client_key"] = $"{oauthConfig.AppId}",
                ["client_secret"] = $"{oauthConfig.AppKey}"
            };
        }

        public override async Task<DouYinAccessTokenModel> GetAccessTokenAsync(Dictionary<string, string> authorizeCallbackParams)
        {
            var accessTokenModel = (await HttpRequestApi.PostAsync<DouYinApiResponse<DouYinAccessTokenModel>>(
                AccessTokenUrl,
                BuildGetAccessTokenParams(authorizeCallbackParams)
            )).Data;
            if (accessTokenModel.HasError())
            {
                throw new Exception(accessTokenModel.ErrorDescription);
            }
            return accessTokenModel;
        }

        public override async Task<DouYinUserInfoModel> GetUserInfoAsync(DouYinAccessTokenModel accessTokenModel)
        {
            var userInfoModel = (await HttpRequestApi.GetAsync<DouYinApiResponse<DouYinUserInfoModel>>(
                UserInfoUrl,
                BuildGetUserInfoParams(accessTokenModel)
            )).Data;
            if (userInfoModel.HasError())
            {
                throw new Exception(userInfoModel.ErrorMessage);
            }
            return userInfoModel;
        }
    }
}
