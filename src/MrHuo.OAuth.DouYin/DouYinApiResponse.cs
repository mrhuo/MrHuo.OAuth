using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.DouYin
{
    class DouYinApiResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
