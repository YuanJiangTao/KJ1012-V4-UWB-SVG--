using System;
using KJ1012.CollectionCenter.Framework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KJ1012.Core.Infrastructure;

namespace KJ1012.CollectionCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.ConfigureApplicationServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (EngineContext.Current is Engine engine)
            {
                engine.ServiceProvider = app.ApplicationServices;
            }
            app.ConfigureStartupConfig(env);
        }
    }
}
