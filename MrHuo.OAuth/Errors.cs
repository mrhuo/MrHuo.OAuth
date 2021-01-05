namespace MrHuo.OAuth
{
    internal class Errors
    {
        public static OAuthException ForbidCSRFException()
        {
            return new OAuthException("禁止跨站请求伪造（CSRF）攻击！");
        }

        public static OAuthException ParameterMissing(string parameterName)
        {
            return new OAuthException($"缺少 {parameterName} 参数！");
        }
    }
}
