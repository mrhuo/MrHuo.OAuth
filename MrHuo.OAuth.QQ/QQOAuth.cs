using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.QQ
{
    /// <summary>
    /// 相关文档：
    /// <para>https://wiki.open.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91%E4%BD%BF%E7%94%A8Authorization_Code%E8%8E%B7%E5%8F%96Access_Token_1</para>
    /// <para>https://wiki.open.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91API%E6%96%87%E6%A1%A3</para>
    /// </summary>
    public class QQOAuth : OAuthApiBase<QQAccessTokenModel, QQUserInfoModel>
    {
        private const string AUTHORIZE_URI = "https://graph.qq.com/oauth2.0/authorize";
        private const string ACCESS_TOKEN_URI = "https://graph.qq.com/oauth2.0/token";
        private const string USERINFO_URI = "https://api.github.com/user";
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;
        private readonly string Scope;

        public QQOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            AppId = _configuration["oauth:qq:app_id"];
            AppKey = _configuration["oauth:qq:app_key"];
            RedirectUri = HttpUtility.UrlEncode(_configuration["oauth:qq:redirect_uri"]);
            Scope = _configuration["oauth:qq:scope"];
        }

        protected override string GetRedirectAuthorizeUrl(string state)
        {
            return $"{AUTHORIZE_URI}?response_type=code&client_id={AppId}&redirect_uri={RedirectUri}&scope={Scope}&state={state}";
        }

        protected override string GetAccessTokenUrl(string code, string state)
        {
            return $"{ACCESS_TOKEN_URI}?grant_type=authorization_code&client_id={AppId}&client_secret={AppKey}&code={code}&redirect_uri={RedirectUri}&state={state}";
        }
    }
}
