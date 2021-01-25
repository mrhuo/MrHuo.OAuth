using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.OAuth
{
    /// <summary>
    /// 字典扩展方法
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 将一个字典转化为 QueryString
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="urlEncode"></param>
        /// <returns></returns>
        public static string ToQueryString(this Dictionary<string, string> dict, bool urlEncode = true)
        {
            return string.Join("&", dict.Select(p => $"{(urlEncode ? p.Key?.UrlEncode() : "")}={(urlEncode ? p.Value?.UrlEncode() : "")}"));
        }

        /// <summary>
        /// 将一个字符串 URL 编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return System.Web.HttpUtility.UrlEncode(str, Encoding.UTF8);
        }

        /// <summary>
        /// 移除空值项
        /// </summary>
        /// <param name="dict"></param>
        public static void RemoveEmptyValueItems(this Dictionary<string, string> dict)
        {
            dict.Where(item => string.IsNullOrEmpty(item.Value)).Select(item => item.Key).ToList().ForEach(key =>
            {
                dict.Remove(key);
            });
        }
    }
}
