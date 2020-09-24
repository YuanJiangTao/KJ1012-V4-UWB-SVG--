using KJ1012.Core.Data;
using KJ1012.Data.Entities.Warn;
using KJ1012.Domain.Enums;
using KJ1012.Services.IServices.Base;
using KJ1012.Services.IServices.Warn;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using KJ1012.Domain.Setting;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

namespace KJ1012.Services.Services.Warn
{
    public class DeviceWarnService : BaseService<DeviceWarn>, IDeviceWarnService
    {
        private readonly IDeviceService _deviceService;
        private readonly IOptionsMonitor<Setting> _kj1012Setting;
        private readonly IUnitOfWork _unitOfWork;
        public DeviceWarnService(
            IDeviceService deviceService,
            IOptionsMonitor<Setting> kj1012Setting,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _deviceService = deviceService;
            _kj1012Setting = kj1012Setting;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Recovery(Guid deviceId, bool save = true)
        {
            var entities =
                await BaseRepository.Table.Where(f => f.DeviceId == deviceId && !f.RecoveryTime.HasValue).ToListAsync();
            entities.ForEach(entity =>
            {
                entity.RecoveryTime = DateTime.Now;
                entity.RecoveryType = 0;
                entity.RecoveryRemark = "自动检测";
            });
            if (save)
            {
                return await _unitOfWork.SaveChangesAsync();
            }

            return 0;
        }

        /// <summary>
        /// 将超过指定时间未收到过状态数据的基站状态改为未知
        /// </summary>
        /// <returns></returns>
        public async Task SetDeviceStateUnknown()
        {
            var setting = _kj1012Setting.CurrentValue;
            var interVal = setting.DeviceStateUnknownInterval;
            //屏蔽异常状态检查后，未知状态也不处理
            var isCloseDeviceWarn = setting.CloseDeviceWarn;
            //配置大于0则执行未知状态检测
            if (interVal > 0 && !isCloseDeviceWarn)
            {
                var isUseDeviceLinkage = _kj1012Setting.CurrentValue.IsUseDeviceLinkage;
                if (isUseDeviceLinkage)
                {
                    var unknownStateList = await _deviceService.BaseRepository.Table
                        .FromSqlRaw("EXEC GetChildrenUnKnownDevice @interVal", new SqlParameter("interVal", interVal))
                        .ToListAsync();

                    foreach (var device in unknownStateList)
                    {
                        _deviceService.BaseRepository.Table.Attach(device);
                        device.DeviceState = device.DeviceState + 2;
                        var newDeviceWarn = new DeviceWarn
                        {
                            DeviceId = device.Id,
                            DeviceState = 2,
                            DelayTime = 0
                        };
                        await BaseRepository.InsertAsync(newDeviceWarn);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    var unknownStateList = await _deviceService.BaseRepository.TableNoTracking
                        .Where(w => w.DeviceType == DeviceTypeEnum.BaseStation && (w.DeviceState & 2) != 2 &&
                                    (EF.Functions.DateDiffMinute(w.LastCheckTime, DateTime.Now) > interVal
                                     || !w.LastCheckTime.HasValue))
                        .ToListAsync();
                    foreach (var device in unknownStateList)
                    {

                        _deviceService.BaseRepository.Table.Attach(device);
                        device.DeviceState = device.DeviceState + 2;
                        var newDeviceWarn = new DeviceWarn
                        {
                            DeviceId = device.Id,
                            DeviceState = 2,
                            DelayTime = 0
                        };
                        await BaseRepository.InsertAsync(newDeviceWarn);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}
