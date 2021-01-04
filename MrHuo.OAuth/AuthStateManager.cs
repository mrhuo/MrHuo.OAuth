using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MrHuo.OAuth
{
    /// <summary>
    /// 状态管理器
    /// </summary>
    internal class OAuthStateManager
    {
        /// <summary>
        /// 内部用于保存状态的列表
        /// </summary>
        private static ConcurrentDictionary<string, string> oauthStates = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 获取授权状态
        /// </summary>
        /// <param name="httpContext">当前请求上下文</param>
        /// <param name="type"></param>
        /// <returns>AuthState</returns>
        public static string RequestState(HttpContext httpContext, Type type)
        {
            //加入线程锁，保证每次获取状态都正确
            var sessionId = httpContext.Session.Id;
            var sessionKey = $"OAuthState_{type.Name}_{sessionId}";
            OAuthLog.Log("Start request state for [{0}], state key=[{1}]", type.FullName, sessionKey);
            try
            {
                //如果同一sessionId和登录平台有未处理完成的状态，就移除
                var exists = oauthStates.Where((p) => p.Key == sessionKey).ToArray();
                if (exists.Count() > 0)
                {
                    OAuthLog.Log("Exists state [{0}], removing...", exists.Count());
                    for (int i = 0; i < exists.Count(); i++)
                    {
                        oauthStates.TryRemove(sessionKey, out var _);
                    }
                }
            }
            catch (Exception)
            {
            }

            //生成新的状态，此状态会在登录完成后移除
            var state = Guid.NewGuid().ToString("N");
            OAuthLog.Log("Generated new state [{0}]", state);
            oauthStates.TryAdd(sessionKey, state);
            OAuthLog.Log("Added new state to oauthStates, count=[{0}]", oauthStates.Count);
            httpContext.Session.Set(sessionKey, Encoding.UTF8.GetBytes(state));
            OAuthLog.Log("Added new state to session=[{0}]", sessionKey);
            return state;
        }

        /// <summary>
        /// 是否存在此状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsStateExist(string state)
        {
            return oauthStates.Values.Contains(state);
        }

        /// <summary>
        /// 在登录操作完毕之后会移除此状态
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="state"></param>
        public static void RemoveState(HttpContext httpContext, string state)
        {
            var sessionId = httpContext.Session.Id;
            var sessionKey = oauthStates.FirstOrDefault(p => p.Value == state).Key;
            if (string.IsNullOrEmpty(sessionKey))
            {
                throw NoCSRF();
            }
            else
            {
                oauthStates.TryRemove(sessionKey, out var _);
                OAuthLog.Log("Remove state [{0}] success.", state);
                httpContext.Session.Remove(sessionKey);
                OAuthLog.Log("Remove session [{0}] success.", sessionKey);
            }
        }

        /// <summary>
        /// 清空状态集合，用于OAuth类销毁之后清理资源
        /// </summary>
        public static void Clear()
        {
            oauthStates.Clear();
        }

        /// <summary>
        /// 统一 CSRF 异常
        /// </summary>
        /// <returns></returns>
        public static OAuthException NoCSRF()
        {
            OAuthLog.Log("Trigger an CSRF exception.");
            return new OAuthException("禁止跨站请求伪造（CSRF）攻击！");
        }
    }
}
