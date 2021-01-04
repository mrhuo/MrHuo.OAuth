using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MrHuo.OAuth.Wechat;
using MrHuo.OAuth.Github;
using MrHuo.OAuth.QQ;

namespace MrHuo.OAuth.NetCoreApp.Controllers
{
    public class OAuthController : Controller
    {
        private readonly GithubOAuth githubOauth = null;
        private readonly WechatOAuth wechatOAuth = null;
        private readonly QQOAuth qqOAuth = null;
        public OAuthController(GithubOAuth githubOauth, WechatOAuth wechatOAuth, QQOAuth qqOAuth)
        {
            this.githubOauth = githubOauth;
            this.wechatOAuth = wechatOAuth;
            this.qqOAuth = qqOAuth;
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
                case "qq":
                    {
                        qqOAuth.Authorize();
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
            var code = Request.Query["code"];
            var state = Request.Query["state"];
            switch (type.ToLower())
            {
                case "github":
                    {
                        var accessToken = githubOauth.GetAccessToken(code, state);
                        var userInfo = githubOauth.GetUserInfo(accessToken);
                        return Content(MrHuo.OAuth.Json.Serialize(new
                        {
                            accessToken,
                            userInfo
                        }));
                    }
                case "wechat":
                    {
                        var accessToken = wechatOAuth.GetAccessToken(code, state);
                        var userInfo = wechatOAuth.GetUserInfo(accessToken);
                        return Content(MrHuo.OAuth.Json.Serialize(new
                        {
                            accessToken,
                            userInfo
                        }));
                    }
                case "qq":
                    {
                        var accessToken = qqOAuth.GetAccessToken(code, state);
                        //var userInfo = wechatOAuth.GetUserInfo(accessToken);
                        return Content(MrHuo.OAuth.Json.Serialize(new
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
