using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Github
{
    public class GithubOAuth : OAuthApiBase<GithubAccessTokenModel, GithubUserModel>
    {
        private const string AUTHORIZE_URI = "https://github.com/login/oauth/authorize";
        private const string ACCESS_TOKEN_URI = "https://github.com/login/oauth/access_token";
        private const string USERINFO_URI = "https://api.github.com/user";

        public GithubOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
            : base(configuration, httpContextAccessor)
        {
        }

        protected override string GetRedirectAuthorizeUrl(string state)
        {
            var clientId = _configuration["oauth:github:app_id"];
            var redirectUri = _configuration["oauth:github:redirect_uri"];
            var scope = _configuration["oauth:github:scope"];
            var url = $"{AUTHORIZE_URI}?" +
                $"client_id={clientId}" +
                $"&redirect_uri={redirectUri}" +
                $"&state={state}" +
                $"&scope={scope}";
            return url;
        }

        protected override string GetAccessTokenUrl(string code, string state)
        {
            var clientId = _configuration["oauth:github:app_id"];
            var clientSecret = _configuration["oauth:github:app_key"];
            var url = $"{ACCESS_TOKEN_URI}?client_id={clientId}&client_secret={clientSecret}&code={code}";
            return url;
        }

        public override GithubUserModel GetUserInfo(GithubAccessTokenModel accessToken)
        {
            var json = API.Get(USERINFO_URI, header: new Dictionary<string, string>()
            {
                ["Authorization"] = $"token {accessToken.AccessToken}"
            });
            return JsonSerializer.Deserialize<GithubUserModel>(json);
        }
    }
}
