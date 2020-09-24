using CachingFramework.Redis.Contracts.Providers;
using KJ1012.Core.Data;
using KJ1012.Data.Entities.Position;
using KJ1012.Services.IServices.Base;
using KJ1012.Services.IServices.Position;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KJ1012.Domain.Setting;
using Microsoft.Extensions.Options;

namespace KJ1012.Services.Services.Position
{
    public class DownMemberService : BaseService<DownMember>, IDownMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheProvider _cacheProvider;
        private readonly IDeviceService _deviceService;
        private readonly IOptionsMonitor<Setting> _kj1012Setting;

        public DownMemberService(IUnitOfWork unitOfWork,
            ICacheProvider cacheProvider,
            IDeviceService deviceService,
            IOptionsMonitor<Setting> kj1012Setting) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cacheProvider = cacheProvider;
            _deviceService = deviceService;
            _kj1012Setting = kj1012Setting;
        }

        public async Task<DownMember> GetEntityAsync(int terminalId)
        {
            return await BaseRepository.TableNoTracking.FirstOrDefaultAsync(r => r.TerminalId == terminalId);
        }

        public async Task<int> UpdateAsync(DownMember entity, Data.Entities.Position.Position positionRecord)
        {
            if (entity.Station != 0)
            {
                //判定轨迹里的基站是否已经录入
                var isAttendanceCheckStation = _kj1012Setting.CurrentValue.IsAttendanceCheckStation;
                if (isAttendanceCheckStation)
                {
                    //未录入基站不更新人员信息，避免误码数据导致数据显示错误
                    var station = await _deviceService.GetCacheStation(entity.Station.GetValueOrDefault());
                    if (station == 0) return 0;
                }
                //更新井下人员实时位置
                var downMember = BaseRepository.Table.Update(entity);
                //当距离中存在大于5000的数据只更新定位时间，不更新位置信息
                if (entity.Distance > 5000)
                {
                    downMember.Property(r => r.Station).IsModified = false;
                    downMember.Property(r => r.Distance).IsModified = false;
                    downMember.Property(r => r.Direction).IsModified = false;


                    downMember.Property(r => r.DataFrom).IsModified = false;
                    downMember.Property(r => r.NextStation).IsModified = false;
                }
                downMember.Property(r => r.Id).IsModified = false;
                downMember.Property(r => r.CreateDate).IsModified = false;
                //设置班次Id不可更改，解决定位数据来的classId为空时，修改数据时会将ClassId修改为空的情况。
                downMember.Property(r => r.ClassId).IsModified = false;
                int result;
                try
                {
                    result = await _unitOfWork.SaveChangesAsync();
                }
                catch
                {
                    //更新失败，说明井下没有该人员，删除缓存
                    await RemoveToRedisCache(entity.TerminalId);
                    return 0;
                }
                return result;
            }
            else
            {
                //参考站为0时只更新定位时间
                var downMember = BaseRepository.Table.Attach(entity);
                downMember.Property(p => p.PositionTime).IsModified = true;
                int result;
                try
                {
                    result = await _unitOfWork.SaveChangesAsync();
                }
                catch
                {
                    //更新失败，说明井下没有该人员，删除缓存
                    await RemoveToRedisCache(entity.TerminalId);
                    return 0;
                }
                return result;
            }
        }
        public async Task RemoveToRedisCache(int terminalId)
        {
            await _cacheProvider.RemoveHashedAsync("downMembers:hash", $"terminalId:{terminalId}");
            await _cacheProvider.RemoveFromSortedSetAsync("downMembers:terminalId", terminalId);
            await _cacheProvider.RemoveHashedAsync("LastStation:hash", $"terminalId:{terminalId}");
            await _cacheProvider.RemoveHashedAsync("downMembers:device", $"terminalId:{terminalId}");

            await _cacheProvider.RemoveAsync($"cache:{terminalId}");
        }
    }
}
