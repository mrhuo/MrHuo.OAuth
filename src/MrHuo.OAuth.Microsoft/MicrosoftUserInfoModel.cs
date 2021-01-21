using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Microsoft
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/graph/api/user-get?view=graph-rest-1.0&tabs=http#code-try-4
    /// </summary>
    public class MicrosoftUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        public string Name { get; set; }

        [JsonPropertyName("userPrincipalName")]
        public string Email { get; set; }

        /// <summary>
        /// 没有返回头像
        /// </summary>
        public string Avatar { get; set; } = "";
        public string ErrorMessage { get; set; }

        [JsonPropertyName("userPrincipalName")]
        public MicrosoftApiErrorResponse Error { get; set; }
    }

    public class MicrosoftApiErrorResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        /*
         "error": {
    "code": "InvalidAuthenticationToken",
    "message": "Access token is empty.",
    "innerError": {
    "date": "2021-01-21T14:18:28",
    "request-id": "1fb2ecff-8961-47bf-9a8d-0c26ce4b7b06",
    "client-request-id": "1fb2ecff-8961-47bf-9a8d-0c26ce4b7b06"
    }
    }
         */
    }
}
