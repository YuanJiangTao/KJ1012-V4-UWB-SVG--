using System;
using System.Threading.Tasks;
using KJ1012.Data.Entities.Warn;

namespace KJ1012.Services.IServices.Warn
{
    public interface IDeviceWarnService : IBaseService<DeviceWarn>
    {
        Task<int> Recovery(Guid deviceId, bool save = true);
        Task SetDeviceStateUnknown();
    }
}