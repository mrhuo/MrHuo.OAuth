using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth
{
    /// <summary>
    /// OAuth 默认配置
    /// </summary>
    public class OAuthConfig
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Secret key
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// 权限范围
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 从 IConfiguration 中读取
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static OAuthConfig LoadFrom(IConfiguration configuration, string prefix)
        {
            return With(
                appId: configuration[prefix + ":app_id"],
                appKey: configuration[prefix + ":app_key"],
                redirectUri: configuration[prefix + ":redirect_uri"],
                scope: configuration[prefix + ":scope"]
            );
        }

        public static OAuthConfig With(string appId, string appKey, string redirectUri, string scope)
        {
            return new OAuthConfig()
            {
                AppId = appId,
                AppKey = appKey,
                RedirectUri = redirectUri,
                Scope = scope
            };
        }
    }
}
