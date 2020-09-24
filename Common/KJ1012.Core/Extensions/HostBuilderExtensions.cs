using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace KJ1012.Core.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseStartup(this IHostBuilder hostBuilder, Type startupType)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    services.AddSingleton(typeof(IStartup), startupType);
                else
                    services.AddSingleton(typeof(IStartup),
                        sp =>
                        {
                            IWebHostEnvironment requiredService = sp.GetRequiredService<IWebHostEnvironment>();
                            return new ConventionBasedStartup(
                                StartupLoader.LoadMethods(sp, startupType, requiredService.EnvironmentName));
                        });
            });
        }
        /// <summary>Specify the startup type to be used by the web host.</summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> to configure.</param>
        /// <typeparam name="TStartup">The type containing the startup methods for the application.</typeparam>
        /// <returns>The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</returns>
        public static IHostBuilder UseStartup<TStartup>(this IHostBuilder hostBuilder) where TStartup : class
        {
            return hostBuilder.UseStartup(typeof(TStartup));
        }
    }
}
