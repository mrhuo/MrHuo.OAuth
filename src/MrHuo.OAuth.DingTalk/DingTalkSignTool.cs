using System;
using System.Security.Cryptography;
using System.Text;

namespace MrHuo.OAuth.DingTalk
{
    /// <summary>
    /// https://ding-doc.dingtalk.com/document/app/signature-calculation-method-for-third-party-access-interfaces-1#topic-2021697
    /// </summary>
    public class DingTalkSignTool
    {
        /// <summary>
        /// 获取 Timestamp
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            //621355968000000000
            //计算方法 long ticks = (new DateTime(1970, 1, 1, 8, 0, 0)).ToUniversalTime().Ticks;
            return $"{(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000}";
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="appSecret"></param>
        /// <param name="suiteTicket">钉钉给应用推送的ticket，测试应用随意填写如：TestSuiteTicket，正式应用需要从推送回调获取suiteTicket</param>
        /// <returns></returns>
        public static string Sign(string timestamp, string appSecret, string suiteTicket = "TestSuiteTicket")
        {
            //1、把timestamp+"\n"+suiteTicket当做签名字符串
            var strToSign = $"{timestamp}\n{suiteTicket}";
            //2、suiteSecret/customSecret做为签名密钥，使用HmacSHA256算法计算签名
            var sign = HmacSHA256(strToSign, appSecret);
            //3、然后把签名参数再进行urlEncode
            return System.Web.HttpUtility.UrlEncode(sign);
        }

        /// <summary>
        /// HmacSHA256 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HmacSHA256(string str, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var strBytes = Encoding.UTF8.GetBytes(str);
            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                var hashmessage = hmacsha256.ComputeHash(strBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
