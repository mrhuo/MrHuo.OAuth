using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Wechat
{
    public class WechatOAuth : OAuthApiBase<WechatAccessTokenModel, WechatUserInfoModel>
    {
        private const string AUTHORIZE_URI_QRCODE = "https://open.weixin.qq.com/connect/qrconnect";
        private const string AUTHORIZE_URI_CLIENT = "https://open.weixin.qq.com/connect/oauth2/authorize";
        private const string ACCESS_TOKEN_URI = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private const string USERINFO_URI = "https://api.weixin.qq.com/sns/userinfo";
        private WechatOAuthType WechatOAuthType;
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;
        private readonly string Scope;

        public WechatOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            var type = _configuration["oauth:wechat:type"];
            Enum.TryParse(type, true, out WechatOAuthType);
            AppId = _configuration["oauth:wechat:app_id"];
            AppKey = _configuration["oauth:wechat:app_key"];
            RedirectUri = _configuration["oauth:wechat:redirect_uri"];
            Scope = _configuration["oauth:wechat:scope"];
        }

        public void SetWechatOAuthType(WechatOAuthType type)
        {
            this.WechatOAuthType = type;
        }

        protected override string GetRedirectAuthorizeUrl(string state)
        {
            return $"{(WechatOAuthType == WechatOAuthType.Qrcode ? AUTHORIZE_URI_QRCODE : AUTHORIZE_URI_CLIENT)}" +
                $"?appid={AppId}&redirect_uri={RedirectUri}&response_type=code&state={state}&scope={Scope}#wechat_redirect";
        }

        protected override string GetAccessTokenUrl(string code, string state)
        {
            return $"{ACCESS_TOKEN_URI}?appid={AppId}&secret={AppKey}&code={code}&grant_type=authorization_code";
        }
    }

    public enum WechatOAuthType
    {
        Qrcode,
        Client
    }
}