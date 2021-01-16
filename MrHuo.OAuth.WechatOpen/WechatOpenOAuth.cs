using System;
using System.Collections.Generic;
using System.Text;

namespace MrHuo.OAuth.WechatOpen
{
    /// <summary>
    /// https://developers.weixin.qq.com/doc/oplatform/Website_App/WeChat_Login/Wechat_Login.html
    /// </summary>
    public class WechatOpenOAuth: OAuthLoginBase<WechatOpenAccessTokenModel, WechatOpenUserInfoModel>
    {
        public WechatOpenOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }

        protected override string AuthorizeUrl => "https://open.weixin.qq.com/connect/qrconnect";
        protected override string AccessTokenUrl => "https://api.weixin.qq.com/sns/oauth2/access_token";
        protected override string UserInfoUrl => "https://api.weixin.qq.com/sns/userinfo";
        protected override Dictionary<string, string> BuildAuthorizeParams(string state)
        {
            return new Dictionary<string, string>()
            {
                ["response_type"] = "code",
                ["appid"] = oauthConfig.AppId,
                ["redirect_uri"] = System.Web.HttpUtility.UrlEncode(oauthConfig.RedirectUri),
                ["scope"] = oauthConfig.Scope,
                ["state"] = state
            };
        }
        public override string GetAuthorizeUrl(string state = "")
        {
            return $"{base.GetAuthorizeUrl(state)}#wechat_redirect";
        }
        protected override Dictionary<string, string> BuildGetAccessTokenParams(Dictionary<string, string> authorizeCallbackParams)
        {
            return new Dictionary<string, string>()
            {
                ["grant_type"] = "authorization_code",
                ["appid"] = $"{oauthConfig.AppId}",
                ["secret"] = $"{oauthConfig.AppKey}",
                ["code"] = $"{authorizeCallbackParams["code"]}"
            };
        }
        protected override Dictionary<string, string> BuildGetUserInfoParams(WechatOpenAccessTokenModel accessTokenModel)
        {
            return new Dictionary<string, string>()
            {
                ["access_token"] = accessTokenModel.AccessToken,
                ["openid"] = accessTokenModel.OpenId,
                ["lang"] = "zh_CN",
            };
        }
    }
}
