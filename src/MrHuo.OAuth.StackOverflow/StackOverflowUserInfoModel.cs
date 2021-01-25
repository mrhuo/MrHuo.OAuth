using System;
using System.Text.Json.Serialization;

namespace MrHuo.OAuth.StackOverflow
{
    /// <summary>
    /// https://api.stackexchange.com/docs/types/user
    /// </summary>
    public class StackOverflowUserInfoModel : IUserInfoModel
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("account_id")]
        public int AccountId { get; set; }

        [JsonPropertyName("display_name")]
        public string Name { get; set; }

        [JsonPropertyName("profile_image")]
        public string Avatar { get; set; }

        [JsonPropertyName("is_employee")]
        public bool IsEmployee { get; set; }

        [JsonPropertyName("last_access_date")]
        public int LastAccessDate { get; set; }

        [JsonPropertyName("creation_date")]
        public int CreationDate { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("reputation")]
        public int Reputation { get; set; }

        [JsonPropertyName("reputation_change_year")]
        public int ReputationChangeYear { get; set; }

        [JsonPropertyName("reputation_change_quarter")]
        public int ReputationChangeQuarter { get; set; }

        [JsonPropertyName("reputation_change_month")]
        public int ReputationChangeMonth { get; set; }

        [JsonPropertyName("reputation_change_week")]
        public int ReputationChangeWeek { get; set; }

        [JsonPropertyName("reputation_change_day")]
        public int ReputationChangeDay { get; set; }
        public string ErrorMessage { get; set; }
    }
}
