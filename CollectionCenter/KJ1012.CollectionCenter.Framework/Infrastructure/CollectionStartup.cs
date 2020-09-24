using System.IO;
using KJ1012.CollectionCenter.MqttClient.Protocol;
using KJ1012.CollectionCenter.SocketSend;
using KJ1012.CollectionCenter.SocketService;
using KJ1012.Core.Configuration;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain;
using KJ1012.Domain.Setting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using KJ1012.CollectionCenter.Protocol.Protocol;
using Microsoft.AspNetCore.Hosting;

namespace KJ1012.CollectionCenter.Framework.Infrastructure
{
    public class CollectionStartup : IKj1012Startup
    {

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //当存在standby.txt文件时说明该服务器是备机
            if (File.Exists("standby.txt"))
            {
                ConstDefine.ServerModel = 1;
            }
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<SocketHostedService>();
            services.AddSingleton(s => (IHostedService)s.GetService<SocketHostedService>());
            services.AddSingleton(s => (ISocketHostedService)s.GetService<SocketHostedService>());

            services.AddTransient<ISocketServer, SocketServer>();
            services.AddSingleton<ISocketSendServer, SocketSendServer>();
            services.AddSingleton(typeof(ISocketClientDictionary<,>), typeof(SocketClientDictionary<,>));
            #region 数据解析服务

            var serviceProvider = services.BuildServiceProvider();
            //数据协议工作对象
            services.AddSingleton<IProtocolFactory, ProtocolFactory>();
            //查找项目中实现IWebStartup接口的类
            var typeFinder = new WebAppTypeFinder();
            var baseServices = typeFinder.FindClassesOfType(typeof(IProtocol));
            var iProtocols = baseServices
                .Select(baseService => (IProtocol)ActivatorUtilities.CreateInstance(serviceProvider, baseService))
                .OrderBy(baseService => baseService.ProtocolId);
            foreach (var protocol in iProtocols)
            {
                protocol.ConfigureServices(services, configuration);
                services.AddSingleton(typeof(IProtocol), protocol);
            }


            #endregion
            #region 数据下发解析服务
            //数据协议工作对象
            services.AddSingleton<ISendProtocolFactory, SendProtocolFactory>();
            //查找项目中实现IWebStartup接口的类
            var sendProtocols = typeFinder.FindClassesOfType(typeof(ISendProtocol));
            foreach (var protocol in sendProtocols)
            {
                services.AddScoped(protocol.GetInterfaces().First(), protocol);
            }
            //配置

            services.AddSetting<CollectionSetting>(configuration.GetSection("KJ1012Collection"));

            #endregion
        }

        public int Order => 2;
    }
}
