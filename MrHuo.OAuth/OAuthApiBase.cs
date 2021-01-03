using System;
using System.Collections.Generic;
using System.Text;
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
            return JsonSerializer.Deserialize<TAccessToken>(accessTokenResponse);
        }

        /// <summary>
        /// 虚方法，执行授权
        /// </summary>
        public virtual void Authorize()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new Exception("请确保在 Web 环境下使用！（HttpContext 为 null）");
            }
            var state = OAuthStateManager.RequestState(_httpContextAccessor.HttpContext, GetType());
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
                throw new Exception("请确保在 Web 环境下使用！（HttpContext 为 null）");
            }
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state) || !OAuthStateManager.IsStateExist(state))
            {
                throw OAuthStateManager.NoCSRF();
            }
            OAuthStateManager.RemoveState(_httpContextAccessor.HttpContext, GetType());
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
