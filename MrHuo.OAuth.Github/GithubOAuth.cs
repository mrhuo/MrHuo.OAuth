using System.Collections.Generic;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Github
{
    /// <summary>
    /// Github OAuth 相关文档参考：
    /// <para>https://docs.github.com/v3/oauth/</para>
    /// <para>http://www.ruanyifeng.com/blog/2019/04/github-oauth.html</para>
    /// </summary>
    public class GithubOAuth : OAuthApiBase<GithubAccessTokenModel, GithubUserModel>
    {
        private const string AUTHORIZE_URI = "https://github.com/login/oauth/authorize";
        private const string ACCESS_TOKEN_URI = "https://github.com/login/oauth/access_token";
        private const string USERINFO_URI = "https://api.github.com/user";
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;
        private readonly string Scope;

        public GithubOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            AppId = _configuration["oauth:github:app_id"];
            AppKey = _configuration["oauth:github:app_key"];
            RedirectUri = HttpUtility.UrlEncode(_configuration["oauth:github:redirect_uri"]);
            Scope = _configuration["oauth:github:scope"];
        }

        public override string GetAuthorizeUrl(string state)
        {
            return $"{AUTHORIZE_URI}?client_id={AppId}&redirect_uri={RedirectUri}&state={state}&scope={Scope}";
        }

        public override string GetAccessTokenUrl(string code, string state)
        {
            return $"{ACCESS_TOKEN_URI}?client_id={AppId}&client_secret={AppKey}&code={code}";
        }

        public override string GetUserInfoUrl(GithubAccessTokenModel accessToken)
        {
            return USERINFO_URI;
        }

        public override GithubUserModel GetUserInfo(GithubAccessTokenModel accessToken)
        {
            var json = API.Get(GetUserInfoUrl(accessToken), new Dictionary<string, string>()
            {
                ["Authorization"] = $"token {accessToken.AccessToken}"
            });
            return JsonSerializer.Deserialize<GithubUserModel>(json);
        }
    }
}
