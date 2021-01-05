using System.Text.Json;
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

        public override string GetUserInfoUrl(QQAccessTokenModel accessToken)
        {
            return $"{USERINFO_URI}?access_token={accessToken.AccessToken}&oauth_consumer_key={AppId}&openid={GetOpenId(accessToken.AccessToken)}";
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
        /// 获取用户 openId
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public string GetOpenId(string accessToken)
        {
            var response = API.Get($"{OPENID_URI}?access_token={accessToken}");
            return JsonSerializer.Deserialize<QQOpenIdModel>(ClearCallbackResponse(response)).OpenId;
        }

        /// <summary>
        /// QQ 会返回类似 “callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} ); ” 的接口数据，这里统一处理一下
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string ClearCallbackResponse(string response)
        {
            if (response.Contains("callback"))
            {
                return response.Substring("callback(".Length, response.Length - 2);
            }
            return response;
        }
    }
}
