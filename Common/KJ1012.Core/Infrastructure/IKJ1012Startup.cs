using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.Core.Infrastructure
{
    public interface IKj1012Startup
    {
        /// <summary>
        /// 注册服务到容器中
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <param name="configuration">应用程序配置</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
        /// <summary>
        /// 配置Http请求管道
        /// </summary>
        /// <param name="app">应用程序管道配置</param>
        /// <param name="env">应用程序运行时的环境对象</param>
        void Configure(IApplicationBuilder app, IWebHostEnvironment env);
        /// <summary>
        /// 执行当前实现的顺序号
        /// </summary>
        int Order { get; }
    }
}
