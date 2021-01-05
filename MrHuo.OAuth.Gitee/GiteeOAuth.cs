using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Gitee
{
    public class GiteeOAuth : OAuthApiBase<GiteeAccessTokenModel, GiteeUserModel>
    {
        private const string AUTHORIZE_URI = "https://gitee.com/oauth/authorize";
        private const string ACCESS_TOKEN_URI = "https://gitee.com/oauth/token";
        private const string USERINFO_URI = "https://gitee.com/api/v5/user";
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;
        private readonly string Scope;

        public GiteeOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            AppId = _configuration["oauth:gitee:app_id"];
            AppKey = _configuration["oauth:gitee:app_key"];
            RedirectUri = HttpUtility.UrlEncode(_configuration["oauth:gitee:redirect_uri"]);
            Scope = _configuration["oauth:gitee:scope"];
        }

        public override string GetAuthorizeUrl(string state)
        {
            return $"{AUTHORIZE_URI}?client_id={AppId}&redirect_uri={RedirectUri}&response_type=code&scope={Scope}&state={state}";
        }

        public override string GetAccessTokenUrl(string code, string state)
        {
            return $"{ACCESS_TOKEN_URI}?grant_type=authorization_code&code={code}&client_id={AppId}&redirect_uri={RedirectUri}&client_secret={AppKey}";
        }

        public override string GetUserInfoUrl(GiteeAccessTokenModel accessToken)
        {
            return $"{USERINFO_URI}?access_token={accessToken.AccessToken}";
        }
    }
}
