using System;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Wechat
{
    /// <summary>
    /// Wechat OAuth 相关文档参考：
    /// <para>https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/Wechat_webpage_authorization.html</para>
    /// </summary>
    public class WechatOAuth : OAuthApiBase<WechatAccessTokenModel, WechatUserInfoModel>
    {
        private const string AUTHORIZE_URI = "https://open.weixin.qq.com/connect/oauth2/authorize";
        private const string ACCESS_TOKEN_URI = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private const string USERINFO_URI = "https://api.weixin.qq.com/sns/userinfo";
        private readonly string Scope;
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;

        public WechatOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            AppId = _configuration["oauth:wechat:app_id"];
            AppKey = _configuration["oauth:wechat:app_key"];
            RedirectUri = HttpUtility.UrlEncode(_configuration["oauth:wechat:redirect_uri"]);
            Scope = _configuration["oauth:wechat:scope"];
        }

        public override string GetAuthorizeUrl(string state)
        {
            return $"{AUTHORIZE_URI}?appid={AppId}&redirect_uri={RedirectUri}&response_type=code&state={state}&scope={Scope}#wechat_redirect";
        }

        public override string GetAccessTokenUrl(string code, string state)
        {
            return $"{ACCESS_TOKEN_URI}?appid={AppId}&secret={AppKey}&code={code}&grant_type=authorization_code";
        }

        public override WechatUserInfoModel GetUserInfo(WechatAccessTokenModel accessToken)
        {
            var api = $"{USERINFO_URI}?access_token={accessToken.AccessToken}&openid={accessToken.OpenId}&lang=zh_CN";
            var json = API.Get(api);
            var userInfo = JsonSerializer.Deserialize<WechatUserInfoModel>(json);
            if (userInfo.ErrorCode != 0)
            {
                throw new OAuthException(json);
            }
            return userInfo;
        }
    }
}