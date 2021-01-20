using System;
using System.Security.Cryptography;
using System.Text;

namespace MrHuo.OAuth.DingTalkQrcode
{
    /// <summary>
    /// https://ding-doc.dingtalk.com/document/app/signature-calculation-for-logon-free-scenarios-1#topic-2021698
    /// </summary>
    public class DingTalkSignTool
    {
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// 获取 Timestamp
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            return $"{(long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds}";
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string Sign(string timestamp, string appSecret)
        {
            return HmacSHA256(timestamp, appSecret);
        }

        /// <summary>
        /// HmacSHA256 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HmacSHA256(string str, string key)
        {
            var keyBytes = DefaultEncoding.GetBytes(key);
            var strBytes = DefaultEncoding.GetBytes(str);
            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                var hashmessage = hmacsha256.ComputeHash(strBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public static String UrlEncode(string value)
        {
            if (value == null)
            {
                return "";
            }
            String encoded = System.Web.HttpUtility.UrlEncode(value, DefaultEncoding);
            return encoded.Replace("+", "%20").Replace("*", "%2A")
                          .Replace("~", "%7E").Replace("/", "%2F");
        }
    }
}
