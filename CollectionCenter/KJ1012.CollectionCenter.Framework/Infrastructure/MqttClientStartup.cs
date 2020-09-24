using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KJ1012.CollectionCenter.MqttClient;
using KJ1012.Core.Infrastructure;

namespace KJ1012.CollectionCenter.Framework.Infrastructure
{
    public class MqttClientStartup : IKj1012Startup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddMqttClient();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    

        public int Order => 4;
    }
}
