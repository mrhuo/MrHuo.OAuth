using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MrHuo.OAuth.Huawei
{
    /// <summary>
    /// https://github.com/HMS-Core/huawei-account-demo
    /// https://developer.huawei.com/consumer/cn/doc/HMSCore-References-V5/get-user-info-0000001060261938-V5
    /// </summary>
    public class HuaweiOAuth : OAuthLoginBase<HuaweiUserInfoModel>
    {
        public HuaweiOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }
        protected override string AuthorizeUrl => "https://oauth-login.cloud.huawei.com/oauth2/v3/authorize";
        protected override string AccessTokenUrl => "https://oauth-login.cloud.huawei.com/oauth2/v3/token";
        protected override string UserInfoUrl => "https://account.cloud.huawei.com/rest.php?nsp_svc=GOpen.User.getInfo";
        protected override Dictionary<string, string> BuildGetUserInfoParams(DefaultAccessTokenModel accessTokenModel)
        {
            var dict = base.BuildGetUserInfoParams(accessTokenModel);
            dict["getNickName"] = "1";
            return dict;
        }
        public override async Task<HuaweiUserInfoModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            var userInfoModel = await HttpRequestApi.PostAsync<HuaweiUserInfoModel>(
                UserInfoUrl,
                BuildGetUserInfoParams(accessTokenModel)
            );
            if (userInfoModel.HasError())
            {
                throw new Exception(userInfoModel.ErrorMessage);
            }
            return userInfoModel;
        }
    }
}
