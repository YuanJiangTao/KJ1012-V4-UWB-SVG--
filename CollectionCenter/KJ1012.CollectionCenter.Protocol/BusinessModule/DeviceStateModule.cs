using System;
using System.Threading.Tasks;
using CachingFramework.Redis.Contracts.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KJ1012.CollectionCenter.Protocol.Protocol;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Core.Helper;
using KJ1012.Data.Entities.Warn;
using KJ1012.Domain.Setting;
using KJ1012.Services.IServices.Base;
using KJ1012.Services.IServices.Warn;
using System.Linq;
using KJ1012.Core.Data;
using KJ1012.Domain.Enums;
using Microsoft.Data.SqlClient;

namespace KJ1012.CollectionCenter.Protocol.BusinessModule
{
    public class DeviceStateModule : IGroupSubscribe<DeviceStateGroupModel>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptionsMonitor<Setting> _kj1012Setting;
        private readonly ILogger<DeviceStateModule> _logger;

        public DeviceStateModule(IServiceProvider serviceProvider,
            ICacheProvider cacheProvider,
            IOptionsMonitor<Setting> kj1012Setting,
            ILogger<DeviceStateModule> logger)
        {
            _serviceProvider = serviceProvider;
            _cacheProvider = cacheProvider;
            _kj1012Setting = kj1012Setting;
            _logger = logger;
        }

