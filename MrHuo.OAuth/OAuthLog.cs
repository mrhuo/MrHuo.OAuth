using System;

namespace MrHuo.OAuth
{
    public static class OAuthLog
    {
        public static bool Enabled { get; set; } = false;
        public static void Log(string message, params object[] param)
        {
            if (Enabled)
            {
                Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {string.Format(message, param)}.");
            }
        }

        public static void Log(Exception ex)
        {
            Log(ex.ToString());
        }
    }
}
