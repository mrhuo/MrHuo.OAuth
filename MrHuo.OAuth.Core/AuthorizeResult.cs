using System;

namespace MrHuo.OAuth
{
    public class AuthorizeResult<TAccessTokenModel, TUserInfoModel>
        where TAccessTokenModel : IAccessTokenModel
        where TUserInfoModel : IUserInfoModel
    {
        public bool IsSccess { get; set; }
        public string ErrorMessage { get; set; }

        public TAccessTokenModel AccessToken { get; set; }
        public TUserInfoModel UserInfo { get; set; }

        public static AuthorizeResult<TAccessTokenModel, TUserInfoModel> Ok(TAccessTokenModel accessToken, TUserInfoModel userInfo)
        {
            return new AuthorizeResult<TAccessTokenModel, TUserInfoModel>()
            {
                IsSccess = true,
                ErrorMessage = null,
                AccessToken = accessToken,
                UserInfo = userInfo
            };
        }

        public static AuthorizeResult<TAccessTokenModel, TUserInfoModel> Error(string errorMessage)
        {
            return new AuthorizeResult<TAccessTokenModel, TUserInfoModel>()
            {
                IsSccess = false,
                ErrorMessage = errorMessage
            };
        }

        public static AuthorizeResult<TAccessTokenModel, TUserInfoModel> Error(Exception exception)
        {
            return new AuthorizeResult<TAccessTokenModel, TUserInfoModel>()
            {
                IsSccess = false,
                ErrorMessage = exception.Message
            };
        }
    }
}
