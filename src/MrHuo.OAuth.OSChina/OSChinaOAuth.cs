using System;
using System.Collections.Generic;
using System.Text;

namespace MrHuo.OAuth.OSChina
{
    /// <summary>
    /// https://www.oschina.net/openapi
    /// </summary>
    public class OSChinaOAuth : OAuthLoginBase<OSChinaUserInfoModel>
    {
        public OSChinaOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://www.oschina.net/action/oauth2/authorize";
        protected override string AccessTokenUrl => "https://www.oschina.net/action/openapi/token";
        protected override string UserInfoUrl => "https://www.oschina.net/action/openapi/user";
    }
}
