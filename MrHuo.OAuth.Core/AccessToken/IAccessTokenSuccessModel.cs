namespace MrHuo.OAuth
{
    /// <summary>
    /// 参考 https://tools.ietf.org/html/rfc6749#section-5.1 提供默认属性
    /// </summary>
    public interface IAccessTokenSuccessModel
    {
        /// <summary>
        /// Token 类型
        /// </summary>
        string TokenType { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// 用于刷新 AccessToken 的 Token
        /// </summary>
        string RefreshToken { get; set; }

        /// <summary>
        /// 此 AccessToken 对应的权限
        /// </summary>
        string Scope { get; set; }

        /// <summary>
        /// AccessToken 过期时间
        /// </summary>
        int ExpiresIn { get; set; }
    }
}
