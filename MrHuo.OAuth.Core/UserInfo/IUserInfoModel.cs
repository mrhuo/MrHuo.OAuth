namespace MrHuo.OAuth
{
    /// <summary>
    /// 用户信息。默认提供姓名和头像
    /// </summary>
    public interface IUserInfoModel: IApiErrorModel
    {
        /// <summary>
        /// 昵称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        string Avatar { get; set; }
    }
}
