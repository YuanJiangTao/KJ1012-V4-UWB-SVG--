using CachingFramework.Redis.Contracts.Providers;
using FluentScheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KJ1012.Core.Helper;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain;
using KJ1012.Domain.Setting;
using KJ1012.Services.IServices.Warn;
using System;
using System.Linq;

namespace KJ1012.Job.Job
{
    public class TerminalWarnDelayJob : IJob
    {
        public async void Execute()
        {
            if (!ConstDefine.IsHostServer) return;
            IEngine engine = EngineContext.Current;
            var kj1012Setting = engine.GetService<IOptionsMonitor<Setting>>().CurrentValue;
            if (kj1012Setting.TerminalWarnDelay >= 5)
            {
                try
                {
                    var collectionProvider = engine.GetService<ICollectionProvider>();
                    var sortedSet = collectionProvider.GetRedisSortedSet<int>("terminalIdDelay:terminalId");
                    var sortedMembers = sortedSet.GetRangeByRank().Where(r =>
                        (DateTime.Now - CommonHelper.MillisecondsConvertToDateTime(r.Score)).TotalSeconds >=
                        kj1012Setting.TerminalWarnDelay).ToList();
                    if (sortedMembers.Any())
                    {
                        var serviceProvider = engine.GetService<IServiceProvider>();
                        using (var serviceScope = serviceProvider.CreateScope())
                        {
                            var terminalWarnService =
                                serviceScope.ServiceProvider.GetService<ITerminalWarnService>();
                            foreach (var sortedMember in sortedMembers)
                            {
                                var exitId = sortedMember.Value;
                                await terminalWarnService.BaseRepository.TableNoTracking.FirstOrDefaultAsync(f => f.TerminalId == exitId);
                                //删除redis已处理设备异常数据
                                sortedSet.Remove(sortedMember.Value);

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ILogger<DeviceWarnDelayJob> logger = engine.GetService<ILogger<DeviceWarnDelayJob>>();
                    logger.LogError($"终端异常数据推送失败:{e.InnerException?.Message ?? e.Message}");

                }
            }
        }
    }
}
