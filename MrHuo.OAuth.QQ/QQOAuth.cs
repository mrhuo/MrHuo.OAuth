using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.QQ
{
    /// <summary>
    /// 相关文档：
    /// <para>https://wiki.open.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91%E4%BD%BF%E7%94%A8Authorization_Code%E8%8E%B7%E5%8F%96Access_Token_1</para>
    /// <para>https://wiki.open.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91API%E6%96%87%E6%A1%A3</para>
    /// </summary>
    public class QQOAuth : OAuthApiBase<QQAccessTokenModel, QQUserInfoModel>
    {
        private const string AUTHORIZE_URI = "https://graph.qq.com/oauth2.0/authorize";
        private const string ACCESS_TOKEN_URI = "https://graph.qq.com/oauth2.0/token";
        private const string OPENID_URI = "https://graph.qq.com/oauth2.0/me";
        private const string USERINFO_URI = "https://graph.qq.com/user/get_user_info";
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;
        private readonly string Scope;

        public QQOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            AppId = _configuration["oauth:qq:app_id"];
            AppKey = _configuration["oauth:qq:app_key"];
            RedirectUri = HttpUtility.UrlEncode(_configuration["oauth:qq:redirect_uri"]);
            Scope = _configuration["oauth:qq:scope"];
        }

        public override string GetAuthorizeUrl(string state)
        {
            return $"{AUTHORIZE_URI}?response_type=code&client_id={AppId}&redirect_uri={RedirectUri}&scope={Scope}&state={state}";
        }

        public override string GetAccessTokenUrl(string code, string state)
        {
            return $"{ACCESS_TOKEN_URI}?grant_type=authorization_code&client_id={AppId}&client_secret={AppKey}&code={code}&redirect_uri={RedirectUri}&state={state}";
        }

        /// <summary>
        /// 如果成功返回，即可在返回包中获取到Access Token。
        /// 返回如下字符串：access_token=FE04************************CCE2&expires_in=7776000 。
        /// </summary>
        /// <param name="accessTokenResponse"></param>
        /// <returns></returns>
        protected override QQAccessTokenModel ResolveAccessTokenFromString(string accessTokenResponse)
        {
            var arr = accessTokenResponse.Split('&');
            return new QQAccessTokenModel()
            {
                AccessToken = arr[0],
                ExpiresIn = int.Parse(arr[1])
            };
        }

        /// <summary>
        /// https://wiki.connect.qq.com/%E5%BC%80%E5%8F%91%E6%94%BB%E7%95%A5_server-side
        /// </summary>
        /// <returns></returns>
        protected override OAuthException GetAuthorizeCallbackException()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var error = request.Query["error"];
            var errorDescription = request.Query["error_description"];
            if (!string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(errorDescription))
            {
                return new OAuthException(errorDescription);
            }
            return null;
        }

        /// <summary>
        /// https://wiki.open.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91%E5%BC%80%E5%8F%91%E6%94%BB%E7%95%A5_Server-side#Step3.EF.BC.9A.E9.80.9A.E8.BF.87Authorization_Code.E8.8E.B7.E5.8F.96Access_Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public override QQUserInfoModel GetUserInfo(QQAccessTokenModel accessToken)
        {
            return base.GetUserInfo(accessToken);
        }

        public string GetOpenId(QQAccessTokenModel accessToken)
        {
            var response = API.Get($"{OPENID_URI}?access_token={accessToken.AccessToken}");
            //var json = 
            //https://graph.qq.com/oauth2.0/me?access_token=YOUR_ACCESS_TOKEN
            return response;
        }

        /// <summary>
        /// QQ 会返回类似 “callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} ); ” 的接口数据，这里统一处理一下
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string ClearCallbackResponse(string response)
        {
            return response;
        }
    }
}
