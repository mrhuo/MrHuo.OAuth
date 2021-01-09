namespace MrHuo.OAuth
{
    /// <summary>
    /// AccessToken 类扩展方法
    /// </summary>
    public static class IAccessTokenModelModelExtensions
    {
        /// <summary>
        /// 是否包含错误
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        public static bool HasError(this IAccessTokenModel accessTokenModel)
        {
            return string.IsNullOrEmpty(accessTokenModel.AccessToken) || 
                   (    
                        !string.IsNullOrEmpty(accessTokenModel.Error) &&
                        !string.IsNullOrEmpty(accessTokenModel.ErrorDescription)
                   );
        }
    }
}
