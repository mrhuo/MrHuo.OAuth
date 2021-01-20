using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MrHuo.OAuth.MeiTuan
{
    /// <summary>
    /// http://open.waimai.meituan.com/openapi_docs/oauth/#oauth
    /// </summary>
    public class MeiTuanOAuth : OAuthLoginBase<MeiTuanUserInfoModel>
    {
        public MeiTuanOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }
        protected override string AuthorizeUrl => "https://openapi.waimai.meituan.com/oauth/authorize";
        protected override string AccessTokenUrl => "https://openapi.waimai.meituan.com/oauth/access_token";
        protected override string UserInfoUrl => "https://openapi.waimai.meituan.com/oauth/userinfo";

        protected override Dictionary<string, string> BuildAuthorizeParams(string state)
        {
            return new Dictionary<string, string>
            {
                ["response_type"] = "code",
                ["app_id"] = (oauthConfig.AppId ?? ""),
                ["redirect_uri"] = (oauthConfig.RedirectUri ?? ""),
                ["scope"] = (oauthConfig.Scope ?? ""),
                ["state"] = (state ?? "")
            };
        }

        protected override Dictionary<string, string> BuildGetAccessTokenParams(Dictionary<string, string> authorizeCallbackParams)
        {
            return new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = (authorizeCallbackParams["code"] ?? ""),
                ["app_id"] = (oauthConfig.AppId ?? ""),
                ["secret"] = (oauthConfig.AppKey ?? ""),
            };
        }

        protected override Dictionary<string, string> BuildGetUserInfoParams(DefaultAccessTokenModel accessTokenModel)
        {
            return new Dictionary<string, string>
            {
                ["access_token"] = accessTokenModel.AccessToken,
                ["app_id"] = (oauthConfig.AppId ?? ""),
                ["secret"] = (oauthConfig.AppKey ?? ""),
            };
        }

        /// <summary>
        /// 美团获取用户信息接口，用的是 HTTP POST 方式
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        public override async Task<MeiTuanUserInfoModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            var userInfoModel = await HttpRequestApi.PostAsync<MeiTuanUserInfoModel>(
                UserInfoUrl,
                BuildGetUserInfoParams(accessTokenModel)
            );
            if (userInfoModel.HasError())
            {
                throw new Exception(userInfoModel.ErrorMessage);
            }
            return userInfoModel;
        }
    }
}
