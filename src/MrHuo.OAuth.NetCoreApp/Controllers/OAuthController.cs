using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MrHuo.OAuth.NetCoreApp.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet("oauth/{type}")]
        public IActionResult Index(string type)
        {
            var oauth = SupportedOAuth.List.SingleOrDefault(p => p.platform.Equals(type, StringComparison.CurrentCultureIgnoreCase));
            if (oauth.type == null)
            {
                return ReturnToError($"没有实现【{type}】登录方式！");
            }
            dynamic oauthProvider = HttpContext.RequestServices.GetService(oauth.type);
            //var state = DefaultStateManager.Instance().RequestState();
            return Redirect(oauthProvider.GetAuthorizeUrl());
        }

        [HttpGet("oauth/{type}callback")]
        public async Task<IActionResult> LoginCallback(
            [FromQuery] string type,
            [FromQuery] string code,
            [FromQuery] string state,
            [FromQuery] string error_description = "")
        {
            Console.WriteLine($"LoginCallback [{HttpContext.Request.Path}{HttpContext.Request.QueryString.ToString()}]");
            try
            {
                if (!string.IsNullOrEmpty(error_description))
                {
                    throw new Exception(error_description);
                }
                //if (!DefaultStateManager.Instance().IsStateExists(state))
                //{
                //    throw new Exception("禁止 CORS 跨站攻击！");
                //}
                //DefaultStateManager.Instance().RemoveState(state);
                HttpContext.Session.SetString("OAuthPlatform", type.ToLower());

                var oauth = SupportedOAuth.List.SingleOrDefault(p => p.platform.Equals(type, StringComparison.CurrentCultureIgnoreCase));
                if (oauth.type == null)
                {
                    throw new Exception($"没有实现【{type}】登录回调！");
                }
                dynamic oauthProvider = HttpContext.RequestServices.GetService(oauth.type);
                var accessToken = await oauthProvider.GetAccessTokenAsync(code, state);
                var userInfo = await oauthProvider.GetUserInfoAsync(accessToken);
                HttpContext.Session.Set("OAuthUser", new UserInfoBase()
                {
                    Name = userInfo.Name,
                    Avatar = userInfo.Avatar
                });
                HttpContext.Session.Set("OAuthUserDetail", (object)userInfo, true);
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

        [HttpGet("oauth/{type}/user")]
        public async Task<IActionResult> GetUserInfo(
             [FromRoute] string type,
             [FromQuery] string token)
        {
            Console.WriteLine($"GetUserInfo [{HttpContext.Request.Path}{HttpContext.Request.QueryString.ToString()}]");

            var oauth = SupportedOAuth.List.SingleOrDefault(p => p.platform.Equals(type, StringComparison.CurrentCultureIgnoreCase));
            if (oauth.type == null)
            {
                throw new Exception($"没有实现【{type}】登录！");
            }
            dynamic oauthProvider = HttpContext.RequestServices.GetService(oauth.type);
            var userInfo = await oauthProvider.GetUserInfoAsync(new DefaultAccessTokenModel()
            {
                AccessToken = token
            });
            return Json(userInfo);
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
