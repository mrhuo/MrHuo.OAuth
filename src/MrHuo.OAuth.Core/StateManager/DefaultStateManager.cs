using System;
using System.Collections.Concurrent;

namespace MrHuo.OAuth
{
    public class DefaultStateManager : IStateManager
    {
        private static readonly ConcurrentDictionary<string, DateTime> authStates = new ConcurrentDictionary<string, DateTime>();
        private static DefaultStateManager _instance = null;
        private static readonly object lockObj = new object();

        public static IStateManager Instance()
        {
            lock (lockObj)
            {
                if (_instance == null)
                {
                    _instance = new DefaultStateManager();
                }
                return _instance;
            }
        }

        public bool IsStateExists(string state)
        {
            if (!authStates.ContainsKey(state))
            {
                return false;
            }
            var stateTime = authStates[state];
            if (DateTime.Now.Subtract(stateTime).TotalMinutes > 3)
            {
                authStates.TryRemove(state, out _);
                return false;
            }
            return true;
        }

        public void RemoveState(string state)
        {
            authStates.TryRemove(state, out _);
        }

        public string RequestState()
        {
            var state = Guid.NewGuid().ToString("N");
            authStates.TryAdd(state, DateTime.Now);
            return state;
        }
    }
}
