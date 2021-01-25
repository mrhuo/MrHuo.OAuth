using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.StackOverflow
{
    public class StackOverflowApiResponse<T>
    {
        [JsonPropertyName("items")]
        public List<T> Items { get; set; }

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }

        [JsonPropertyName("quota_max")]
        public int QuotaMax { get; set; }

        [JsonPropertyName("quota_remaining")]
        public int QuotaRemaining { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }
    }

}
