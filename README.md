# MrHuo.OAuth

> 前置条件：.net core 项目或 .net framework 4.6 以上

快速接入 OAuth 登录。

体验网址：[https://oauthlogin.net/](https://oauthlogin.net/)

#### 已支持平台

- [x] 百度
- [x] 微信
- [x] Gitlab
- [x] Gitee
- [x] Github
- [x] 华为
...更多平台持续开发中

1.`Startup.cs`

```
public void ConfigureServices(IServiceCollection services)
{
    //将第三方登录组件注入进去
    services.AddSingleton(new Baidu.BaiduOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:baidu")));
    services.AddSingleton(new Wechat.WechatOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:wechat")));
    services.AddSingleton(new Gitlab.GitlabOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:gitlab")));
    services.AddSingleton(new Gitee.GiteeOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:gitee")));
    //... 其他登录方式
}
```

2.`OAuthController.cs` 根据实际需要自行命名

```
public class OAuthController : Controller
{
    [HttpGet("oauth/{type}")]
    public IActionResult Index(
        string type,
        [FromServices] BaiduOAuth baiduOAuth,
        [FromServices] WechatOAuth wechatOAuth
    )
    {
        var redirectUrl = "";
        switch (type.ToLower())
        {
            case "baidu":
                {
                    redirectUrl = baiduOAuth.GetAuthorizeUrl();
                    break;
                }
            case "wechat":
                {
                    redirectUrl = wechatOAuth.GetAuthorizeUrl();
                    break;
                }
            default:
                return ReturnToError($"没有实现【{type}】登录方式！");
        }
        return Redirect(redirectUrl);
    }

    [HttpGet("oauth/{type}callback")]
    public async Task<IActionResult> LoginCallback(
        string type,
        [FromServices] BaiduOAuth baiduOAuth,
        [FromServices] WechatOAuth wechatOAuth,
        [FromQuery] string code,
        [FromQuery] string state)
    {
        try
        {
            switch (type.ToLower())
            {
                case "baidu":
                    {
                        var authorizeResult = await baiduOAuth.AuthorizeCallback(code, state);
                        if (!authorizeResult.IsSccess)
                        {
                            throw new Exception(authorizeResult.ErrorMessage);
                        }
                        return Json(authorizeResult);
                    }
                case "wechat":
                    {
                        var authorizeResult = await wechatOAuth.AuthorizeCallback(code, state);
                        if (!authorizeResult.IsSccess)
                        {
                            throw new Exception(authorizeResult.ErrorMessage);
                        }
                        return Json(authorizeResult);
                    }
                default:
                    throw new Exception($"没有实现【{type}】登录回调！");
            }
        }
        catch (Exception ex)
        {
            return Content(ex.Message);
        }
    }
}
```

3.`Views`

```
<!--在代码中放置授权按钮-->
<a href="/oauth/baidu">Baidu 登录</a>
<a href="/oauth/wechat">Wechat 扫码登录</a>
<!-- //其他登录方式照样子往下写 -->
```


#### 扩展

扩展其他平台非常容易，拿 `Gitee` 平台的代码来说：[https://github.com/mrhuo/MrHuo.OAuth/tree/main/MrHuo.OAuth.Gitee](https://github.com/mrhuo/MrHuo.OAuth/tree/main/MrHuo.OAuth.Gitee)

##### 第一步：找平台对应 OAuth 文档，找到获取用户信息接口返回JSON，转换为 C# 实体类。如下：

> 根据自己需要和接口标准，扩展用户属性

```
public class GiteeUserModel : IUserInfoModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("avatar_url")]
    public string Avatar { get; set; }

    [JsonPropertyName("message")]
    public string ErrorMessage { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("blog")]
    public string Blog { get; set; }

    //...其他属性类似如上
}
```

##### 第二步：写对应平台的授权接口

```
/// <summary>
/// https://gitee.com/api/v5/oauth_doc#/
/// </summary>
public class GiteeOAuth : OAuthLoginBase<GiteeUserModel>
{
    public GiteeOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }
    protected override string AuthorizeUrl => "https://gitee.com/oauth/authorize";
    protected override string AccessTokenUrl => "https://gitee.com/oauth/token";
    protected override string UserInfoUrl => "https://gitee.com/api/v5/user";
}
```

加上注释，总共十行，如你所见，非常方便。如果该平台协议遵循 OAuth2 标准开发，那么就这么几行就好了。

就连修改字段的微信登录实现，也不过复杂，只需要定义基本参数就OK。代码如下：

```
/// <summary>
/// Wechat OAuth 相关文档参考：
/// <para>https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/Wechat_webpage_authorization.html</para>
/// </summary>
public class WechatOAuth : OAuthLoginBase<WechatAccessTokenModel, WechatUserInfoModel>
{
    public WechatOAuth(OAuthConfig oauthConfig) : base(oauthConfig) { }
    protected override string AuthorizeUrl => "https://open.weixin.qq.com/connect/oauth2/authorize";
    protected override string AccessTokenUrl => "https://api.weixin.qq.com/sns/oauth2/access_token";
    protected override string UserInfoUrl => "https://api.weixin.qq.com/sns/userinfo";
    protected override Dictionary<string, string> BuildAuthorizeParams(string state)
    {
        return new Dictionary<string, string>()
        {
            ["response_type"] = "code",
            ["appid"] = oauthConfig.AppId,
            ["redirect_uri"] = System.Web.HttpUtility.UrlEncode(oauthConfig.RedirectUri),
            ["scope"] = oauthConfig.Scope,
            ["state"] = state
        };
    }
    public override string GetAuthorizeUrl(string state = "")
    {
        return $"{base.GetAuthorizeUrl(state)}#wechat_redirect";
    }
    protected override Dictionary<string, string> BuildGetAccessTokenParams(Dictionary<string, string> authorizeCallbackParams)
    {
        return new Dictionary<string, string>()
        {
            ["grant_type"] = "authorization_code",
            ["appid"] = $"{oauthConfig.AppId}",
            ["secret"] = $"{oauthConfig.AppKey}",
            ["code"] = $"{authorizeCallbackParams["code"]}"
        };
    }
    protected override Dictionary<string, string> BuildGetUserInfoParams(WechatAccessTokenModel accessTokenModel)
    {
        return new Dictionary<string, string>()
        {
            ["access_token"] = accessTokenModel.AccessToken,
            ["openid"] = accessTokenModel.OpenId,
            ["lang"] = "zh_CN",
        };
    }
}
```