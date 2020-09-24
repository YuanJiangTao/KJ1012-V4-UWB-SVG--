using Microsoft.EntityFrameworkCore;
using KJ1012.Core.Data;
using KJ1012.Data.Entities.Base;
using KJ1012.Domain;
using KJ1012.Domain.Enums;
using KJ1012.Services.IServices.Base;
using KJ1012.Services.Publish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KJ1012.Core.Exceptions;
using CachingFramework.Redis.Contracts.Providers;

namespace KJ1012.Services.Services.Base
{
    public class DeviceService : BaseService<Device>, IDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDevicePublish _devicePublish;
        private readonly ICacheProvider _cacheProvider;

        public DeviceService(IUnitOfWork unitOfWork,
            IDevicePublish devicePublish,
            ICacheProvider cacheProvider) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _devicePublish = devicePublish;
            _cacheProvider = cacheProvider;
        }

        public async Task<int> GetCacheStation(int station)
        {
            try
            {
                var device = await _cacheProvider.FetchObjectAsync($"devices:{station}", async () =>
                        await BaseRepository.TableNoTracking.Where(w => w.DeviceNum == station && w.DeviceType == DeviceTypeEnum.BaseStation)
                            .Select(s => s.DeviceNum).FirstOrDefaultAsync(),
                    TimeSpan.FromHours(new Random().Next(3, 6)));
                return device;
            }
            catch (Exception e)
            {
                return station;
            }
        }

        public async Task<int> UpdateCheckTime(DeviceTypeEnum deviceType, int deviceNum)
        {
            var device =
               await BaseRepository.Table.FirstOrDefaultAsync(f => f.DeviceType == deviceType && f.DeviceNum == deviceNum);
            if (device != null)
            {
                device.LastCheckTime = DateTime.Now;
                return await _unitOfWork.SaveChangesAsync();
            }

            return 0;
        }

        public IQueryable<Device> GetListByDeviceType(params DeviceTypeEnum[] types)
        {
            return BaseRepository.TableNoTracking.Where(r => types.Contains(r.DeviceType)).OrderBy(r => r.DeviceNum);
        }


        public IQueryable<Device> GetDevicesByTypeTrack(params DeviceTypeEnum[] types)
        {
            return BaseRepository.Table.Where(r => types.Contains(r.DeviceType)).OrderBy(r => r.DeviceNum);
        }

        public async Task<ResultEx> SaveEntityAsync(Device entity)
        {
            //分站程序编号和序号保持一致
            if (entity.DeviceType == DeviceTypeEnum.Substation)
            {
                if (entity.DeviceNum < 1 || entity.DeviceNum > 64)
                {
                    return ResultEx.Init(false, "分站编号只能为1~64");
                }
                entity.SerialNum = entity.DeviceNum;
            }
            if (!(entity.DeviceType == DeviceTypeEnum.BaseStation || entity.DeviceType == DeviceTypeEnum.BeaconCard))
            {
                entity.SubstationId = null;
            }
            else
            {
                if (entity.DeviceNum < 1 || entity.DeviceNum > 65535)
                {
                    return ResultEx.Init(false, "基站编号只能为1~65535");
                }
                if (entity.SerialNum.GetValueOrDefault(0) > 64 || entity.SerialNum.GetValueOrDefault(0) < 1)
                {
                    return ResultEx.Init(false, "设备序号只能为1~64的数字");
                }
            }

            if (entity.Id == Guid.Empty)
            {
                // 查看相同设备类型下的编号是否已经使用
                var existEntity = await BaseRepository.TableNoTracking.FirstOrDefaultAsync(r =>
                     r.DeviceType == entity.DeviceType && r.DeviceNum == entity.DeviceNum);

                if (existEntity != null)
                    return ResultEx.Init(false, "该设备类型指定的编号已经存在,请使用其它编号");
                //查看基站和定位器序号是否已经使用
                if (entity.DeviceType == DeviceTypeEnum.BaseStation || entity.DeviceType == DeviceTypeEnum.BeaconCard)
                {
                    //基站和定位器编号不能重复
                    existEntity = await BaseRepository.TableNoTracking.FirstOrDefaultAsync(r =>
                        (r.DeviceType == DeviceTypeEnum.BaseStation || r.DeviceType == DeviceTypeEnum.BeaconCard) && r.DeviceNum == entity.DeviceNum);
                    if (existEntity != null)
                        return ResultEx.Init(false, "编号在基站或者定位器中已使用，请换其他编号");

                    //判断相同类型设备同一分站下序号是否重复
                    existEntity = await BaseRepository.TableNoTracking.FirstOrDefaultAsync(r =>
                        r.DeviceType == entity.DeviceType && r.SubstationId == entity.SubstationId && r.SerialNum == entity.SerialNum);

                    if (existEntity != null)
                        return ResultEx.Init(false, "设备序号已经使用，请换其它序号");
                }
                //基站和分站新增时状态处理为未知
                if (entity.DeviceType == DeviceTypeEnum.BaseStation || entity.DeviceType == DeviceTypeEnum.Substation)
                {
                    entity.DeviceState = -1;
                }
                entity.SysStartTime = DateTime.Now;
                entity.SysEndTime = DateTime.MaxValue;
                //新增
                await BaseRepository.InsertAsync(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0) return ResultEx.Init(false, "保存失败");

                await _cacheProvider.RemoveAsync($"devices:{entity.DeviceNum}");
                //新增时同步将数据推送到mqtt，下发到下位机，编辑时不下发
                //因为保存进来的数据没有设备所属分站信息，所有这里需要从数据库重新查询一次
                if (entity.SubstationId != null && entity.SubstationId != Guid.Empty)
                    entity.Substation = await BaseRepository.GetByKeys(entity.SubstationId);
#pragma warning disable 4014
                _devicePublish.AddPublishAsync(entity);
#pragma warning restore 4014

            }
            else
            {
                //修改时候讲分站对象赋值空，否则，修改时候会循环多次修改相关数据
                entity.Substation = null;
                var newDevice = BaseRepository.Table.Update(entity);
                //设备状态和设备状态监测时间是不能修改的
                newDevice.Property(r => r.DeviceState).IsModified = false;
                newDevice.Property(r => r.LastCheckTime).IsModified = false;
                newDevice.Property(r => r.CreateDate).IsModified = false;
                newDevice.Property(r => r.SysStartTime).IsModified = false;
                newDevice.Property(r => r.SysEndTime).IsModified = false;
                int result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0) return ResultEx.Init(false, "保存失败");
            }
            return ResultEx.Init();
        }

        public async Task<Device> UpdateStateAsync(DeviceTypeEnum deviceType, int deviceNum, int deviceState)
        {
            var entity =
              await BaseRepository.Table.FirstOrDefaultAsync(r => r.DeviceType == deviceType && r.DeviceNum == deviceNum);
            if (entity != null)
            {
                //如果是通讯异常报警，该项由上级设备上报状态，顾累加到现有状态上
                if (deviceState == 2 && (entity.DeviceState & 2) != 2)
                {
                    entity.DeviceState += 2;
                }
                else
                {
                    entity.DeviceState = deviceState;
                }
                entity.LastCheckTime = DateTime.Now;
                await _unitOfWork.SaveChangesAsync();
            }
            return entity;
        }
        public async Task<Device> UpdateStateAsync(Guid id, int deviceState)
        {
            var entity =
                await BaseRepository.Table.FirstOrDefaultAsync(r => r.Id == id);
            if (entity != null)
            {
                //如果是通讯异常报警，该项由上级设备上报状态，顾累加到现有状态上
                if (deviceState == 2 && (entity.DeviceState & 2) != 2)
                {
                    entity.DeviceState += 2;
                }
                else
                {
                    entity.DeviceState = deviceState;
                }
                entity.LastCheckTime = DateTime.Now;
                await _unitOfWork.SaveChangesAsync();
            }
            return entity;
        }
        /// <summary>
        /// 根据分站id回去分站下的基站列表
        /// </summary>
        /// <param name="substationIds"></param>
        /// <returns></returns>
        public IQueryable<Device> GetBaseStationBySub(params int[] substationIds)
        {
            return BaseRepository.TableNoTracking
                .Where(r => substationIds.Contains(r.Substation.DeviceNum) && r.DeviceType == DeviceTypeEnum.BaseStation)
                .OrderBy(r => r.DeviceNum);
        }
        /// <summary>
        /// 获取分站下制定类型设备
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="substation"></param>
        /// <returns></returns>
        public IQueryable<Device> GetDevicesByTypeAndSubNum(DeviceTypeEnum deviceType, int substation)
        {
            return BaseRepository.TableNoTracking.Where(r =>
                    r.DeviceType == deviceType && r.Substation.DeviceNum == substation)
                .OrderBy(o => o.SerialNum);
        }
        /// <summary>
        /// 批量更新分站下所有基站的设备状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<List<Device>> BatchUpdateStateAsync(IList<(int Index, int[] States)> list)
        {
            //修改所有分站下的所有基站
            var baseStationS = GetDevicesByTypeTrack(DeviceTypeEnum.BaseStation);
            var devices = new List<Device>();
            foreach (var deviceStates in list)
            {
                var hasSubDevices =
                    baseStationS.Where(r => r.Substation.DeviceNum == deviceStates.Index);
                await hasSubDevices.ForEachAsync(d =>
                {
                    if (!d.SerialNum.HasValue) return;
                    //基站状态为二进制数据11时，表示分站尚未获取到该基站状态更新，上位机忽略改基站的状态更新
                    if (deviceStates.States[d.SerialNum.Value - 1] == 0b11) return;
                    //基站序号1-64，数组下标0-63,这里要减1；
                    d.DeviceState = deviceStates.States[d.SerialNum.Value - 1];
                    //演示临时处理，屏蔽所有设备异常数据
                    //d.DeviceState = 0;
                    d.LastCheckTime = DateTime.Now;
                    devices.Add(d);
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return devices;
        }

        public async Task<Device> GetDeviceByTypeAndNumAsync(DeviceTypeEnum deviceType, int deviceNum)
        {
            return await BaseRepository.TableNoTracking.Include(r => r.Substation).FirstOrDefaultAsync(r =>
                  r.DeviceType == deviceType && r.DeviceNum == deviceNum);
        }

        public override async Task<int> DeleteAsync(Device entity)
        {
            using (var transaction = _unitOfWork.GetDbContext().Database.BeginTransaction())
            {
                if (entity.DeviceType == DeviceTypeEnum.Substation)
                {
                    var list = await BaseRepository.Table.Where(r => r.SubstationId == entity.Id).ToListAsync();
                    if (list.Count > 0)
                    {
                        throw new CustomException("请先删除下属的基站及信标卡设备再删除分站设备");
                    }
                }

                try
                {
                    BaseRepository.Delete(entity);
                    //删除设备报警表为恢复的报警数据

                    var result = await _unitOfWork.SaveChangesAsync();
                    transaction.Commit();
                    if (result > 0)
                    {
                        //同步将数据推送到mqtt，下发到下位机
                        if (entity.SubstationId != null && entity.SubstationId != Guid.Empty)
                            entity.Substation = await BaseRepository.GetByKeys(entity.SubstationId);
#pragma warning disable 4014
                        _devicePublish.DeletePublishAsync(entity);
#pragma warning restore 4014
                    }
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }
    }
}
