namespace MrHuo.OAuth
{
    /// <summary>
    /// 统一授权接口
    /// </summary>
    internal interface IOAuthApi<TAccessToken, TUserInfo>
         where TAccessToken : IAccessTokenModel
         where TUserInfo : IUserInfoModel
    {
        /// <summary>
        /// 获取授权跳转URL
        /// </summary>
        /// <returns></returns>
        string GetAuthorizeUrl(string state);
        /// <summary>
        /// 获取 AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        TAccessToken GetAccessToken(string code, string state);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        TUserInfo GetUserInfo(TAccessToken accessToken);
    }
}
