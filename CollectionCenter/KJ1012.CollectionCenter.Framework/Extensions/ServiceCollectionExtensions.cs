using System;
using KJ1012.Core.Caching;
using KJ1012.Core.Configuration;
using KJ1012.Core.Data;
using KJ1012.Core.Encrypt;
using KJ1012.Core.Infrastructure;
using KJ1012.Data;
using KJ1012.Domain;
using KJ1012.Domain.Setting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace KJ1012.CollectionCenter.Framework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 配置应用程序所需服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">应用程序配置</param>
        /// <returns></returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            ////add NopConfig configuration parameters
            //services.ConfigureStartupConfig<NopConfig>(configuration.GetSection("Nop"));
            ////add hosting configuration parameters
            //services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));
            //add accessor to HttpContext
            services.AddHttpContextAccessor();


            //创建、初始化并配置引擎对象
            var engine = EngineContext.Current;
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services, configuration);

            //if (DataSettingsHelper.DatabaseIsInstalled())
            //{
            //    //implement schedule tasks
            //    //database is already installed, so start scheduled tasks
            //    TaskManager.Instance.Initialize();
            //    TaskManager.Instance.Start();

            //    //log application start
            //    EngineContext.Current.Resolve<ILogger>().Information("Application started", null, null);
            //}

            return serviceProvider;
        }
        /// <summary>
        /// 通知指定配置参数注册和绑定配置对象 
        /// </summary>
        /// <typeparam name="TConfig">配置对象实例模型</typeparam>
        /// <param name="services">服务描述集合</param>
        /// <param name="configuration">配置参数</param>
        /// <returns>配置参数对象的实例</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //创建配置实例
            var config = new TConfig();

            configuration.Bind(config);

            //注册为服务
            services.AddSingleton(config);

            return config;
        }

        /// <summary>
        /// 注册 HttpContext 访问器
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        /// <summary>
        /// 注入数据库上下文
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param> 
        public static void AddCustomDbContent(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSetting<DataSettings>(configuration.GetSection("DataSettings"));
            var dataSetting = services.BuildServiceProvider().GetService<IOptions<DataSettings>>().Value;

            DataSettingManager.IsInstalled = dataSetting.IsValid;
            if (dataSetting.IsValid)
            {
                if (dataSetting.DataProvider == DataProviderType.SqlServer)
                {
                    string connectionString =
                        AesHelper.AesDecrypt(dataSetting.ConnectionString, ConstDefine.DataSettingAesKey);
                    //数据库配置
                    services.AddDbContextPool<Kj1012Context>(options =>
                    {
                        options.UseSqlServer(
                            connectionString, b => { b.MigrationsAssembly("KJ1012.Web"); });
                    }, 180);
                }
            }
        }
        /// <summary>
        /// 注入Redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRedis(options =>
            {
                options.Configuration = configuration.GetSection("KJ1012:RedisConnection").Value;
            });
        }
        /// <summary>
        /// 注入内存缓存对象
        /// </summary>
        /// <param name="services"></param>
        public static void AddMemoryCacheManager(this IServiceCollection services)
        {
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
        }
        /// <summary>
        /// 注入Mvc配置信息
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomMvc(this IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }
        /// <summary>
        /// 注入跨越处理逻辑
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins("http://localhost:8081", "http://localhost:8080", "http://localhost:8082", "http://localhost:9080");
                });
            });
        }
        /// <summary>
        /// 应用程序配置信息
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddCustomSetting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSetting<Setting>(configuration.GetSection("KJ1012"));
            services.AddSetting<PositionSetting>(configuration.GetSection("PositionSetting"));
        }

    }
}
