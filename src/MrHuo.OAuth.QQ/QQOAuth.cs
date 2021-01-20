using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MrHuo.OAuth.QQ
{
    /// <summary>
    /// 相关文档：
    /// <para>https://wiki.open.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91%E4%BD%BF%E7%94%A8Authorization_Code%E8%8E%B7%E5%8F%96Access_Token_1</para>
    /// <para>https://wiki.open.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91API%E6%96%87%E6%A1%A3</para>
    /// </summary>
    public class QQOAuth : OAuthLoginBase<QQUserInfoModel>
    {
        private const string OPENID_URI = "https://graph.qq.com/oauth2.0/me";
        protected override string AuthorizeUrl => "https://graph.qq.com/oauth2.0/authorize";
        protected override string AccessTokenUrl => "https://graph.qq.com/oauth2.0/token";
        protected override string UserInfoUrl => "https://graph.qq.com/user/get_user_info";

        public QQOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        /// <summary>
        /// 如果成功返回，即可在返回包中获取到 Access Token。
        /// 返回如下字符串：access_token=FE04************************CCE2&expires_in=7776000 。
        /// </summary>
        /// <param name="authorizeCallbackParams"></param>
        /// <returns></returns>
        public override async Task<DefaultAccessTokenModel> GetAccessTokenAsync(Dictionary<string, string> authorizeCallbackParams)
        {
            var accessTokenModelReponseText = await HttpRequestApi.PostStringAsync(
                AccessTokenUrl,
                BuildGetAccessTokenParams(authorizeCallbackParams)
            );
            if (string.IsNullOrEmpty(accessTokenModelReponseText))
            {
                throw new Exception("没有获取到正确的 AccessToken！");
            }
            var arr = accessTokenModelReponseText.Split('&');
            return new DefaultAccessTokenModel()
            {
                AccessToken = arr[0],
                ExpiresIn = int.Parse(arr[1])
            };
        }

        protected override Dictionary<string, string> BuildGetUserInfoParams(DefaultAccessTokenModel accessTokenModel)
        {
            var openId = GetOpenId(accessTokenModel.AccessToken).ConfigureAwait(false).GetAwaiter().GetResult();
            return new Dictionary<string, string>()
            {
                ["access_token"] = accessTokenModel.AccessToken,
                ["oauth_consumer_key"] = oauthConfig.AppId,
                ["openid"] = openId,
            };
        }

        /// <summary>
        /// 获取用户 openId
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<string> GetOpenId(string accessToken)
        {
            var response = await HttpRequestApi.GetStringAsync($"{OPENID_URI}?access_token={accessToken}");
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
