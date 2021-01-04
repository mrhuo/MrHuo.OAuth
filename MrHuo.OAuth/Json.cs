using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MrHuo.OAuth
{
    public class Json
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        public static T Deserialize<T>(string json, JsonSerializerOptions options = null)
        {
            return JsonSerializer.Deserialize<T>(json, options ?? DefaultJsonOptions);
        }
        public static string Serialize(object obj, JsonSerializerOptions options = null)
        {
            return JsonSerializer.Serialize(obj, options ?? DefaultJsonOptions);
        }
    }
}