namespace MrHuo.OAuth
{
    public static class IUserInfoModelExtensions
    {
        /// <summary>
        /// 判断获取到的用户信息是否包含错误
        /// <para>用户信息为 null 或用户姓名为空，则认为是获取失败</para>
        /// </summary>
        /// <param name="userInfoModel"></param>
        /// <returns></returns>
        public static bool HasError(this IUserInfoModel userInfoModel)
        {
            return userInfoModel == null || string.IsNullOrEmpty(userInfoModel.Name);
        }
    }
}
