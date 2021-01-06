using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MrHuo.OAuth.Baidu;
using MrHuo.OAuth.Gitee;
using MrHuo.OAuth.Github;
using MrHuo.OAuth.Huawei;
using MrHuo.OAuth.QQ;
using MrHuo.OAuth.Wechat;
using MrHuo.OAuth.Alipay;

namespace MrHuo.OAuth.NetCoreApp.Controllers
{
    public class OAuthController : Controller
    {
        private readonly GithubOAuth githubOauth = null;
        private readonly WechatOAuth wechatOAuth = null;
        private readonly QQOAuth qqOAuth = null;
        private readonly HuaweiOAuth huaweiOAuth = null;
        private readonly GiteeOAuth giteeOAuth = null;
        private readonly BaiduOAuth baiduOAuth = null;
        private readonly AlipayOAuth alipayOAuth = null;

        public OAuthController(
            GithubOAuth githubOauth, 
            WechatOAuth wechatOAuth, 
            QQOAuth qqOAuth,
            HuaweiOAuth huaweiOAuth,
            GiteeOAuth giteeOAuth,
            BaiduOAuth baiduOAuth,
            AlipayOAuth alipayOAuth
        )
        {
            this.githubOauth = githubOauth;
            this.wechatOAuth = wechatOAuth;
            this.qqOAuth = qqOAuth;
            this.huaweiOAuth = huaweiOAuth;
            this.giteeOAuth = giteeOAuth;
            this.baiduOAuth = baiduOAuth;
            this.alipayOAuth = alipayOAuth;
            this.alipayOAuth.EnableStateCheck = false;
        }

        [HttpGet("oauth/{type}")]
        public IActionResult Index(string type)
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
                case "qq":
                    {
                        qqOAuth.Authorize();
                        break;
                    }
                case "huawei":
                    {
                        huaweiOAuth.Authorize();
                        break;
                    }
                case "gitee":
                    {
                        giteeOAuth.Authorize();
                        break;
                    }
                case "baidu":
                    {
                        baiduOAuth.Authorize();
                        break;
                    }
                case "alipay":
                    {
                        alipayOAuth.Authorize();
                        break;
                    }
                default:
                    return Content($"没有实现【{type}】登录！");
            }
            return Content("");
        }

        [HttpGet("oauth/{type}callback")]
        public IActionResult LoginCallback(string type)
        {
            switch (type.ToLower())
            {
                case "github":
                    {
                        var accessToken = githubOauth.AuthorizeCallback();
                        var userInfo = githubOauth.GetUserInfo(accessToken);
                        return Content(JsonSerializer.Serialize(new
                        {
                            accessToken,
                            userInfo
                        }));
                    }
                case "wechat":
                    {
                        var accessToken = wechatOAuth.AuthorizeCallback();
                        var userInfo = wechatOAuth.GetUserInfo(accessToken);
                        return Content(JsonSerializer.Serialize(new
                        {
                            accessToken,
                            userInfo
                        }));
                    }
                case "qq":
                    {
                        var accessToken = qqOAuth.AuthorizeCallback();
                        var userInfo = qqOAuth.GetUserInfo(accessToken);
                        return Content(JsonSerializer.Serialize(new
                        {
                            accessToken,
                            userInfo
                        }));
                    }
                case "huawei":
                    {
                        var accessToken = huaweiOAuth.AuthorizeCallback();
                        var userInfo = huaweiOAuth.GetUserInfo(accessToken);
                        return Content(JsonSerializer.Serialize(new
                        {
                            accessToken,
                            userInfo
                        }));
                    }
                case "gitee":
                    {
                        var accessToken = giteeOAuth.AuthorizeCallback();
                        var userInfo = giteeOAuth.GetUserInfo(accessToken);
                        return Content(JsonSerializer.Serialize(new
                        {
                            accessToken,
                            userInfo
                        }));
                    }
                case "baidu":
                    {
                        var accessToken = baiduOAuth.AuthorizeCallback();
                        var userInfo = baiduOAuth.GetUserInfo(accessToken);
                        return Content(JsonSerializer.Serialize(new
                        {
                            accessToken,
                            userInfo
                        }));
                    }
                case "alipay":
                    {
                        var accessToken = alipayOAuth.AuthorizeCallback();
                        //var userInfo = alipayOAuth.GetUserInfo(accessToken);
                        return Content(JsonSerializer.Serialize(new
                        {
                            accessToken,
                            //userInfo
                        }));
                    }
            }
            return Content($"没有实现【{type}】登录！");
        }
    }
}
