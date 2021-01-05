using System;

namespace MrHuo.OAuth
{

    [Serializable]
    public class OAuthException : Exception
    {
        public OAuthException(string message) : this(message, null) { }
        public OAuthException(string message, Exception inner) : base(message, inner) { }
    }
}
