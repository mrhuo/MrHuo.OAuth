using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.DingTalkQrcode
{
    /// <summary>
    /// https://ding-doc.dingtalk.com/document/app/obtain-the-user-information-based-on-the-sns-temporary-authorization#topic-1995619
    /// </summary>
    public class DingTalkUserInfoModel: IUserInfoModel
    {
        /// <summary>
        /// 用户主企业是否达到高级认证级别。
        /// </summary>
        [JsonPropertyName("main_org_auth_high_level")]
        public bool MainOrgAuthHighLevel { get; set; }

        /// <summary>
        /// 用户在钉钉上面的昵称。
        /// </summary>
        [JsonPropertyName("nick")]
        public string Name { get; set; }

        /// <summary>
        /// 用户在当前开放应用所属企业的唯一标识。
        /// </summary>
        [JsonPropertyName("unionid")]
        public string UnionId { get; set; }

        /// <summary>
        /// 用户在当前开放应用内的唯一标识。
        /// </summary>
        [JsonPropertyName("openid")]
        public string OpenId { get; set; }

        public string Avatar { get; set; } = "";
        public string ErrorMessage { get; set; }
    }


    internal class DingTalkGetUserInfoApiResponse
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("errmsg")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("user_info")]
        public DingTalkUserInfoModel UserInfo { get; set; }

        public bool HasError()
        {
            return ErrorCode != 0 && ErrorMessage != "ok" || UserInfo == null;
        }
    }
}
