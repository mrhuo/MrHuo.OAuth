using System;
using System.Collections.Generic;
using System.Text;

namespace MrHuo.OAuth.DouYin
{
    /// <summary>
    /// https://open.douyin.com/platform/doc/6848834666171009035
    /// </summary>
    public class DouYinOAuth : OAuthLoginBase<DouYinUserInfoModel>
    {
        public DouYinOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://open.douyin.com/platform/oauth/connect/";
        protected override string AccessTokenUrl => "https://open.douyin.com/oauth/access_token/";
        protected override string UserInfoUrl => "https://open.douyin.com/oauth/userinfo/";
    }
}
