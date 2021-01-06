using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Baidu
{
    /// <summary>
    /// http://developer.baidu.com/wiki/index.php?title=docs/oauth/authorization
    /// </summary>
    public class BaiduAccessTokenModel : IAccessTokenModel
    {
        /// <summary>
        /// 用于刷新Access Token 的 Refresh Token,所有应用都会返回该参数；（10年的有效期）
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 要获取的Access Token；
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 基于http调用Open API时所需要的Session Key，其有效期与Access Token一致；
        /// </summary>
        [JsonPropertyName("session_key")]
        public string SessionKey { get; set; }

        /// <summary>
        /// 基于http调用Open API时计算参数签名用的签名密钥。
        /// </summary>
        [JsonPropertyName("session_secret")]
        public string SessionSecret { get; set; }

        /// <summary>
        /// Access Token的有效期，以秒为单位；
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Access Token最终的访问范围，即用户实际授予的权限列表（用户在授权页面时，有可能会取消掉某些请求的权限）
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
