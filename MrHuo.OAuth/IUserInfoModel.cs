using System;
using System.Collections.Generic;
using System.Text;

namespace MrHuo.OAuth
{
    /// <summary>
    /// UserInfo 基础模型接口
    /// </summary>
    public interface IUserInfoModel
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
