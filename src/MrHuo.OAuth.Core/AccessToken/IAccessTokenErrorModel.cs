namespace MrHuo.OAuth
{
    /// <summary>
    /// AccessToken 授权后错误模型类
    /// </summary>
    public interface IAccessTokenErrorModel
    {
        /// <summary>
        /// 错误的详细描述
        /// </summary>
        string ErrorDescription { get; set; }
    }
}
