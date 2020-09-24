using KJ1012.CollectionCenter.Framework.Extensions;
using KJ1012.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.CollectionCenter.Framework.Infrastructure
{
    public class CommonStartup : IKj1012Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseLog4Net(env);
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomMvc();

            services.AddCustomDbContent(configuration);

            services.AddRedisCache(configuration);

            services.AddMemoryCacheManager();

            services.AddCustomSetting(configuration);

            //services.AddDataProtection(configuration);
        }

        public int Order => 1;
    }
}