        public async Task ExecReceive(DeviceStateGroupModel protocolModel)
        {
            try
            {
                if (protocolModel != null)
                {
                    using (var serviceScope = _serviceProvider.CreateScope())
                    {

                        IDeviceService deviceService =
                            serviceScope.ServiceProvider.GetService<IDeviceService>();
                        IDeviceWarnService deviceWarnService =
                            serviceScope.ServiceProvider.GetService<IDeviceWarnService>();
                        var setting = _kj1012Setting.CurrentValue;
                        //修改设备最后监测时间
                        await deviceService.UpdateCheckTime(protocolModel.DeviceType, protocolModel.DeviceNum);

                        //如果配置容许屏蔽设备异常信息则屏蔽掉非电池供电和低电量状态
                        if (setting.CloseDeviceWarn)
                        {
                            if (protocolModel.DeviceState != 0)
                            {
                                _logger.LogInformation($"设备异常原始信息,{protocolModel.DeviceNum}:{protocolModel.DeviceState}");
                                var state = protocolModel.DeviceState & 5;
                                if (state == 0) return;
                                protocolModel.DeviceState = state;
                            }
                        }

                        if (protocolModel.DeviceState == 0)
                        {
                            await Success(deviceService, deviceWarnService, protocolModel);
                        }
                        else
                        {
                            await Error(serviceScope, deviceService, deviceWarnService, protocolModel);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }
        }

        private async Task Success(IDeviceService deviceService, IDeviceWarnService deviceWarnService,
            DeviceStateGroupModel protocolModel)
        {
            await Task.Delay(1000);
            if (_kj1012Setting.CurrentValue.IsUseDeviceLinkage2)
            {
                //恢复时判定所有上级是否存在断线情况，如果上级存在断线情况，下级不恢复
                var deviceList = await deviceService.BaseRepository.Table
                    .FromSqlRaw("EXEC GetParentErrorDevice @deviceNum", new SqlParameter("deviceNum", protocolModel.DeviceNum))
                    .ToListAsync();
                if (deviceList.Count > 0) return;
            }

            var device = await deviceService.UpdateStateAsync(protocolModel.DeviceType,
                protocolModel.DeviceNum, protocolModel.DeviceState);
            if (device != null)
            {
                await deviceWarnService.Recovery(device.Id);
            }
        }


        private async Task Error(IServiceScope serviceScope, IDeviceService deviceService, IDeviceWarnService deviceWarnService,
            DeviceStateGroupModel protocolModel)
        {
            async Task PublishAsync(Guid id)
            {
                var delayTimes = _kj1012Setting.CurrentValue.DeviceWarnDelay;
                //延迟推送大于10秒才执行延迟推送，否则延迟推送没什么意义,延迟推送则在数据处理端处理推送
                if (delayTimes > 0)
                {
                    //对应已恢复的部分状态实时恢复设备表状态
                    if (protocolModel.DeviceState != 2)
                    {
                        var device = await deviceService.BaseRepository.Table.FirstOrDefaultAsync(f => f.Id == id);

                        if (device != null)
                        {
                            if (_kj1012Setting.CurrentValue.IsUseDeviceLinkage2)
                            {
                                if ((device.DeviceState & 2) == 2)
                                {
                                    //恢复时判定所有上级是否存在断线情况，如果上级存在断线情况，下级不恢复
                                    var deviceList = await deviceService.BaseRepository.Table
                                        .FromSqlRaw("EXEC GetParentErrorDevice @deviceNum", new SqlParameter("deviceNum", protocolModel.DeviceNum))
                                        .ToListAsync();
                                    if (deviceList.Count > 0) return;
                                }
                            }
                            device.DeviceState = device.DeviceState & protocolModel.DeviceState;
                            device.LastCheckTime = DateTime.Now;
                            await deviceService.SaveAsync(device);
                        }
                    }
                }
                else
                {
                    await deviceService.UpdateStateAsync(id, protocolModel.DeviceState);
                }
            }

            //状态为2由上级单独上报，单独处理
            if (protocolModel.DeviceState == 2)
            {
                var warnDevice = deviceWarnService.BaseRepository.TableNoTracking.FirstOrDefault(f =>
                    f.Device.DeviceType == protocolModel.DeviceType &&
                    f.Device.DeviceNum == protocolModel.DeviceNum && !f.RecoveryTime.HasValue &&
                    f.DeviceState == protocolModel.DeviceState);
                if (warnDevice == null)
                {
                    async Task DeviceError()
                    {
                        var device = await deviceService.GetDeviceByTypeAndNumAsync(protocolModel.DeviceType,
                            protocolModel.DeviceNum);
                        if (device != null)
                        {
                            var newDeviceWarn = new DeviceWarn
                            {
                                DeviceId = device.Id,
                                DeviceState = 2,
                                DelayTime = _kj1012Setting.CurrentValue.DeviceWarnDelay
                            };
                            await deviceWarnService.SaveAsync(newDeviceWarn);
                            await PublishAsync(device.Id);
                        }
                    }
                    if (_kj1012Setting.CurrentValue.IsUseDeviceLinkage)
                    {
                        //基站和分站断线时，上级设备断线，下级设备全部处理为断线
                        if (protocolModel.DeviceType == DeviceTypeEnum.BaseStation ||
                            protocolModel.DeviceType == DeviceTypeEnum.Substation)
                        {
                            var deviceList = await deviceService.BaseRepository.Table
                                .FromSqlRaw("EXEC GetChildrenSuccessDevice @deviceNum", new SqlParameter("@deviceNum", protocolModel.DeviceNum))
                                .ToListAsync();

                            if (deviceList.Count == 0)
                            {
                                await DeviceError();
                            }
                            else
                            {
                                foreach (var device in deviceList)
                                {
                                    var newDeviceWarn = new DeviceWarn
                                    {
                                        DeviceId = device.Id,
                                        DeviceState = 2,
                                        DelayTime = _kj1012Setting.CurrentValue.DeviceWarnDelay
                                    };
                                    await deviceWarnService.BaseRepository.Table.AddAsync(newDeviceWarn);
                                }
                                var unitOfWork = serviceScope.ServiceProvider.GetService<IUnitOfWork>();
                                var result = await unitOfWork.SaveChangesAsync();
                                if (result > 0)
                                {
                                    foreach (var device in deviceList)
                                    {
                                        await PublishAsync(device.Id);
                                    }
                                }
                            }
                        }
                        else
                        {
                            await DeviceError();
                        }
                    }
                    else
                    {
                        await DeviceError();
                    }

                }
            }
            else
            {
                var deviceWarns = await deviceWarnService.BaseRepository.Table.Where(f =>
                    f.Device.DeviceType == protocolModel.DeviceType && f.DeviceState != -1 &&
                    f.Device.DeviceNum == protocolModel.DeviceNum && !f.RecoveryTime.HasValue).ToListAsync();
                if (deviceWarns.Count > 0)
                {
                    var totalOldState = deviceWarns.Sum(d => d.DeviceState);

                    if (totalOldState != protocolModel.DeviceState)
                    {
                        var totalState = totalOldState | protocolModel.DeviceState;

                        var bitString = CommonHelper.IntToBit(totalState).Reverse().ToArray();

                        var nowBitString = CommonHelper.IntToBit(protocolModel.DeviceState)
                            .PadLeft(bitString.Length, '0').Reverse().ToArray();
                        bool isPublish = false;
                        for (int i = 0; i < bitString.Length; i++)
                        {
                            if (bitString[i] == '1')
                            {
                                var deviceWarn = deviceWarns.FirstOrDefault(f =>
                                    f.DeviceState == Convert.ToInt32(Math.Pow(2, i)));
                                if (deviceWarn == null)
                                {
                                    var newDeviceWarn = new DeviceWarn
                                    {
                                        DeviceId = deviceWarns.First().DeviceId,
                                        DeviceState = Convert.ToInt32(Math.Pow(2, i))
                                    };
                                    await deviceWarnService.SaveAsync(newDeviceWarn);

                                    isPublish = true;
                                }
                                else
                                {
                                    //恢复异常
                                    if (nowBitString[i] == '0')
                                    {
                                        if (_kj1012Setting.CurrentValue.IsUseDeviceLinkage2)
                                        {
                                            if (deviceWarn.DeviceState == 2)
                                            {
                                                //恢复时判定所有上级是否存在断线情况，如果上级存在断线情况，下级不恢复
                                                var deviceList = await deviceService.BaseRepository.Table
                                                    .FromSqlRaw("EXEC GetParentErrorDevice @deviceNum", new SqlParameter("deviceNum", protocolModel.DeviceNum))
                                                    .ToListAsync();
                                                if (deviceList.Count > 0) return;
                                            }
                                        }
                                        deviceWarn.RecoveryTime = DateTime.Now;
                                        deviceWarn.RecoveryType = 0;
                                        deviceWarn.RecoveryRemark = "自动检测";
                                        await deviceWarnService.SaveAsync(deviceWarn);
                                        isPublish = true;
                                    }
                                }
                            }
                        }

                        if (isPublish)
                        {
                            await PublishAsync(deviceWarns.First().DeviceId.GetValueOrDefault());
                        }
                    }
                }
                else
                {
                    var device = await deviceService.GetDeviceByTypeAndNumAsync(protocolModel.DeviceType,
                        protocolModel.DeviceNum);

                    if (device != null)
                    {
                        var bitString = CommonHelper.IntToBit(protocolModel.DeviceState).Reverse().ToArray();
                        for (int i = 0; i < bitString.Length; i++)
                        {
                            if (bitString[i] == '1')
                            {
                                var newDeviceWarn = new DeviceWarn
                                {
                                    DeviceId = device.Id,
                                    DeviceState = Convert.ToInt32(Math.Pow(2, i))
                                };
                                await deviceWarnService.SaveAsync(newDeviceWarn);

                            }
                        }

                        await PublishAsync(device.Id);
                    }
                }
            }

        }

    }

}