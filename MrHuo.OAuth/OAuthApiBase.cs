using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth
{
    /// <summary>
    /// OAuth 授权基类
    /// </summary>
    /// <typeparam name="TAccessToken"></typeparam>
    /// <typeparam name="TUserInfo"></typeparam>
    public abstract class OAuthApiBase<TAccessToken, TUserInfo> : IOAuthApi<TAccessToken, TUserInfo>
         where TAccessToken : IAccessTokenModel
         where TUserInfo : IUserInfoModel
    {
        protected readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 允许 state 检查（防止跨站请求伪造（CSRF）攻击），默认为 true
        /// </summary>
        public bool EnableStateCheck { get; set; } = true;

        public OAuthApiBase(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取需要重定向的 AuthorizeUrl
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract string GetAuthorizeUrl(string state);

        /// <summary>
        /// 获取 AccessToken 请求 URL
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract string GetAccessTokenUrl(string code, string state);

        /// <summary>
        /// 获取用户信息接口 URL
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public abstract string GetUserInfoUrl(TAccessToken accessToken);

        /// <summary>
        /// 检查授权完成后的回调地址 URL 中，是否包含错误，没有错误请返回 null。
        /// <para>默认检查 URL 中是否包含 error 和 error_description 字段</para>
        /// </summary>
        /// <returns></returns>
        protected virtual OAuthException GetAuthorizeCallbackException()
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
        /// 虚方法，从 accessTokenResponse 解析 TAccessToken。
        /// <para>提供此方法的原因是因为 QQ 这个平台，接口返回的并非是 JSON 格式，而是 QueryString 格式。</para>
        /// <para>默认使用：JsonSerializer.Deserialize</para>
        /// </summary>
        /// <param name="accessTokenResponse"></param>
        /// <returns></returns>
        protected virtual TAccessToken ResolveAccessTokenFromString(string accessTokenResponse)
        {
            OAuthLog.Log("ResolveAccessTokenFromString [{0}]", accessTokenResponse);
            return JsonSerializer.Deserialize<TAccessToken>(accessTokenResponse);
        }

        /// <summary>
        /// 虚方法，执行授权
        /// </summary>
        public virtual void Authorize()
        {
            var requestPath = _httpContextAccessor.HttpContext.Request.Path;
            OAuthLog.Log("Start authorize [{0}]", requestPath);
            var state = "";
            if (EnableStateCheck)
            {
                state = OAuthStateManager.RequestState(_httpContextAccessor.HttpContext, GetType());
            }
            OAuthLog.Log("Start authorize [{0}], state=[{1}]", requestPath, state);
            var redirectUrl = GetAuthorizeUrl(state);
            OAuthLog.Log("Authorize [{0}], start redirect=[{1}]", requestPath, redirectUrl);
            _httpContextAccessor.HttpContext.Response.Redirect(redirectUrl, true);
        }

        /// <summary>
        /// 虚方法，执行授权回调
        /// </summary>
        /// <returns></returns>
        public virtual TAccessToken AuthorizeCallback()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var queryStrings = request.QueryString.ToString();
            OAuthLog.Log("Start AuthorizeCallback, query string=[{0}]", queryStrings);
            var exception = GetAuthorizeCallbackException();
            if (exception != null)
            {
                throw exception;
            }
            var code = request.Query["code"];
            var state = request.Query["state"];
            if (string.IsNullOrEmpty(code))
            {
                throw Errors.ParameterMissing(nameof(code));
            }
            if (string.IsNullOrEmpty(state))
            {
                throw Errors.ParameterMissing(nameof(state));
            }
            return GetAccessToken(code, state);
        }

        /// <summary>
        /// 虚方法，获取 AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual TAccessToken GetAccessToken(string code, string state)
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
            return ResolveAccessTokenFromString(API.Post(GetAccessTokenUrl(code, state)));
        }

        /// <summary>
        /// 虚方法，获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual TUserInfo GetUserInfo(TAccessToken accessToken)
        {
            return JsonSerializer.Deserialize<TUserInfo>(API.Get(GetUserInfoUrl(accessToken)));
        }
    }
}
