using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MrHuo.OAuth.Huawei;
using MrHuo.OAuth.Wechat;
using MrHuo.OAuth.Gitlab;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MrHuo.OAuth.Baidu;
using Microsoft.AspNetCore.Http;
using MrHuo.OAuth.Gitee;
using MrHuo.OAuth.Github;
using MrHuo.OAuth.Coding;
using MrHuo.OAuth.SinaWeibo;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using MrHuo.OAuth.Alipay;
using MrHuo.OAuth.QQ;
using MrHuo.OAuth.OSChina;
using MrHuo.OAuth.DouYin;
using MrHuo.OAuth.WechatOpen;
using MrHuo.OAuth.MeiTuan;
using MrHuo.OAuth.XunLei;

namespace MrHuo.OAuth.NetCoreApp.Controllers
{
    public class OAuthController : Controller
    {
        private readonly BaiduOAuth baiduOAuth;
        private readonly WechatOAuth wechatOAuth;
        private readonly GitlabOAuth gitlabOAuth;
        private readonly GiteeOAuth giteeOAuth;
        private readonly GithubOAuth githubOAuth;
        private readonly HuaweiOAuth huaweiOAuth;
        private readonly CodingOAuth codingOAuth;
        private readonly SinaWeiboOAuth sinaWeiboOAuth;
        private readonly AlipayOAuth alipayOAuth;
        private readonly QQOAuth qqOAuth;
        private readonly OSChinaOAuth oschinaOAuth;
        private readonly DouYinOAuth douYinOAuth;
        private readonly WechatOpenOAuth wechatOpenOAuth;
        private readonly MeiTuanOAuth meiTuanOAuth;
        private readonly XunLeiOAuth xunLeiOAuth;

        public OAuthController(
            [FromServices] BaiduOAuth _baiduOAuth,
            [FromServices] WechatOAuth _wechatOAuth,
            [FromServices] GitlabOAuth _gitlabOAuth,
            [FromServices] GiteeOAuth _giteeOAuth,
            [FromServices] GithubOAuth _githubOAuth,
            [FromServices] HuaweiOAuth _huaweiOAuth,
            [FromServices] CodingOAuth _codingOAuth,
            [FromServices] SinaWeiboOAuth _sinaWeiboOAuth,
            [FromServices] AlipayOAuth _alipayOAuth,
            [FromServices] QQOAuth _qqOAuth,
            [FromServices] OSChinaOAuth _oschinaOAuth,
            [FromServices] DouYinOAuth _douYinOAuth,
            [FromServices] WechatOpenOAuth _wechatOpenOAuth,
            [FromServices] MeiTuanOAuth _meiTuanOAuth,
            [FromServices] XunLeiOAuth _xunLeiOAuth
            )
        {
            this.baiduOAuth = _baiduOAuth;
            this.wechatOAuth = _wechatOAuth;
            this.gitlabOAuth = _gitlabOAuth;
            this.giteeOAuth = _giteeOAuth;
            this.githubOAuth = _githubOAuth;
            this.huaweiOAuth = _huaweiOAuth;
            this.codingOAuth = _codingOAuth;
            this.sinaWeiboOAuth = _sinaWeiboOAuth;
            this.alipayOAuth = _alipayOAuth;
            this.qqOAuth = _qqOAuth;
            this.oschinaOAuth = _oschinaOAuth;
            this.douYinOAuth = _douYinOAuth;
            this.wechatOpenOAuth = _wechatOpenOAuth;
            this.meiTuanOAuth = _meiTuanOAuth;
            this.xunLeiOAuth = _xunLeiOAuth;
        }

        [HttpGet("oauth/{type}")]
        public IActionResult Index(string type)
        {
            var state = DefaultStateManager.Instance().RequestState();
            var authorizeUrls = new Dictionary<string, Func<string, string>>()
            {
                ["baidu"] = baiduOAuth.GetAuthorizeUrl,
                ["wechat"] = wechatOAuth.GetAuthorizeUrl,
                ["gitlab"] = gitlabOAuth.GetAuthorizeUrl,
                ["gitee"] = giteeOAuth.GetAuthorizeUrl,
                ["github"] = githubOAuth.GetAuthorizeUrl,
                ["huawei"] = huaweiOAuth.GetAuthorizeUrl,
                ["coding"] = codingOAuth.GetAuthorizeUrl,
                ["sinaweibo"] = sinaWeiboOAuth.GetAuthorizeUrl,
                ["alipay"] = alipayOAuth.GetAuthorizeUrl,
                ["qq"] = qqOAuth.GetAuthorizeUrl,
                ["oschina"] = oschinaOAuth.GetAuthorizeUrl,
                ["douyin"] = douYinOAuth.GetAuthorizeUrl,
                ["wechatopen"] = wechatOpenOAuth.GetAuthorizeUrl,
                ["meituan"] = meiTuanOAuth.GetAuthorizeUrl,
                ["xunlei"] = xunLeiOAuth.GetAuthorizeUrl,
            };

            if (!authorizeUrls.TryGetValue(type.ToLower(), out var redirectUrl))
                return ReturnToError($"没有实现【{type}】登录方式！");

            return Redirect(redirectUrl(state));
        }

        [HttpGet("oauth/{type}callback")]
        public async Task<IActionResult> LoginCallback(
            string type,
            [FromQuery] string code,
            [FromQuery] string state,
            [FromQuery] string error_description = "")
        {
            Console.WriteLine($"LoginCallback [{HttpContext.Request.Path}]");
            try
            {
                if (!string.IsNullOrEmpty(error_description))
                {
                    throw new Exception(error_description);
                }
                if (!DefaultStateManager.Instance().IsStateExists(state))
                {
                    throw new Exception("禁止 CORS 跨站攻击！");
                }
                DefaultStateManager.Instance().RemoveState(state);
                HttpContext.Session.SetString("OAuthPlatform", type.ToLower());

                switch (type.ToLower())
                {
                    case "baidu":
                        {
                            var authorizeResult = await baiduOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "wechat":
                        {
                            var authorizeResult = await wechatOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
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
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "gitee":
                        {
                            var authorizeResult = await giteeOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "github":
                        {
                            var authorizeResult = await githubOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "huawei":
                        {
                            var authorizeResult = await huaweiOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "coding":
                        {
                            var authorizeResult = await codingOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "sinaweibo":
                        {
                            var authorizeResult = await sinaWeiboOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "alipay":
                        {
                            code = HttpContext.Request.Query["auth_code"];
                            var authorizeResult = await alipayOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "qq":
                        {
                            var authorizeResult = await qqOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "oschina":
                        {
                            var authorizeResult = await oschinaOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "douyin":
                        {
                            var authorizeResult = await douYinOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "wechatopen":
                        {
                            var authorizeResult = await wechatOpenOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "meituan":
                        {
                            var authorizeResult = await meiTuanOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    case "xunlei":
                        {
                            var authorizeResult = await xunLeiOAuth.AuthorizeCallback(code, state);
                            if (!authorizeResult.IsSccess)
                            {
                                throw new Exception(authorizeResult.ErrorMessage);
                            }
                            HttpContext.Session.Set("OAuthUser", authorizeResult.UserInfo.ToUserInfoBase());
                            HttpContext.Session.Set("OAuthUserDetail", authorizeResult.UserInfo, true);
                            break;
                        }
                    default:
                        throw new Exception($"没有实现【{type}】登录回调！");
                }
                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                HttpContext.Session.Remove("OAuthPlatform");
                HttpContext.Session.Remove("OAuthUser");
                HttpContext.Session.Remove("OAuthUserDetail");
                Console.WriteLine(ex.ToString());
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
