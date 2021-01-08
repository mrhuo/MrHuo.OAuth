using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MrHuo.OAuth.Github
{
    /// <summary>
    /// Github OAuth 相关文档参考：
    /// <para>https://docs.github.com/v3/oauth/</para>
    /// <para>http://www.ruanyifeng.com/blog/2019/04/github-oauth.html</para>
    /// </summary>
    public class GithubOAuth : OAuthLoginBase<GithubUserModel>
    {
        public GithubOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }
        protected override string AuthorizeUrl => "https://github.com/login/oauth/authorize";
        protected override string AccessTokenUrl => "https://github.com/login/oauth/access_token";
        protected override string UserInfoUrl => "https://api.github.com/user";
        public override async Task<GithubUserModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            var userInfoModel = await HttpRequestApi.GetAsync<GithubUserModel>(
                UserInfoUrl,
                BuildGetUserInfoParams(accessTokenModel),
                new Dictionary<string, string>()
                {
                    ["Authorization"] = $"token {accessTokenModel.AccessToken}"
                }
            );
            if (userInfoModel.HasError())
            {
                throw new Exception(userInfoModel.ErrorMessage);
            }
            return userInfoModel;
        }
    }
}
