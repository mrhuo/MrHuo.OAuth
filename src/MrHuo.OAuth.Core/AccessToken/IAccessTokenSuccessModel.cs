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
        /// <para>2021.1.9 修改为动态类型，因为 coding.net 中过期时间为字符串，非整数</para>
        /// </summary>
        dynamic ExpiresIn { get; set; }
    }
}
