using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MrHuo.OAuth.NetCoreApp
{
    public static class ISessionExtensions
    {
        public static void Set(this ISession Session, string key, object data, bool jsonIndentFormat = false)
        {
            if (data == null)
            {
                return;
            }
            var json = "";
            if (jsonIndentFormat)
            {
                json = JsonSerializer.Serialize(data, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
            }
            else
            {
                json = JsonSerializer.Serialize(data);
            }
            Session.SetString(key, json);
        }

        public static T Get<T>(this ISession Session, string key)
        {
            var json = Session.GetString(key);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
