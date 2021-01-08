namespace MrHuo.OAuth.Gitlab
{
    public class GitlabOAuth : OAuthLoginBase<GitlabUserInfoModel>
    {
        public GitlabOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://gitlab.com/oauth/authorize";

        protected override string AccessTokenUrl => "https://gitlab.com/oauth/token";

        protected override string UserInfoUrl => "https://gitlab.com/api/v4/user";
    }
}
