using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Huawei
{
    /// <summary>
    /// 当应用有获取头像、手机号、服务地国家、注册地、生日、年龄段、邮箱权限后才返回对应信息，获取权限请参见帐号开放信息获取流程。
    /// </summary>
    public class HuaweiUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("openID")]
        public string OpenId { get; set; }

        [JsonPropertyName("displayName")]
        public string Name { get; set; }

        [JsonPropertyName("headPictureURL")]
        public string Avatar { get; set; }

        [JsonPropertyName("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonPropertyName("srvNationalCode")]
        public string SrvNationalCode { get; set; }

        [JsonPropertyName("nationalCode")]
        public string NationalCode { get; set; }

        [JsonPropertyName("birthDate")]
        public string BirthDate { get; set; }

        /// <summary>
        /// 年龄段。
        /// -1：年龄未知（没输入生日） 。
        /// 0：成人。
        /// 1：未成人，介于儿童和成人之间。
        /// 2：儿童。
        /// </summary>
        [JsonPropertyName("ageGroupFlag")]
        public int AgeGroupFlag { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
