namespace MrHuo.OAuth.Baidu
{
    /// <summary>
    /// <para>http://developer.baidu.com/console#app/project</para>
    /// <para>http://developer.baidu.com/wiki/index.php?title=docs/oauth</para>
    /// <para>http://developer.baidu.com/wiki/index.php?title=docs/oauth/rest/file_data_apis_list</para>
    /// </summary>
    public class BaiduOAuth : OAuthLoginBase<BaiduAccessTokenModel, BaiduUserInfoModel>
    {
        public BaiduOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://openapi.baidu.com/oauth/2.0/authorize";
        protected override string AccessTokenUrl => "https://openapi.baidu.com/oauth/2.0/token";
        protected override string UserInfoUrl => "https://openapi.baidu.com/rest/2.0/passport/users/getInfo";
    }
}