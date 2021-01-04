using System;
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
        protected abstract string GetRedirectAuthorizeUrl(string state);

        /// <summary>
        /// 获取 AccessToken 请求 URL
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected abstract string GetAccessTokenUrl(string code, string state);

        /// <summary>
        /// 虚方法，从 accessTokenResponse 解析 TAccessToken。
        /// <para>默认使用：JsonSerializer.Deserialize</para>
        /// </summary>
        /// <param name="accessTokenResponse"></param>
        /// <returns></returns>
        protected virtual TAccessToken ResolveAccessTokenFromString(string accessTokenResponse)
        {
            OAuthLog.Log("ResolveAccessTokenFromString [{0}]", accessTokenResponse);
            return Json.Deserialize<TAccessToken>(accessTokenResponse);
        }

        /// <summary>
        /// 虚方法，执行授权
        /// </summary>
        public virtual void Authorize()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new OAuthException("请确保在 Web 环境下使用！（HttpContext 为 null）");
            }
            OAuthLog.Log("Start authorize [{0}]", _httpContextAccessor.HttpContext.Request.Path);
            var state = "";
            if (EnableStateCheck)
            {
                state = OAuthStateManager.RequestState(_httpContextAccessor.HttpContext, GetType());
            }
            OAuthLog.Log("Start authorize [{0}], state=[{1}]", _httpContextAccessor.HttpContext.Request.Path, state);
            var redirectUrl = GetRedirectAuthorizeUrl(state);
            OAuthLog.Log("Authorize [{0}], start redirect=[{1}]", _httpContextAccessor.HttpContext.Request.Path, redirectUrl);
            _httpContextAccessor.HttpContext.Response.Redirect(GetRedirectAuthorizeUrl(state));
        }

        /// <summary>
        /// 虚方法，获取 AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual TAccessToken GetAccessToken(string code, string state)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new OAuthException("请确保在 Web 环境下使用！（HttpContext 为 null）");
            }
            OAuthLog.Log("Start GetAccessToken code=[{0}], state=[{1}]", code, state);
            if (string.IsNullOrEmpty(code))
            {
                throw new OAuthException("缺少 code 参数！");
            }
            if (EnableStateCheck)
            {
                if (string.IsNullOrEmpty(state) || !OAuthStateManager.IsStateExist(state))
                {
                    throw OAuthStateManager.NoCSRF();
                }
                OAuthStateManager.RemoveState(_httpContextAccessor.HttpContext, state);
            }
            var serverResponse = API.Post(GetAccessTokenUrl(code, state));
            return ResolveAccessTokenFromString(serverResponse);
        }

        /// <summary>
        /// 获取用户信息，默认未实现，返回 null
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual TUserInfo GetUserInfo(TAccessToken accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
