using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
//using MrHuo.OAuth.Baidu;
//using MrHuo.OAuth.Gitee;
//using MrHuo.OAuth.Github;
//using MrHuo.OAuth.Huawei;
//using MrHuo.OAuth.QQ;
//using MrHuo.OAuth.Alipay;
using MrHuo.OAuth.Wechat;
using MrHuo.OAuth.Gitlab;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MrHuo.OAuth.Baidu;
using Microsoft.AspNetCore.Http;

namespace MrHuo.OAuth.NetCoreApp.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet("oauth/{type}")]
        public IActionResult Index(
            [FromServices] BaiduOAuth baiduOAuth,
            [FromServices] WechatOAuth wechatOAuth,
            [FromServices] GitlabOAuth gitlabOAuth,
            string type
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
                case "gitlab":
                    {
                        redirectUrl = gitlabOAuth.GetAuthorizeUrl();
                        break;
                    }
                default:
                    return ReturnToError($"没有实现【{type}】登录！");
            }
            return Redirect(redirectUrl);
        }

        [HttpGet("oauth/{type}callback")]
        public async Task<IActionResult> LoginCallback(
            string type,
            [FromServices] WechatOAuth wechatOAuth,
            [FromServices] GitlabOAuth gitlabOAuth,
            [FromQuery] string code,
            [FromQuery] string state)
        {
            try
            {
                HttpContext.Session.SetString("OAuthPlatform", type.ToLower());
                switch (type.ToLower())
                {
                    case "wechat":
                        {
                            var authorizeResult = await wechatOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo);
                            break;
                        }
                    case "gitlab":
                        {
                            var authorizeResult = await gitlabOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo);
                            break;
                        }
                    default:
                        throw new Exception($"没有实现【{type}】登录！");
                }
                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                HttpContext.Session.Remove("OAuthPlatform");
                HttpContext.Session.Remove("OAuthUser");
                HttpContext.Session.Remove("OAuthUserDetail");
                return ReturnToError(ex.Message);
            }
        }

        public IActionResult Result()
        {
            var oauthPlatform = HttpContext.Session.GetString("OAuthPlatform");
            var userInfo = HttpContext.Session.GetString("OAuthUser");
            var userInfoDetail = HttpContext.Session.GetString("OAuthUserDetail");
            if (string.IsNullOrEmpty(oauthPlatform) || string.IsNullOrEmpty(userInfoDetail))
            {
                return ReturnToError("OAuth授权失败！");
            }
            ViewBag.OAuthPlatform = oauthPlatform;
            ViewBag.OAuthUser = JsonSerializer.Deserialize<UserInfoBase>(userInfo);
            ViewBag.OAuthUserDetail = userInfoDetail;
            return View();
        }

        private IActionResult ReturnToError(string error)
        {
            return RedirectToAction("Error", "Home", new { error });
        }
    }
}
