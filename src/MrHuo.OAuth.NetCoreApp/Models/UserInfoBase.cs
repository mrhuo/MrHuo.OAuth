using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MrHuo.OAuth.NetCoreApp
{
    public class UserInfoBase
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }

    public static class IUserInfoModelExtensions
    {
        public static UserInfoBase ToUserInfoBase(this IUserInfoModel userInfoModel)
        {
            return new UserInfoBase()
            {
                Name = userInfoModel.Name,
                Avatar = userInfoModel.Avatar
            };
        }
    }
}
