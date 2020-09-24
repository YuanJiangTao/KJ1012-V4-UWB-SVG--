using System;
using FluentScheduler;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain;
using KJ1012.Services.IServices.Warn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KJ1012.Job.Job
{
    public class DeviceStateJob : IJob
    {
        /// <summary>
        /// 启动时执行设备状态未知处理
        /// </summary>
        public async void Execute()
        {
            if (!ConstDefine.IsHostServer) return;
            IEngine engine = EngineContext.Current;
            try
            {
                var serviceProvider = engine.GetService<IServiceProvider>();
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var deviceWarnService =
                        serviceScope.ServiceProvider.GetService<IDeviceWarnService>();
                    await deviceWarnService.SetDeviceStateUnknown();
                }
            }
            catch (Exception e)
            {
                ILogger<DeviceStateJob> logger = engine.GetService<ILogger<DeviceStateJob>>();
                logger.LogError($"设备未知状态处理失败:{e.InnerException?.Message ?? e.Message}");
            }
        }
    }
}
