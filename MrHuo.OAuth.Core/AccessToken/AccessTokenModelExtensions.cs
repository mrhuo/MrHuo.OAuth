namespace MrHuo.OAuth
{
    /// <summary>
    /// AccessToken 类扩展方法
    /// </summary>
    public static class IAccessTokenErrorModelExtensions
    {
        /// <summary>
        /// 是否包含错误
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        public static bool HasError(this IAccessTokenErrorModel accessTokenModel)
        {
            return !string.IsNullOrEmpty(accessTokenModel.Error) && !string.IsNullOrEmpty(accessTokenModel.ErrorDescription);
        }
    }
}
