using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.Core.Infrastructure
{
    public interface IEngine
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="services">服务集合</param>
        void Initialize(IServiceCollection services);

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">应用程序配置项</param>
        /// <returns>服务提供者</returns>
        IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        ///配置http请求管道
        /// </summary>
        /// <param name="application">构建应用程序http请求管道</param>
        void ConfigureRequestPipeline(IApplicationBuilder application, IWebHostEnvironment env);

        /// <summary>
        /// 获取提供服务实例的对象
        /// </summary>
        /// <typeparam name="T">要获取的实例类型</typeparam>
        /// <returns>提供服务实例的对象</returns>
        T GetService<T>() where T : class;

        /// <summary>
        /// 获取提供服务实例对象的集合
        /// </summary>
        /// <param name="type">要获取的实例类型</param>
        /// <returns>提供服务实例对象的集合</returns>
        IEnumerable<object> GetServices(Type type);
        /// <summary>
        /// 获取提供服务实例对象的集合
        /// </summary>
        /// <typeparam name="T">要获取的实例类型</typeparam>
        /// <returns>提供服务实例对象的集合</returns>
        IEnumerable<T> GetServices<T>();
        /// <summary>
        /// 获取提供服务实例的对象
        /// </summary>
        /// <typeparam name="T">要获取的实例类型</typeparam>
        /// <returns>提供服务实例的对象</returns>
        T GetRequiredService<T>() where T : class;

        /// <summary>
        /// 获取提供服务实例的对象
        /// </summary>
        /// <param name="type">要获取的实例类型</param>
        /// <returns>提供服务实例的对象</returns>
        object GetRequiredService(Type type);

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type">Type of service</param>
        /// <returns>Resolved service</returns>
        object ResolveUnregistered(Type type);
    }
}

