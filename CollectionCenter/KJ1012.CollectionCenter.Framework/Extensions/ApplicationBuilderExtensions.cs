using KJ1012.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KJ1012.CollectionCenter.Framework.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 应用程序http管道配置
        /// </summary>
        /// <param name="application">应用程序请求管道构建器</param>
        /// <param name="env">系统当前环境对象</param>
        public static void ConfigureStartupConfig(this IApplicationBuilder application, IWebHostEnvironment env)
        {
            EngineContext.Current.ConfigureRequestPipeline(application,env);
        }
        /// <summary>
        /// 替换默认日志为Log4net
        /// </summary>
        /// <param name="application"></param>
        /// <param name="env"></param>
        public static void UseLog4Net(this IApplicationBuilder application,IWebHostEnvironment env)
        {
            var loggerFactory = application.ApplicationServices.GetService<ILoggerFactory>();
            if (env.IsProduction())
                loggerFactory.AddLog4Net($"{env.ApplicationName}.log4net.config");
            else
                loggerFactory.AddLog4Net($"{env.ApplicationName}.log4net.{env.EnvironmentName}.config");
        }
        /// <summary>
        /// 设置程序能够访问dwg地图文件
        /// </summary>
        /// <param name="application"></param>
        public static void UseCustomStaticFiles(this IApplicationBuilder application)
        {
            // Set up custom content types - associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Add(".dwg", "drawing/x-dwg");
            provider.Mappings.Add(".obj", "application/octet-stream");
            provider.Mappings.Add(".mtl", "application/octet-stream");
            // Add new mappings

            application.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
        }
    }
}
