using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.KuaiShou
{
    class KuaiShouApiUserInfoResponse<T>
    {
        /// <summary>
        /// 错误码。非1 表示失败
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }


        [JsonPropertyName("user_info")]
        public T Data { get; set; }
    }
}
