using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.Mi
{
    public class MiApiResponse<T>
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        public bool HasError()
        {
            return Code != 0;
        }
    }
}
