using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Baidu
{
    /// <summary>
    /// <para>http://developer.baidu.com/wiki/index.php?title=docs/oauth</para>
    /// <para>http://developer.baidu.com/wiki/index.php?title=docs/oauth/rest/file_data_apis_list</para>
    /// </summary>
    public class BaiduOAuth : OAuthApiBase<BaiduAccessTokenModel, BaiduUserInfoModel>
    {
        private const string AUTHORIZE_URI = "https://openapi.baidu.com/oauth/2.0/authorize";
        private const string ACCESS_TOKEN_URI = "https://openapi.baidu.com/oauth/2.0/token";
        private const string USERINFO_URI = "https://openapi.baidu.com/rest/2.0/passport/users/getInfo";
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;
        private readonly string Scope;

        public BaiduOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            AppId = _configuration["oauth:baidu:app_id"];
            AppKey = _configuration["oauth:baidu:app_key"];
            RedirectUri = HttpUtility.UrlEncode(_configuration["oauth:baidu:redirect_uri"]);
            Scope = _configuration["oauth:baidu:scope"];
        }

        public override string GetAuthorizeUrl(string state)
        {
            return $"{AUTHORIZE_URI}?response_type=code&client_id={AppId}&redirect_uri={RedirectUri}&scope={Scope}&state={state}&display=popup";
        }

        public override string GetAccessTokenUrl(string code, string state)
        {
            return $"{ACCESS_TOKEN_URI}?grant_type=client_credentials&client_id={AppId}&client_secret={AppKey}";
        }

        public override string GetUserInfoUrl(BaiduAccessTokenModel accessToken)
        {
            return $"{USERINFO_URI}?access_token={accessToken.AccessToken}";
        }

        public override BaiduUserInfoModel GetUserInfo(BaiduAccessTokenModel accessToken)
        {
            var userInfo = base.GetUserInfo(accessToken);
            if (userInfo.HasError())
            {
                throw new OAuthException(userInfo.ErrorMessage);
            }
            return userInfo;
        }
    }
}
