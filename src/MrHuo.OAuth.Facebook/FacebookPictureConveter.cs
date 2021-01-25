using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using static MrHuo.OAuth.Facebook.FacebookUserInfoModel;

namespace MrHuo.OAuth.Facebook
{
    public class FacebookPictureConveter : JsonConverter<PictureInfo>
    {


        public override PictureInfo Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<PictureInfo>(ref reader);
        }


        public override void Write(Utf8JsonWriter writer, PictureInfo value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value);
        }
    }
}
