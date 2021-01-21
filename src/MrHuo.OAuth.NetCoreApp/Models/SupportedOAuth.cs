using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MrHuo.OAuth;

namespace MrHuo.OAuth.NetCoreApp
{
    public class SupportedOAuth
    {
        public static List<(string platform, string platformName, Type type, bool available)> List
        {
            get
            {
                return new List<(string, string, Type, bool)>()
                {
                    ("Baidu", "百度", typeof(Baidu.BaiduOAuth), true),
                    ("Wechat", "微信公众号", typeof(Wechat.WechatOAuth), true),
                    ("Gitlab", "Gitlab", typeof(Gitlab.GitlabOAuth), true),
                    ("Gitee", "Gitee", typeof(Gitee.GiteeOAuth), true),
                    ("Github", "Github", typeof(Github.GithubOAuth), true),
                    ("Huawei", "华为", typeof(Huawei.HuaweiOAuth), true),
                    ("Coding", "Coding", typeof(Coding.CodingOAuth), true),
                    ("SinaWeibo", "新浪微博", typeof(SinaWeibo.SinaWeiboOAuth), true),
                    ("Alipay", "支付宝", typeof(Alipay.AlipayOAuth), true),
                    ("QQ", "QQ", typeof(QQ.QQOAuth), true),
                    ("OSChina", "OSChina", typeof(OSChina.OSChinaOAuth), true),
                    ("DouYin", "抖音", typeof(DouYin.DouYinOAuth), false),
                    ("WechatOpen", "微信开放平台", typeof(WechatOpen.WechatOpenOAuth), false),
                    ("MeiTuan", "美团外卖", typeof(MeiTuan.MeiTuanOAuth), false),
                    ("XunLei", "迅雷", typeof(XunLei.XunLeiOAuth), true),
                    ("DingTalk", "钉钉", typeof(DingTalk.DingTalkOAuth), true),
                    ("DingTalkQrcode", "钉钉扫码登录", typeof(DingTalkQrcode.DingTalkQrcodeOAuth), true),
                };
            }
        }
    }
}
