namespace MrHuo.OAuth
{
    /// <summary>
    /// 调用 Authorize API 回调地址参数匹配类
    /// </summary>
    public class AuthorizeCallbackModel
    {
        public string Code { get; set; }
        public string State { get; set; }
    }
}
