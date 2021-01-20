using System.Text.Json.Serialization;

namespace MrHuo.OAuth.SinaWeibo
{
    public class SinaWeiboAccessTokenModel: DefaultAccessTokenModel
    {
        /// <summary>
        /// Token 类型
        /// </summary>
        [JsonPropertyName("uid")]
        public virtual string Uid { get; set; }
    }
}
