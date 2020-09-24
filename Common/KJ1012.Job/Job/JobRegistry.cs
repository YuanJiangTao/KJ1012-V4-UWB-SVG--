using System;
using FluentScheduler;

namespace KJ1012.Job.Job
{
    public class JobRegistry : Registry
    {
        public JobRegistry()
        {
            Schedule<DeviceWarnDelayJob>().ToRunEvery(4).Seconds();
            Schedule<TerminalWarnDelayJob>().ToRunEvery(5).Seconds();
            Schedule<DeviceStateJob>().ToRunEvery(1).Minutes();
            Schedule<DeleteLogJob>().ToRunOnceAt(DateTime.Now.AddMinutes(1));
            Schedule<DeleteLogJob>().ToRunEvery(1).Days().At(3, 0);
        }
    }
}
