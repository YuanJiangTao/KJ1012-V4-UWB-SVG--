using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Management;

namespace KJ1012.Core.Helper
{
    public static class OsHelper
    {

        public static async Task<double> GetCpuPercent()
        {
            var cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total") { MachineName = "." };
            cpu.NextValue();
            await Task.Delay(1000);
            var percentage = cpu.NextValue();
            return Math.Round(percentage, 2, MidpointRounding.AwayFromZero);
        }
        public static double GetMemoryPercent()
        {
            //剩余内存
            var ram1 = new PerformanceCounter("Memory", "Available MBytes", null);
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            var mPhysicalMemory = (long)16 * 1024 * 1024 * 1024;
            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                if (mo["TotalPhysicalMemory"] != null)
                {
                    mPhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
            var percentage1 = ram1.NextValue();
            mPhysicalMemory = mPhysicalMemory / 1024 / 1024;
            return Math.Round((mPhysicalMemory - percentage1) * 100 / mPhysicalMemory, 2, MidpointRounding.AwayFromZero);
        }
        public static dynamic GetDriveInfos()
        {
            //获取本地磁盘，判断网络磁盘及U盘等
            return DriveInfo.GetDrives().Where(w => w.DriveType == DriveType.Fixed).Select(s => new
            {
                s.Name,
                s.TotalSize,
                s.TotalFreeSpace,
                s.AvailableFreeSpace,
                s.VolumeLabel,
            }).ToList();
        }
    }
}
