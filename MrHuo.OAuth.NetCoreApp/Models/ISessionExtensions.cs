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
        public static void Set(this ISession Session, string key, object data)
        {
            if (data == null)
            {
                return;
            }
            Session.SetString(key, JsonSerializer.Serialize(data));
        }

        public static T Get<T>(this ISession Session, string key) 
        {
            var json = Session.GetString(key);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
