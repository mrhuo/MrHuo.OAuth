using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Alipay
{
    /// <summary>
    /// https://opendocs.alipay.com/open/284/web#%E5%90%8C%E6%AD%A5%E5%93%8D%E5%BA%94%E7%BB%93%E6%9E%9C%E7%A4%BA%E4%BE%8B_2
    /// </summary>
    public class AlipayUserInfoModel : IUserInfoModel
    {
        /// <summary>
        /// 支付宝用户的 Use_id
        /// </summary>
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户昵称。如果没有数据的时候不会返回该数据，请做好容错。
        /// </summary>
        [JsonPropertyName("nick_name")]
        public string Name { get; set; }

        /// <summary>
        /// 用户头像。如果没有数据的时候不会返回该数据，请做好容错。
        /// </summary>
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// 省份。用户注册时填写的省份 如果没有数据的时候不会返回该数据，请做好容错
        /// </summary>
        [JsonPropertyName("province")]
        public string Province { get; set; }

        /// <summary>
        /// 城市。用户注册时填写的城市， 如果没有数据的时候不会返回该数据，请做 好容错
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// 用户性别。M为男性，F为女性， 如果没有数据的时候不会返回该数据，请做好容错
        /// </summary>
        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("error_response")]
        public string ErrorResponse { get; set; }

        [JsonPropertyName("sub_code")]
        public string SubCode { get; set; }

        [JsonPropertyName("sub_msg")]
        public string SubMsg { get; set; }

        //没有实现
        public string ErrorMessage { get; set; }
    }
}
