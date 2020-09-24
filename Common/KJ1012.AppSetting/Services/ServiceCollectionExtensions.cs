using KJ1012.AppSetting.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.AppSetting.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInstallationManage(this IServiceCollection services)
        {
            services.AddScoped<ISqlInstallationService, SqlServerInstallationService>();
        }
    }
}
