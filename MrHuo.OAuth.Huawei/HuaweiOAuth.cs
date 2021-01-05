using System.Collections.Generic;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Huawei
{
    /// <summary>
    /// https://github.com/HMS-Core/huawei-account-demo
    /// </summary>
    public class HuaweiOAuth : OAuthApiBase<HuaweiAccessTokenModel, HuaweiUserInfoModel>
    {
        private const string AUTHORIZE_URI = "https://oauth-login.cloud.huawei.com/oauth2/v3/authorize";
        private const string ACCESS_TOKEN_URI = "https://oauth-login.cloud.huawei.com/oauth2/v3/token";
        /// <summary>
        /// https://developer.huawei.com/consumer/cn/doc/HMSCore-References-V5/get-user-info-0000001060261938-V5
        /// </summary>
        private const string USERINFO_URI = "https://account.cloud.huawei.com/rest.php?nsp_svc=GOpen.User.getInfo";
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;
        private readonly string Scope;

        public HuaweiOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            AppId = _configuration["oauth:huawei:app_id"];
            AppKey = _configuration["oauth:huawei:app_key"];
            RedirectUri = HttpUtility.UrlEncode(_configuration["oauth:huawei:redirect_uri"]);
            Scope = _configuration["oauth:huawei:scope"];
        }

        public override string GetAuthorizeUrl(string state)
        {
            return $"{AUTHORIZE_URI}?response_type=code&client_id={AppId}&redirect_uri={RedirectUri}&access_type=offline&scope={Scope}";
        }

        public override string GetAccessTokenUrl(string code, string state)
        {
            return $"{ACCESS_TOKEN_URI}?grant_type=authorization_code&client_id={AppId}&client_secret={AppKey}&code={code}&redirect_uri={RedirectUri}";
        }

        public override string GetUserInfoUrl(HuaweiAccessTokenModel accessToken)
        {
            return USERINFO_URI;
        }

        public override HuaweiUserInfoModel GetUserInfo(HuaweiAccessTokenModel accessToken)
        {
            var json = API.Post(GetUserInfoUrl(accessToken), new Dictionary<string, string>()
            {
                ["access_token"] = accessToken.AccessToken,
                ["getNickName"] = "1"
            });
            return JsonSerializer.Deserialize<HuaweiUserInfoModel>(json);
        }
    }
}
