using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MrHuo.OAuth.Mi
{
    /// <summary>
    /// https://dev.mi.com/docs/passport/oauth2/
    /// https://dev.mi.com/docs/passport/authorization-code/
    /// https://dev.mi.com/docs/passport/open-api/
    /// </summary>
    public class MiOAuth : OAuthLoginBase<MiUserInfoModel>
    {
        public MiOAuth(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }

        protected override string AuthorizeUrl => "https://account.xiaomi.com/oauth2/authorize";

        protected override string AccessTokenUrl => "https://account.xiaomi.com/oauth2/token";

        protected override string UserInfoUrl => "https://open.account.xiaomi.com/user/profile";

        protected override Dictionary<string, string> BuildGetUserInfoParams(DefaultAccessTokenModel accessTokenModel)
        {
            return new Dictionary<string, string>
            {
                ["clientId"] = oauthConfig.AppId,
                ["token"] = accessTokenModel.AccessToken,
            };
        }

        public override async Task<MiUserInfoModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            var userInfoModel = await HttpRequestApi.GetAsync<MiApiResponse<MiUserInfoModel>>(
                UserInfoUrl,
                BuildGetUserInfoParams(accessTokenModel)
            );
            if (userInfoModel.HasError())
            {
                throw new Exception(userInfoModel.Description);
            }
            return userInfoModel.Data;
        }
    }
}
