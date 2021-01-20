using System.Collections.Generic;

namespace MrHuo.OAuth.SinaWeibo
{
    /// <summary>
    /// https://open.weibo.com/wiki/%E5%BE%AE%E5%8D%9AAPI
    /// </summary>
    public class SinaWeiboOAuth : OAuthLoginBase<SinaWeiboAccessTokenModel, SinaWeiboUserInfoModel>
    {
        public SinaWeiboOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }

        protected override string AuthorizeUrl => "https://api.weibo.com/oauth2/authorize";
        protected override string AccessTokenUrl => "https://api.weibo.com/oauth2/access_token";
        protected override string UserInfoUrl => "https://api.weibo.com/2/users/show.json";

        protected override Dictionary<string, string> BuildGetUserInfoParams(SinaWeiboAccessTokenModel accessTokenModel)
        {
            var dict = base.BuildGetUserInfoParams(accessTokenModel);
            dict["uid"] = $"{accessTokenModel.Uid}";
            return dict;
        }
    }
}
