namespace MrHuo.OAuth.Gitee
{
    /// <summary>
    /// https://gitee.com/api/v5/oauth_doc#/
    /// </summary>
    public class GiteeOAuth : OAuthLoginBase<GiteeUserModel>
    {
        public GiteeOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }
        protected override string AuthorizeUrl => "https://gitee.com/oauth/authorize";
        protected override string AccessTokenUrl => "https://gitee.com/oauth/token";
        protected override string UserInfoUrl => "https://gitee.com/api/v5/user";
    }
}
