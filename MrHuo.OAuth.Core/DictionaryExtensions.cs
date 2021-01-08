using System.Collections.Generic;
using System.Linq;

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
        /// <returns></returns>
        public static string ToQueryString(this Dictionary<string, string> dict)
        {
            return string.Join("&", dict.Select(p => $"{p.Key}={p.Value}"));
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
