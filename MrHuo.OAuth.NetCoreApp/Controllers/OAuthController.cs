using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace MrHuo.OAuth.NetCoreApp.Controllers
{
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
}
