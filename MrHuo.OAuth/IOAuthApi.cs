namespace MrHuo.OAuth
{
    /// <summary>
    /// 统一授权接口
    /// </summary>
    /// <typeparam name="TAccessToken"></typeparam>
    /// <typeparam name="TUserInfo"></typeparam>
    internal interface IOAuthApi<TAccessToken, TUserInfo>
         where TAccessToken : IAccessTokenModel
         where TUserInfo : IUserInfoModel
    {
        /// <summary>
        /// 执行授权
        /// </summary>
        void Authorize();
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
