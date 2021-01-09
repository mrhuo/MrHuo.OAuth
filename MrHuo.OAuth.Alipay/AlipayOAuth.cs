using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alipay.EasySDK.Factory;
using Alipay.EasySDK.Kernel;

namespace MrHuo.OAuth.Alipay
{
    /// <summary>
    /// https://opendocs.alipay.com/open/284/106001
    /// 支付宝回调URL：
    /// https://oauthlogin.net/oauth/alipaycallback?app_id=2021002122645005&source=alipay_wallet&userOutputs=auth_user&scope=auth_user&alipay_token=&auth_code=2c58e763fdca4fb6b1f5a5bf4d26WA05
    /// https://github.com/alipay/alipay-easysdk/tree/master/csharp
    /// </summary>
    public class AlipayOAuth : OAuthLoginBase<AlipayAccessTokenModel, AlipayUserInfoModel>
    {
        public AlipayOAuth(OAuthConfig oauthConfig, string privateRSAKey, string publicRSAKey, string encryptKey) : base(oauthConfig)
        {
            var config = new Config()
            {
                Protocol = "http",
                GatewayHost = "openapi.alipay.com",
                SignType = "RSA2",

                AppId = oauthConfig.AppId,

                // 为避免私钥随源码泄露，推荐从文件中读取私钥字符串而不是写入源码中
                MerchantPrivateKey = privateRSAKey,

                // 如果采用非证书模式，则无需赋值上面的三个证书路径，改为赋值如下的支付宝公钥字符串即可
                AlipayPublicKey = publicRSAKey,

                //可设置异步通知接收服务地址（可选）
                NotifyUrl = oauthConfig.RedirectUri,

                //可设置AES密钥，调用AES加解密相关接口时需要（可选）
                EncryptKey = encryptKey
            };
            Factory.SetOptions(config);
        }

        protected override string AuthorizeUrl => "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm";
        protected override string AccessTokenUrl => "https://openapi.alipay.com/gateway.do";
        protected override string UserInfoUrl => throw new NotImplementedException();

        protected override Dictionary<string, string> BuildAuthorizeParams(string state)
        {
            return new Dictionary<string, string>()
            {
                ["response_type"] = "code",
                ["app_id"] = $"{oauthConfig.AppId}",
                ["redirect_uri"] = $"{oauthConfig.RedirectUri}",
                ["scope"] = $"{oauthConfig.Scope}",
                ["state"] = $"{state}"
            };
        }

        public override async Task<AlipayAccessTokenModel> GetAccessTokenAsync(Dictionary<string, string> authorizeCallbackParams)
        {
            var token = await Factory.Base.OAuth().GetTokenAsync(authorizeCallbackParams["code"]);
            if (!string.IsNullOrEmpty(token.SubMsg))
            {
                throw new Exception(token.SubMsg);
            }
            return new AlipayAccessTokenModel()
            {
                AccessToken = token.AccessToken,
                Error = token.SubCode,
                ErrorDescription = token.SubMsg,
                ExpiresIn = token.ExpiresIn,
                RefreshToken = token.RefreshToken,
                UserId = token.UserId
            };
        }

        public override async Task<AlipayUserInfoModel> GetUserInfoAsync(AlipayAccessTokenModel accessTokenModel)
        {
            var user = await Factory.Util.Generic()
                .ExecuteAsync("alipay.user.info.share", new Dictionary<string, string>()
                {
                    ["auth_token"] = accessTokenModel.AccessToken
                }, new Dictionary<string, object>());
            if (!string.IsNullOrEmpty(user.SubMsg))
            {
                throw new Exception(user.SubMsg);
            }
            Console.WriteLine($"GetUserInfoAsync: {user.HttpBody}");
            return System.Text.Json.JsonSerializer.Deserialize<AlipayUserInfoModel>(user.HttpBody);
        }
    }
}
