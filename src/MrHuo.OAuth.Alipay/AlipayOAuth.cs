using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MrHuo.OAuth.Alipay
{
    /// <summary>
    /// https://opendocs.alipay.com/open/284/106001
    /// 支付宝回调URL：
    /// https://oauthlogin.net/oauth/alipaycallback?app_id=2021002122645005&source=alipay_wallet&userOutputs=auth_user&scope=auth_user&alipay_token=&auth_code=2c58e763fdca4fb6b1f5a5bf4d26WA05
    /// https://github.com/alipay/alipay-easysdk/tree/master/csharp
    /// </summary>
    public class AlipayOAuth : OAuthLoginBase<AlipayAccessTokenModel, AlipayUserInfoModel>
    {
        private readonly AlipayApiRequest alipayApiRequest;

        public AlipayOAuth(OAuthConfig oauthConfig, string privateRSAKey, string publicRSAKey, string encryptKey) : base(oauthConfig)
        {
            alipayApiRequest = new AlipayApiRequest()
            {
                PrivateRSAKey = privateRSAKey,
                PublicRSAKey = publicRSAKey,
                AppId = oauthConfig.AppId
            };
        }

        protected override string AuthorizeUrl => "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm";
        protected override string AccessTokenUrl => throw new NotImplementedException();
        protected override string UserInfoUrl => throw new NotImplementedException();

        protected override Dictionary<string, string> BuildAuthorizeParams(string state)
        {
            return new Dictionary<string, string>()
            {
                ["response_type"] = "code",
                ["app_id"] = $"{oauthConfig.AppId}",
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
                ["code"] = authorizeCallbackParams["code"]
            };
        }

        protected override Dictionary<string, string> BuildGetUserInfoParams(AlipayAccessTokenModel accessTokenModel)
        {
            return new Dictionary<string, string>()
            {
                ["auth_token"] = accessTokenModel.AccessToken
            };
        }

        public override async Task<AlipayAccessTokenModel> GetAccessTokenAsync(Dictionary<string, string> authorizeCallbackParams)
        {
            var getAccessTokenResponse = await alipayApiRequest.PostAsync<AlipayApiResponse>(
                "alipay.system.oauth.token", 
                BuildGetAccessTokenParams(authorizeCallbackParams)
            );
            if (getAccessTokenResponse.AccessTokenResponse.SubMsg != null)
            {
                throw new Exception(getAccessTokenResponse.AccessTokenResponse.SubMsg);
            }
            return getAccessTokenResponse.AccessTokenResponse;
        }

        public override async Task<AlipayUserInfoModel> GetUserInfoAsync(AlipayAccessTokenModel accessTokenModel)
        {
            var getUserInfoResponse = await alipayApiRequest.PostAsync<AlipayApiResponse>(
                "alipay.user.info.share",
                BuildGetUserInfoParams(accessTokenModel)
            );
            if (getUserInfoResponse.AlipayUserInfoModel.SubMsg != null)
            {
                throw new Exception(getUserInfoResponse.AlipayUserInfoModel.SubMsg);
            }
            return getUserInfoResponse.AlipayUserInfoModel;
        }
    }
}
