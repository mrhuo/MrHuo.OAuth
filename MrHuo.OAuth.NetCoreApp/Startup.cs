using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MrHuo.OAuth.NetCoreApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddControllersWithViews();

            //将第三方登录组件注入进去
            services.AddSingleton(new Baidu.BaiduOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:baidu")));
            services.AddSingleton(new Wechat.WechatOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:wechat")));
            services.AddSingleton(new Gitlab.GitlabOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:gitlab")));
            services.AddSingleton(new Gitee.GiteeOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:gitee")));
            services.AddSingleton(new Github.GithubOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:github")));
            services.AddSingleton(new Huawei.HuaweiOAuth(OAuthConfig.LoadFrom(Configuration, "oauth:huawei")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
