using System;
using System.Linq;
using System.Threading.Tasks;
using KJ1012.Data.Entities.Base;
using KJ1012.Domain.Enums;

namespace KJ1012.Services.IServices.Base
{
    public interface IDeviceService : IBaseService<Device>
    {
        IQueryable<Device> GetListByDeviceType(params DeviceTypeEnum[] types);
        Task<Device> UpdateStateAsync(DeviceTypeEnum deviceType, int deviceNum, int deviceState);
        Task<Device> UpdateStateAsync(Guid id, int deviceState);
        IQueryable<Device> GetDevicesByTypeAndSubNum(DeviceTypeEnum deviceType, int substation);
        Task<Device> GetDeviceByTypeAndNumAsync(DeviceTypeEnum deviceType, int deviceNum);

        Task<int> GetCacheStation(int station);

        Task<int> UpdateCheckTime(DeviceTypeEnum deviceType, int deviceNum);
    }
}