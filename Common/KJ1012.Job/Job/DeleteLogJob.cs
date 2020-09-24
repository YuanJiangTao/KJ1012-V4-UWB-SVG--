using System;
using System.IO;
using System.Linq;
using FluentScheduler;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain;
using KJ1012.Domain.Setting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KJ1012.Job.Job
{
    public class DeleteLogJob : IJob
    {
        public void Execute()
        {
            if (!ConstDefine.IsHostServer) return;
            IEngine engine = EngineContext.Current;
            try
            {
                var setting = engine.GetService<IOptionsMonitor<Setting>>().CurrentValue;
                var saveDays = setting.MaxDayLogSave;
                if (saveDays > 0)
                {
                    var directoryInfo = new DirectoryInfo("logs");
                    if (!directoryInfo.Exists) return;


                    var deleteName = DateTime.Now.AddDays(-saveDays).Date.ToString("yyyyMMdd");
                    var deleteDirectories = directoryInfo.GetDirectories().Where(w => string.Compare(w.Name, deleteName, StringComparison.Ordinal) < 0).ToList();
                    foreach (var deleteDirectory in deleteDirectories)
                    {
                        deleteDirectory.Delete(true);
                    }
                }
            }
            catch (Exception e)
            {
                ILogger<DeleteLogJob> logger = engine.GetService<ILogger<DeleteLogJob>>();
                logger.LogError($"删除日志目录文件失败:{e.InnerException?.Message ?? e.Message}");
            }
        }
    }
}
