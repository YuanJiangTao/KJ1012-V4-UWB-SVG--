using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Implementations;

namespace KJ1012.CollectionCenter.MqttClient
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMqttClient(this IServiceCollection services)
        {
            services.AddSingleton((IMqttNetLogger)new MqttNetLogger());
            services.AddSingleton<CollectionMqttClient>();
            services.AddSingleton(s => (IHostedService)s.GetService<CollectionMqttClient>());
            services.AddSingleton(s => (IMqttClient)s.GetService<CollectionMqttClient>());
            services.AddSingleton<MqttClientAdapterFactory>();
            services.AddSingleton(s => (IMqttClientAdapterFactory)s.GetService<MqttClientAdapterFactory>());

            return services;
        }
    }
}
