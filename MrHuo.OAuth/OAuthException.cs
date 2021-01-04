using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MrHuo.OAuth
{

    [Serializable]
    public class OAuthException : Exception
    {
        public OAuthException(string message) : this(message, null) { }
        public OAuthException(string message, Exception inner) : base(message, inner) { }
    }
}
