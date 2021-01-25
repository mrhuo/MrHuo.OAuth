using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MrHuo.OAuth.StackOverflow
{
    /// <summary>
    /// https://api.stackexchange.com/docs/authentication
    /// </summary>
    public class StackOverflowOAuth : OAuthLoginBase<StackOverflowUserInfoModel>
    {
        private readonly string ApiKey;
        public StackOverflowOAuth(OAuthConfig oauthConfig, string key) : base(oauthConfig)
        {
            this.ApiKey = key;
        }

        protected override string AuthorizeUrl => "https://stackoverflow.com/oauth";

        protected override string AccessTokenUrl => "https://stackoverflow.com/oauth/access_token/json";

        protected override string UserInfoUrl => "https://api.stackexchange.com/2.2/me";

        protected override Dictionary<string, string> BuildGetUserInfoParams(DefaultAccessTokenModel accessTokenModel)
        {
            return new Dictionary<string, string>()
            {
                ["key"] = ApiKey,
                ["access_token"] = accessTokenModel.AccessToken,
                ["site"] = "stackoverflow"
            };
        }

        public override async Task<StackOverflowUserInfoModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            var userInfoModel = await HttpRequestApi.GetAsync<StackOverflowApiResponse<StackOverflowUserInfoModel>>(
                UserInfoUrl,
                BuildGetUserInfoParams(accessTokenModel)
            );
            if (!string.IsNullOrEmpty(userInfoModel.ErrorMessage))
            {
                throw new Exception(userInfoModel.ErrorMessage);
            }
            return userInfoModel.Items?.First();
        }
    }
}
