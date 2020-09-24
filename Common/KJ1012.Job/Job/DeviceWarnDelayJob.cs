using FluentScheduler;
using Microsoft.Extensions.Logging;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain;
using System;
using KJ1012.Core.Data;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.Job.Job
{
    public class DeviceWarnDelayJob : IJob
    {
        public async void Execute()
        {
            if (!ConstDefine.IsHostServer) return;
            IEngine engine = EngineContext.Current;
            try
            {
                var serviceProvider = engine.GetService<IServiceProvider>();
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var unitOfWork =
                        serviceScope.ServiceProvider.GetService<IUnitOfWork>();
                    await unitOfWork.ExecuteSqlCommandAsync("EXEC UpdateDelayDeviceState");
                }

            }
            catch (Exception e)
            {
                ILogger<DeviceWarnDelayJob> logger = engine.GetService<ILogger<DeviceWarnDelayJob>>();
                logger.LogError($"设备延迟处理失败:{e.InnerException?.Message ?? e.Message}");
            }
        }
    }
}
