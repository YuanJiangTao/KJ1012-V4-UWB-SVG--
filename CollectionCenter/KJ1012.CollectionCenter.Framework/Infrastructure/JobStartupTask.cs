using FluentScheduler;
using KJ1012.Core.Data;
using KJ1012.Core.Infrastructure;
using KJ1012.Job.Job;

namespace KJ1012.CollectionCenter.Framework.Infrastructure
{
    public class JobStartupTask : IStartupTask
    {
        public void Execute()
        {
            if (DataSettingManager.IsInstalled)
            {
                JobManager.Initialize(new JobRegistry());
            }
        }

        public int Order => 2;
    }
}
