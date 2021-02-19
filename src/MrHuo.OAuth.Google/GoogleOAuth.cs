using System;

namespace MrHuo.OAuth.Google
{
    /// <summary>
    /// 文档：https://developers.google.com/identity/protocols/OAuth2
    /// Scope: https://developers.google.com/identity/protocols/oauth2/scopes
    /// </summary>
    public class GoogleOAuth : OAuthLoginBase<GoogleUserInfoModel>
    {
        public GoogleOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://accounts.google.com/o/oauth2/v2/auth";

        protected override string AccessTokenUrl => "https://oauth2.googleapis.com/token";

        protected override string UserInfoUrl => "https://openidconnect.googleapis.com/v1/userinfo";
    }
}
