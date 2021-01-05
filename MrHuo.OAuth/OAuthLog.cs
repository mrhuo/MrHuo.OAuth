using System;

namespace MrHuo.OAuth
{
    /// <summary>
    /// 用于 OAuth 过程调试，可使用：OAuthLog.Enabled = true; 来开启日志。日志会输出到控制台
    /// </summary>
    public static class OAuthLog
    {
        public static bool Enabled { get; set; } = false;
        public static void Log(string message, params object[] param)
        {
            if (Enabled)
            {
                Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}] {string.Format(message, param)}.");
            }
        }

        public static void Log(Exception ex)
        {
            Log(ex.ToString());
        }
    }
}
