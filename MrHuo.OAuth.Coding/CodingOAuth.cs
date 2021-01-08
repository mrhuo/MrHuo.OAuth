namespace MrHuo.OAuth.Coding
{
    /// <summary>
    /// https://help.coding.net/docs/open/start.html
    /// </summary>
    public class CodingOAuth : OAuthLoginBase<CodingUserInfoModel>
    {
        private readonly string team;
        public CodingOAuth(OAuthConfig oauthConfig, string team) : base(oauthConfig)
        {
            this.team = team;
        }
        protected override string AuthorizeUrl
        {
            get
            {
                return $"https://{team}.coding.net/oauth_authorize.html";
            }
        }
        protected override string AccessTokenUrl
        {
            get
            {
                return $"https://{team}.coding.net/api/oauth/access_token";
            }
        }
        protected override string UserInfoUrl
        {
            get
            {
                return $"https://{team}.coding.net/api/me";
            }
        }
    }
}
