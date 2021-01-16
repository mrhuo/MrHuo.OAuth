using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MrHuo.OAuth.XunLei
{
    public class XunLeiOAuth : OAuthLoginBase<XunLeiUserInfoModel>
    {
        public XunLeiOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://i.xunlei.com/xluser/oauth.html";

        protected override string AccessTokenUrl => "https://xluser-ssl.xunlei.com/v1/auth/token";

        protected override string UserInfoUrl => "https://xluser-ssl.xunlei.com/v1/user/me";

        /// <summary>
        /// 迅雷会验证 UserAgent，如果是默认浏览器的话，就会抛出错误：
        /// <para>{"error":"permission_denied","error_code":7,"error_description":"[Danger], Please Do Not Save Your client_secret in browser, it is NOT SAFE"}</para>
        /// </summary>
        /// <param name="authorizeCallbackParams"></param>
        /// <returns></returns>
        public override async Task<DefaultAccessTokenModel> GetAccessTokenAsync(Dictionary<string, string> authorizeCallbackParams)
        {
            var accessTokenModel = await HttpRequestApi.PostAsync<DefaultAccessTokenModel>(
                AccessTokenUrl,
                BuildGetAccessTokenParams(authorizeCallbackParams),
                new Dictionary<string, string>()
                {
                    { "User-Agent", "OAuthLogin.net App"}
                }
            );
            if (accessTokenModel.HasError())
            {
                throw new Exception(accessTokenModel.ErrorDescription);
            }
            return accessTokenModel;
        }

        public override async Task<XunLeiUserInfoModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            var userInfoModel = await HttpRequestApi.GetAsync<XunLeiUserInfoModel>(
                UserInfoUrl,
                null,
                new Dictionary<string, string>() {
                    {"authorization", $"Bearer {accessTokenModel.AccessToken}" }
                }
            );
            if (userInfoModel.HasError())
            {
                throw new Exception(userInfoModel.ErrorMessage);
            }
            return userInfoModel;
        }
    }
}
