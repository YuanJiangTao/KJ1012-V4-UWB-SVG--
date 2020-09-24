using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.Core.Configuration
{
    public static class ServiceCollectionSettingExtensions
    {
        public static IServiceCollection AddSetting<T>(this IServiceCollection serviceCollection, IConfiguration configuration)
            where T : class, new()
        {
            serviceCollection.Configure<T>(configuration);
            return serviceCollection;
        }
        public static IServiceCollection AddSetting<T>(this IServiceCollection serviceCollection, string appSettingFile, bool optional = false, bool reloadOnChange = true)
            where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = appSettingFile, Optional = optional, ReloadOnChange = reloadOnChange })
                .Build();

            var appconfig = new ServiceCollection();
            appconfig.Configure<T>(config);
            return appconfig;
        }
    }
}
