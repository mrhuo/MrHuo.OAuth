using System.Text.Json.Serialization;

namespace MrHuo.OAuth.QQ
{
    /// <summary>
    /// https://wiki.open.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91get_user_info#3.4.E8.BF.94.E5.9B.9E.E5.8F.82.E6.95.B0.E8.AF.B4.E6.98.8E
    /// </summary>
    public class QQUserInfoModel : IUserInfoModel
    {
        /// <summary>
        /// 返回码
        /// </summary>
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        /// <summary>
        /// 如果ret<0，会有相应的错误信息提示，返回数据全部用UTF-8编码。
        /// </summary>
        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        /// <summary>
        /// 用户在QQ空间的昵称。
        /// </summary>
        [JsonPropertyName("nickname")]
        public string Name { get; set; }

        /// <summary>
        /// 大小为40×40像素的QQ头像URL。一定会有。
        /// </summary>
        [JsonPropertyName("figureurl_qq_1")]
        public string Avatar { get; set; }

        /// <summary>
        /// Qzone 30px 头像
        /// </summary>
        [JsonPropertyName("figureurl")]
        public string Qzone30Avatar { get; set; }

        /// <summary>
        /// Qzone 50px 头像
        /// </summary>
        [JsonPropertyName("figureurl_1")]
        public string Qzone50Avatar { get; set; }

        /// <summary>
        /// Qzone 100px 头像
        /// </summary>
        [JsonPropertyName("figureurl_2")]
        public string Qzone100Avatar { get; set; }

        /// <summary>
        /// 大小为40×40像素的QQ头像URL。
        /// </summary>
        public string QQAvatar { get { return Avatar; } }

        /// <summary>
        /// 大小为100×100像素的QQ头像URL。需要注意，不是所有的用户都拥有QQ的100x100的头像，但40x40像素则是一定会有。
        /// </summary>
        [JsonPropertyName("figureurl_qq_2")]
        public string QQ100Avatar { get; set; }

        /// <summary>
        /// 性别。 如果获取不到则默认返回"男"
        /// </summary>
        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// 标识用户是否为黄钻用户（0：不是；1：是）。
        /// </summary>
        [JsonPropertyName("is_yellow_vip")]
        public string IsYellowVip { get; set; }

        /// <summary>
        /// 标识用户是否为 VIP 用户（0：不是；1：是）
        /// </summary>
        [JsonPropertyName("vip")]
        public string Vip { get; set; }

        /// <summary>
        /// 黄钻等级
        /// </summary>
        [JsonPropertyName("yellow_vip_level")]
        public string YellowVipLevel { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [JsonPropertyName("level")]
        public string Level { get; set; }

        /// <summary>
        /// 标识是否为年费黄钻用户（0：不是； 1：是）
        /// </summary>
        [JsonPropertyName("is_yellow_year_vip")]
        public string IsYellowYearVip { get; set; }
    }
}
