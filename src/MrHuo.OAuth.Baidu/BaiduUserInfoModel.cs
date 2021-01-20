using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Baidu
{
    public class BaiduUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("userid")]
        public int UserId { get; set; }

        [JsonPropertyName("username")]
        public string Name { get; set; }

        [JsonPropertyName("realname")]
        public string RealName { get; set; }

        /// <summary>
        /// http://developer.baidu.com/wiki/index.php?title=docs/oauth/rest/file_data_apis_list#.E8.8E.B7.E5.8F.96.E5.BD.93.E5.89.8D.E7.99.BB.E5.BD.95.E7.94.A8.E6.88.B7.E7.9A.84.E7.AE.80.E5.8D.95.E4.BF.A1.E6.81.AF
        /// </summary>
        private string _avatar = "";
        [JsonPropertyName("portrait")]
        public string Avatar
        {
            get
            {
                if (string.IsNullOrEmpty(_avatar))
                {
                    return "";
                }
                return $"http://tb.himg.baidu.com/sys/portrait/item/{_avatar}";
            }
            set { _avatar = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonPropertyName("error_msg")]
        public string ErrorMessage { get; set; }
    }
}
