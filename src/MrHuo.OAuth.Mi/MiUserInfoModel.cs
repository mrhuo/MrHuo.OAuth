using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Mi
{
    /// <summary>
    /// https://dev.mi.com/docs/passport/open-api/
    /// {
    /// 	"data": {
    /// 		"unionId": "n-cBnFC_10gI9yQwwn3swNz4jladyJ8m8O4OOJlg",
    /// 		"miliaoNick": "霍小平",
    /// 		"miliaoIcon": "https://cdn.cnbj1.fds.api.mi-img.com/user-avatar/p01TGEjgxsHV/hnzcmcCoju4bxd.jpg",
    /// 		"miliaoIcon_75": "https://cdn.cnbj1.fds.api.mi-img.com/user-avatar/p01TGEjgxsHV/hnzcmcCoju4bxd_75.jpg",
    /// 		"miliaoIcon_90": "https://cdn.cnbj1.fds.api.mi-img.com/user-avatar/p01TGEjgxsHV/hnzcmcCoju4bxd_90.jpg",
    /// 		"miliaoIcon_120": "https://cdn.cnbj1.fds.api.mi-img.com/user-avatar/p01TGEjgxsHV/hnzcmcCoju4bxd_120.jpg",
    /// 		"miliaoIcon_320": "https://cdn.cnbj1.fds.api.mi-img.com/user-avatar/p01TGEjgxsHV/hnzcmcCoju4bxd_320.jpg",
    /// 		"miliaoIcon_orig": "https://cdn.cnbj1.fds.api.mi-img.com/user-avatar/p01TGEjgxsHV/hnzcmcCoju4bxd.jpg"
    /// 	},
    /// 	"result": "ok",
    /// 	"code": 0,
    /// 	"description": "no error"
    /// }
    /// </summary>
    public class MiUserInfoModel : IUserInfoModel
    {
        /// <summary>
        /// 小米用户账号
        /// </summary>
        [JsonPropertyName("unionId")]
        public string UnionId { get; set; }

        /// <summary>
        /// 小米帐号昵称
        /// </summary>
        [JsonPropertyName("miliaoNick")]
        public string Name { get; set; }

        /// <summary>
        /// 头像URL(会返回多个分辨率版本的头像)
        /// </summary>
        [JsonPropertyName("miliaoIcon")]
        public string Avatar { get; set; }

        public string ErrorMessage { get; set; }
    }
}
