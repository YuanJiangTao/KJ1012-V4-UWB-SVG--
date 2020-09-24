using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KJ1012.Core.Helper;
using KJ1012.Core.Mapper;

namespace KJ1012.Core.Infrastructure
{
    public class Engine : IEngine
    {
        /// <summary>
        /// 系统初始化
        /// </summary>
        /// <param name="services"></param>
        public void Initialize(IServiceCollection services)
        {
            //所有api都需要使用Tls12
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //设置应用程序的根目录
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
            CommonHelper.BaseDirectory = hostingEnvironment.ContentRootPath;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //查找项目中实现IWebStartup接口的类
            var typeFinder = new WebAppTypeFinder();
            //注册引擎对象
            services.AddSingleton<IEngine>(this);

            //注册ITypeFinder对象
            services.AddSingleton<ITypeFinder>(typeFinder);

            var startupConfigurations = typeFinder.FindClassesOfType<IKj1012Startup>();

            //创建并排序IStartup接口的类
            var instances = startupConfigurations
                .Select(startup => (IKj1012Startup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //配置服务
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration);

            //注册autoMapper
            AddAutoMapper(services, typeFinder);

            //依赖注入
            //var nopConfig = services.BuildServiceProvider().GetService<NopConfig>();
            RegisterDependencies(services, typeFinder);

            //run startup tasks
            RunStartupTasks(typeFinder);

            ////resolve assemblies here. otherwise, plugins can throw an exception when rendering views
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            ////set App_Data path as base data directory (required to create and save SQL Server Compact database file in App_Data folder)
            //AppDomain.CurrentDomain.SetData("DataDirectory", CommonHelper.MapPath("~/App_Data/"));

            return services.BuildServiceProvider();
        }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="typeFinder">类型查找对象</param>
        protected virtual void RegisterDependencies(IServiceCollection services, ITypeFinder typeFinder)
        {

            //查找项目中的实现IDependencyRegistrar的类
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            //var serviceProvider = services.BuildServiceProvider();
            //var typeActivatorCache = serviceProvider.GetService<ITypeActivatorCache>();

            ////按照序号创建依赖注入对象
            //var instances = dependencyRegistrars
            //    //.Where(dependencyRegistrar => PluginManager.FindPlugin(dependencyRegistrar).Return(plugin => plugin.Installed, true)) //ignore not installed plugins
            //    .Select(dependencyRegistrar => typeActivatorCache.CreateInstance<IDependencyRegistrar>(serviceProvider, dependencyRegistrar))
            //    .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //按照序号创建依赖注入对象
            var instances = dependencyRegistrars
                //.Where(dependencyRegistrar => PluginManager.FindPlugin(dependencyRegistrar).Return(plugin => plugin.Installed, true)) //ignore not installed plugins
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //注册所有的依赖注入项
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(services, typeFinder);

        }
        /// <summary>
        /// 注册并配置 AutoMapper
        /// </summary>
        /// <param name="services">服务提供者</param>
        /// <param name="typeFinder">查找类型帮助对象</param>
        protected virtual void AddAutoMapper(IServiceCollection services, ITypeFinder typeFinder)
        {
            //查找程序集AutoMapper对象
            var mapperConfigurations = typeFinder.FindClassesOfType<IMapperProfile>();

            //创建排序初始化配置对象
            var instances = mapperConfigurations
                //.Where(mapperConfiguration => PluginManager.FindPlugin(mapperConfiguration)?.Installed ?? true) //ignore not installed plugins
                .Select(mapperConfiguration => (IMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //创建配置对象
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //注册 AutoMapper
            services.AddAutoMapper();

            //注册
            AutoMapperConfiguration.Init(config);
        }
        public void ConfigureRequestPipeline(IApplicationBuilder application, IWebHostEnvironment env)
        {
            //从程序集中查找IWebStartup类型
            var typeFinder = GetService<ITypeFinder>();
            var startupConfigurations = typeFinder.FindClassesOfType<IKj1012Startup>();

            //创建并排序启动配置
            var instances = startupConfigurations
                //.Where(startup => PluginManager.FindPlugin(startup)?.Installed ?? true) //忽略没有注册的插件
                .Select(startup => (IKj1012Startup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //管道注册
            foreach (var instance in instances)
                instance.Configure(application, env);
        }
        /// <summary>
        /// 运行程序启动后的任务
        /// </summary>
        /// <param name="typeFinder">类型查询器</param>
        protected virtual void RunStartupTasks(ITypeFinder typeFinder)
        {
            //find startup tasks provided by other assemblies
            var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();

            //create and sort instances of startup tasks
            //we startup this interface even for not installed plugins. 
            //otherwise, DbContext initializers won't run and a plugin installation won't work
            var instances = startupTasks
                .Select(startupTask => (IStartupTask)Activator.CreateInstance(startupTask))
                .OrderBy(startupTask => startupTask.Order);

            //execute tasks
            foreach (var task in instances)
                task.Execute();
        }

        public T GetService<T>() where T : class
        {
            return GetCurrentServiceProvider().GetService<T>();
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return GetCurrentServiceProvider().GetServices(type);
        }

        public IEnumerable<T> GetServices<T>()
        {
            return GetCurrentServiceProvider().GetServices<T>();
        }

        public T GetRequiredService<T>() where T : class
        {
            return GetCurrentServiceProvider().GetRequiredService<T>();
        }

        public object GetRequiredService(Type type)
        {
            return GetCurrentServiceProvider().GetRequiredService(type);
        }

        public object ResolveUnregistered(Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取服务提供者
        /// </summary>
        /// <returns></returns>
        protected IServiceProvider GetCurrentServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context != null ? context.RequestServices : ServiceProvider;
        }
        /// <summary>
        /// 服务提供者
        /// </summary>
        public virtual IServiceProvider ServiceProvider { get; set; }
    }
}