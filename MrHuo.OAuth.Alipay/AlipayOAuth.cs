using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.Alipay
{
    /// <summary>
    /// https://opendocs.alipay.com/open/284/106001
    /// </summary>
    public class AlipayOAuth : OAuthApiBase<AlipayAccessTokenModel, AlipayUserInfoModel>
    {
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string RedirectUri;
        private readonly string Scope;
        private readonly AlipayApiRequest AlipayApiRequest;

        public AlipayOAuth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            AppId = _configuration["oauth:alipay:app_id"];
            AppKey = _configuration["oauth:alipay:app_key"];
            RedirectUri = HttpUtility.UrlEncode(_configuration["oauth:alipay:redirect_uri"]);
            Scope = _configuration["oauth:alipay:scope"];

            var privateRSAKey = _configuration["oauth:alipay:private_key"];
            var publicRSAKey = _configuration["oauth:alipay:public_key"];
            AlipayApiRequest = new AlipayApiRequest()
            {
                AppId = AppId,
                PrivateRSAKey = privateRSAKey,
                PublicRSAKey = publicRSAKey
            };
        }

        public override string GetAuthorizeUrl(string state)
        {
            return $"https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?app_id={AppId}&scope={Scope}&redirect_uri={RedirectUri}&state={state}";
        }

        public override string GetAccessTokenUrl(string code, string state)
        {
            return "https://openapi.alipay.com/gateway.do";
        }

        public override string GetUserInfoUrl(AlipayAccessTokenModel accessToken)
        {
            throw new NotImplementedException();
        }

        public override AlipayAccessTokenModel AuthorizeCallback()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var queryStrings = request.QueryString.ToString();
            OAuthLog.Log("Start AuthorizeCallback, query string=[{0}]", queryStrings);
            var exception = GetAuthorizeCallbackException();
            if (exception != null)
            {
                throw exception;
            }
            //支付宝回调URL：
            //https://oauthlogin.net/oauth/alipaycallback?app_id=2021002122645005&source=alipay_wallet&userOutputs=auth_user&scope=auth_user&alipay_token=&auth_code=2c58e763fdca4fb6b1f5a5bf4d26WA05
            var code = request.Query["auth_code"];
            var state = request.Query["state"];
            if (string.IsNullOrEmpty(code))
            {
                throw Errors.ParameterMissing("auth_code");
            }
            if (EnableStateCheck && string.IsNullOrEmpty(state))
            {
                throw Errors.ParameterMissing("state");
            }
            return GetAccessToken(code, state);
        }

        public override AlipayAccessTokenModel GetAccessToken(string code, string state)
        {
            OAuthLog.Log("Start GetAccessToken code=[{0}], state=[{1}]", code, state);
            if (EnableStateCheck)
            {
                if (string.IsNullOrEmpty(state) || !OAuthStateManager.IsStateExist(state))
                {
                    throw Errors.ForbidCSRFException();
                }
                OAuthStateManager.RemoveState(_httpContextAccessor.HttpContext, state);
            }
            var token = AlipayApiRequest.Get<AlipayAccessTokenModel>("alipay.system.oauth.token", new Dictionary<string, string>()
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code
            });
            return token;
        }
    }
}
