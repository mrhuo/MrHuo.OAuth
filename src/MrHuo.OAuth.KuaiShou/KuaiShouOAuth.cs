using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MrHuo.OAuth.KuaiShou
{
    /// <summary>
    /// https://open.kuaishou.com/platform/openApi?menu=13
    /// </summary>
    public class KuaiShouOAuth : OAuthLoginBase<KuaiShouAccessTokenModel, KuaiShouUserInfoModel>
    {
        public KuaiShouOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://open.kuaishou.com/oauth2/authorize";
        protected override string AccessTokenUrl => "https://open.kuaishou.com/oauth2/access_token";
        protected override string UserInfoUrl => "https://open.kuaishou.com/openapi/user_info";

        protected override Dictionary<string, string> BuildAuthorizeParams(string state)
        {
            return new Dictionary<string, string>()
            {
                ["app_id"] = $"{oauthConfig.AppId}",
                ["scope"] = $"{oauthConfig.Scope}",
                ["response_type"] = "code",
                ["ua"] = "pc",
                ["redirect_uri"] = $"{oauthConfig.RedirectUri}",
                ["state"] = $"{state}"
            };
        }

        protected override Dictionary<string, string> BuildGetAccessTokenParams(Dictionary<string, string> authorizeCallbackParams)
        {
            return new Dictionary<string, string>()
            {
                ["app_id"] = $"{oauthConfig.AppId}",
                ["app_secret"] = $"{oauthConfig.AppKey}",
                ["code"] = $"{authorizeCallbackParams["code"]}",
                ["grant_type"] = "authorization_code",
            };
        }

        public override async Task<KuaiShouAccessTokenModel> GetAccessTokenAsync(Dictionary<string, string> authorizeCallbackParams)
        {
            var accessTokenModel = (await HttpRequestApi.GetAsync<KuaiShouAccessTokenModel>(
                AccessTokenUrl,
                BuildGetAccessTokenParams(authorizeCallbackParams)
            ));
            if (accessTokenModel.HasError())
            {
                throw new Exception(accessTokenModel.ErrorDescription);
            }
            return accessTokenModel;
        }

        protected override Dictionary<string, string> BuildGetUserInfoParams(KuaiShouAccessTokenModel accessTokenModel)
        {
            return new Dictionary<string, string>()
            {
                ["app_id"] = $"{oauthConfig.AppId}",
                ["access_token"] = accessTokenModel.AccessToken
            };
        }

        public override async Task<KuaiShouUserInfoModel> GetUserInfoAsync(KuaiShouAccessTokenModel accessTokenModel)
        {
            var userInfoModel = (await HttpRequestApi.GetAsync<KuaiShouApiUserInfoResponse<KuaiShouUserInfoModel>>(
                UserInfoUrl,
                BuildGetUserInfoParams(accessTokenModel)
            )).Data;

            if (userInfoModel.HasError())
            {
                throw new Exception(userInfoModel.ErrorMessage);
            }
            userInfoModel.OpenId = accessTokenModel.OpenId;
            return userInfoModel;
        }
    }
}
