using System.Threading.Tasks;

namespace MrHuo.OAuth
{
    /// <summary>
    /// OAuth 登录 API 接口规范
    /// </summary>
    public interface IOAuthLoginApi<TAccessTokenModel, TUserInfoModel>
        where TAccessTokenModel : IAccessTokenModel
        where TUserInfoModel : IUserInfoModel
    {
        /// <summary>
        /// 获取跳转授权的 URL
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        string GetAuthorizeUrl(string state = "");

        /// <summary>
        /// 异步获取 AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<TAccessTokenModel> GetAccessTokenAsync(string code, string state = "");

        /// <summary>
        /// 异步获取用户详细信息
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        Task<TUserInfoModel> GetUserInfoAsync(TAccessTokenModel accessTokenModel);
    }
}
