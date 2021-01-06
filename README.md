# MrHuo.OAuth

> 前置条件：.net core 项目或 .net framework 4.6 以上

快速接入 OAuth 登录。

体验网址：[https://oauthlogin.net/](https://oauthlogin.net/)

#### 已支持平台

- [x] Github
- [x] QQ
- [x] 微信
- [x] 华为
- [x] Gitee
- [x] 百度
- [ ] 支付宝
- [ ] CSDN
- [ ] 钉钉
- [ ] Microsoft
...更多平台开发中

1.`Startup.cs`

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddSession();
    services.AddHttpContextAccessor();

    services.AddSingleton<GithubOAuth>();
    services.AddSingleton<WechatOAuth>();
    //... 其他登录方式
}
```

```
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    //...
    app.UseSession();
    //...
}
```

2.`OAuthController.cs` 根据实际需要自行命名

```
public class OAuthController : Controller
{
    private readonly Github.GithubOAuth githubOauth = null;
    private readonly Wechat.WechatOAuth wechatOAuth = null;

    public OAuthController(
        Github.GithubOAuth githubOauth,
        Wechat.WechatOAuth wechatOAuth
    )
    {
        this.githubOauth = githubOauth;
        this.wechatOAuth = wechatOAuth;
    }

    [HttpGet("oauth/{type}")]
    public void Index(string type)
    {
        switch (type.ToLower())
        {
            case "github":
                {
                    githubOauth.Authorize();
                    break;
                }
            case "wechat":
                {
                    wechatOAuth.Authorize();
                    break;
                }
            default:
                HttpContext.Response.StatusCode = 404;
                break;
        }
    }

    [HttpGet("oauth/{type}callback")]
    public IActionResult LoginCallback(string type)
    {
        switch (type.ToLower())
        {
            case "github":
                {
                    var accessToken = githubOauth.GetAccessToken(Request.Query["code"], Request.Query["state"]);
                    var userInfo = githubOauth.GetUserInfo(accessToken);
                    return Json(new
                    {
                        accessToken,
                        userInfo
                    });
                }
            case "wechat":
                {
                    var accessToken = wechatOAuth.GetAccessToken(Request.Query["code"], Request.Query["state"]);
                    var userInfo = wechatOAuth.GetUserInfo(accessToken);
                    return Json(new
                    {
                        accessToken,
                        userInfo
                    });
                }
        }
        return Content($"没有实现【{type}】登录！");
    }
}
```

3.`Views`

```
<!--在代码中放置授权按钮-->
<a href="/oauth/github" class="btn btn-primary">Github 登录</a>
<a href="/oauth/wechat" class="btn btn-primary">Wechat 扫码登录</a>
<!-- //其他登录方式照样子往下写 -->
```