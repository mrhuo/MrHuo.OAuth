using System.Text.Json.Serialization;

namespace MrHuo.OAuth
{
    /// <summary>
    /// 默认的 AccessToken 模型实现类
    /// </summary>
    public class DefaultAccessTokenModel : IAccessTokenModel
    {
        /// <summary>
        /// Token 类型
        /// </summary>
        [JsonPropertyName("token_type")]
        public virtual string TokenType { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        [JsonPropertyName("access_token")]
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// 用于刷新 AccessToken 的 Token
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public virtual string RefreshToken { get; set; }

        /// <summary>
        /// 此 AccessToken 对应的权限
        /// </summary>
        [JsonPropertyName("scope")]
        public virtual string Scope { get; set; }

        /// <summary>
        /// AccessToken 过期时间
        /// </summary>
        [JsonPropertyName("expires_in")]
        public virtual int ExpiresIn { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        [JsonPropertyName("error")]
        public virtual string Error { get; set; }

        /// <summary>
        /// 错误的详细描述
        /// </summary>
        [JsonPropertyName("error_description")]
        public virtual string ErrorDescription { get; set; }
    }
}
