using System.Collections.Generic;

namespace MrHuo.OAuth.Wechat
{
    /// <summary>
    /// Wechat OAuth 相关文档参考：
    /// <para>https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/Wechat_webpage_authorization.html</para>
    /// </summary>
    public class WechatOAuth : OAuthLoginBase<WechatAccessTokenModel, WechatUserInfoModel>
    {
        public WechatOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }

        protected override string AuthorizeUrl => "https://open.weixin.qq.com/connect/oauth2/authorize";
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
                ["state"] = state + "#wechat_redirect",
                //,
            };
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
        protected override Dictionary<string, string> BuildGetUserInfoParams(WechatAccessTokenModel accessTokenModel)
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