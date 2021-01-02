using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Wechat
{
    public class WechatOAuth : OAuthApiBase<WechatAccessTokenModel, WechatUserInfoModel>
    {
        private const string AUTHORIZE_URI = "https://open.weixin.qq.com/connect/qrconnect";
        private const string ACCESS_TOKEN_URI = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private const string USERINFO_URI = "https://api.weixin.qq.com/sns/userinfo";

        public WechatOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
        }

        protected override string GetAccessTokenUrl(string code, string state)
        {
            var clientId = _configuration["oauth:wechat:app_id"];
            var clientSecret = _configuration["oauth:wechat:app_key"];
            var url = $"{ACCESS_TOKEN_URI}?" +
                $"appid={clientId}" +
                $"&secret={clientSecret}" +
                $"&code={code}" +
                $"&grant_type=authorization_code";
            return url;
        }

        protected override string GetRedirectAuthorizeUrl(string state)
        {
            var clientId = _configuration["oauth:wechat:app_id"];
            var redirectUri = _configuration["oauth:wechat:redirect_uri"];
            var scope = _configuration["oauth:wechat:scope"];
            var url = $"{AUTHORIZE_URI}?" +
                $"appid={clientId}" +
                $"&redirect_uri={redirectUri}" +
                $"&response_type=code" +
                $"&state={state}" +
                $"&scope={scope}" +
                $"#wechat_redirect";
            return url;
        }
    }
}
