using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MrHuo.OAuth
{
    /// <summary>
    /// 抽象的 OAuth 基类，使用默认的 DefaultAccessTokenModel 作为 AccessToken 模型
    /// <para>如果需要其他字段，请自行继承 OAuthLoginBase&lt;TAccessTokenModel, TUserInfoModel&gt;</para>
    /// </summary>
    /// <typeparam name="TUserInfoModel"></typeparam>
    public abstract class OAuthLoginBase<TUserInfoModel> : OAuthLoginBase<DefaultAccessTokenModel, TUserInfoModel>
        where TUserInfoModel : IUserInfoModel
    {
        public OAuthLoginBase(OAuthConfig oauthConfig) : base(oauthConfig)
        {
        }
    }

    /// <summary>
    /// 抽象的 OAuth 基类
    /// </summary>
    /// <typeparam name="TUserInfoModel"></typeparam>
    public abstract class OAuthLoginBase<TAccessTokenModel, TUserInfoModel> : IOAuthLoginApi<TAccessTokenModel, TUserInfoModel>
        where TAccessTokenModel : IAccessTokenModel
        where TUserInfoModel : IUserInfoModel
    {
        protected readonly OAuthConfig oauthConfig;
        public OAuthLoginBase(OAuthConfig oauthConfig)
        {
            this.oauthConfig = oauthConfig;
        }

        /// <summary>
        /// 授权 URL
        /// </summary>
        protected abstract string AuthorizeUrl { get; }

        /// <summary>
        /// AccessToken URL
        /// </summary>
        protected abstract string AccessTokenUrl { get; }

        /// <summary>
        /// 用户信息 URL
        /// </summary>
        protected abstract string UserInfoUrl { get; }

        #region [默认实现]
        /// <summary>
        /// 构造 AuthorizeUrl
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> BuildAuthorizeParams(string state)
        {
            return new Dictionary<string, string>()
            {
                ["response_type"] = "code",
                ["client_id"] = $"{oauthConfig.AppId}",
                ["redirect_uri"] = $"{oauthConfig.RedirectUri}",
                ["scope"] = $"{oauthConfig.Scope}",
                ["state"] = $"{state}"
            };
        }

        /// <summary>
        /// 构造请求 AccessToken 参数
        /// </summary>
        /// <param name="authorizeCallbackParams">Callback 页面的请求参数</param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> BuildGetAccessTokenParams(Dictionary<string, string> authorizeCallbackParams)
        {
            return new Dictionary<string, string>()
            {
                ["grant_type"] = "authorization_code",
                ["code"] = $"{authorizeCallbackParams["code"]}",
                ["client_id"] = $"{oauthConfig.AppId}",
                ["client_secret"] = $"{oauthConfig.AppKey}",
                ["redirect_uri"] = $"{oauthConfig.RedirectUri}",
                //["state"] = $"{authorizeCallbackParams["state"]}"
            };
        }

        /// <summary>
        /// 构造获取用户信息参数
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> BuildGetUserInfoParams(TAccessTokenModel accessTokenModel)
        {
            return new Dictionary<string, string>()
            {
                ["access_token"] = accessTokenModel.AccessToken
            };
        }

        /// <summary>
        /// 获取 AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual Task<TAccessTokenModel> GetAccessTokenAsync(string code, string state = "")
        {
            return GetAccessTokenAsync(new Dictionary<string, string>()
            {
                ["code"] = code,
                ["state"] = state
            });
        }

        /// <summary>
        /// 获取 AccessToken
        /// </summary>
        /// <param name="authorizeCallbackParams">Callback 页面的请求参数</param>
        /// <returns></returns>
        public virtual async Task<TAccessTokenModel> GetAccessTokenAsync(Dictionary<string, string> authorizeCallbackParams)
        {
            var accessTokenModel = await HttpRequestApi.PostAsync<TAccessTokenModel>(
                AccessTokenUrl,
                BuildGetAccessTokenParams(authorizeCallbackParams)
            );
            if (accessTokenModel.HasError())
            {
                throw new Exception(accessTokenModel.ErrorDescription);
            }
            return accessTokenModel;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        public virtual async Task<TUserInfoModel> GetUserInfoAsync(TAccessTokenModel accessTokenModel)
        {
            var userInfoModel = await HttpRequestApi.GetAsync<TUserInfoModel>(
                UserInfoUrl, 
                BuildGetUserInfoParams(accessTokenModel)
            );
            if (userInfoModel.HasError())
            {
                throw new Exception(userInfoModel.ErrorMessage);
            }
            return userInfoModel;
        }

        /// <summary>
        /// 构造一个用于跳转授权的 URL
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual string GetAuthorizeUrl(string state = "")
        {
            var param = BuildAuthorizeParams(state);
            param.RemoveEmptyValueItems();
            return $"{AuthorizeUrl}?{param.ToQueryString()}";
        }

        /// <summary>
        /// Authorize 回调方法，返回已授权用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual Task<AuthorizeResult<TAccessTokenModel, TUserInfoModel>> AuthorizeCallback(string code, string state = "")
        {
            return AuthorizeCallback(new Dictionary<string, string>()
            {
                ["code"] = code,
                ["state"] = state
            });
        }

        /// <summary>
        /// Authorize 回调方法，返回已授权用户信息
        /// </summary>
        /// <param name="authorizeCallbackParams">Callback 页面的请求参数</param>
        /// <returns></returns>
        public virtual async Task<AuthorizeResult<TAccessTokenModel, TUserInfoModel>> AuthorizeCallback(Dictionary<string, string> authorizeCallbackParams)
        {
            try
            {
                var accessTokenModel = await GetAccessTokenAsync(authorizeCallbackParams);
                var userInfoModel = await GetUserInfoAsync(accessTokenModel);
                return AuthorizeResult<TAccessTokenModel, TUserInfoModel>.Ok(accessTokenModel, userInfoModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return AuthorizeResult<TAccessTokenModel, TUserInfoModel>.Error(ex);
            }
        }
        #endregion
    }
}
