using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Core.Mapper;
using KJ1012.Data.Entities.Position;
using KJ1012.Data.Views;
using KJ1012.Domain.Enums;
using KJ1012.Services.IServices.Position;
using System;
using System.Threading.Tasks;
using CachingFramework.Redis.Contracts.Providers;
using KJ1012.Domain.Setting;
using KJ1012.Services.IServices.View;
using Microsoft.Extensions.Options;

namespace KJ1012.CollectionCenter.Protocol.BusinessModule
{
    public abstract class BaseMemberPositionModule<T> where T : BaseProtocolModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptionsMonitor<Setting> _kj1012Setting;
        private readonly ILogger<BaseMemberPositionModule<T>> _logger;

        protected BaseMemberPositionModule(IServiceProvider serviceProvider,
            ICacheProvider cacheProvider,
            IOptionsMonitor<Setting> kj1012Setting,
            ILogger<BaseMemberPositionModule<T>> logger)
        {
            _serviceProvider = serviceProvider;
            _cacheProvider = cacheProvider;
            _kj1012Setting = kj1012Setting;
            _logger = logger;
        }

        protected async Task BasePositionReceive(T protocolModel, int terminalId)
        {
            try
            {
                if (protocolModel != null)
                {
                    using (var serviceScope = _serviceProvider.CreateScope())
                    {
                        async Task<Position> SavePosition()
                        {
                            var locationRecord = MapToPosition(protocolModel);
                            if (((locationRecord.Distance >> 4) & 7) > 1)
                            {
                                locationRecord.Distance = 0;
                            }

                            //对第一个参考站为0的数据不存入历史数据表，但需执行标识卡报警处理
                            if (locationRecord.Station != 0)
                            {
                                var service =
                                    serviceScope.ServiceProvider.GetService<IPositionService>();
                                locationRecord.NextStation = 0;
                                await service.SaveAsync(locationRecord);
                            }

                            return locationRecord;
                        }

                        var entity = await GetUsedEntityAsync(serviceScope, terminalId);
                        if (entity != null)
                        {
                            var locationRecord = await SavePosition();
                            await UpdatePosition(locationRecord, serviceScope);
                        }
                        else
                        {
                            //未录入系统的标识卡号只记录定位数据，不做考勤及数据推送
                            await SavePosition();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }
        }


        private Position MapToPosition(T groupModel)
        {
            return groupModel.MapTo<T, Position>();
        }

        private async Task<ViewTerminal> GetUsedEntityAsync(IServiceScope serviceScope, int terminalId)
        {
            var entity = await _cacheProvider.GetObjectAsync<ViewTerminal>($"cache:{terminalId}");
            if (entity != null) return entity;
            var viewTerminalService = serviceScope.ServiceProvider.GetService<IViewTerminalService>();
            entity = await viewTerminalService.GetEntityAsync(terminalId);
            if (entity != null)
            {
                await _cacheProvider.SetObjectAsync($"cache:{terminalId}", entity, TimeSpan.FromHours(1));
            }
            else
            {
                var setting = _kj1012Setting.CurrentValue;
                if (setting.AllowNotInsert)
                    return new ViewTerminal
                    {
                        TerminalId = terminalId,
                        Type = TerminalTypeEnum.Member
                    };
            }

            return entity;
        }

        private DownMember PositionMapTo(Position groupModel)
        {
            return groupModel.MapTo<Position, DownMember>();
        }

        /// <summary>
        /// 修改井下人员定位实时数据
        /// 定位数据只做更新井下人员实时数据处理，入井判断只根据考勤数据来做出井下人员新增和删除
        /// </summary>
        /// <param name="positionRecord">定位数据</param>
        /// <param name="serviceScope"></param>
        /// <returns></returns>
        private async Task UpdatePosition(Position positionRecord, IServiceScope serviceScope)
        {
            try
            {
                if (positionRecord != null)
                {
                    var entity = await _cacheProvider.FetchHashedAsync("downMembers:hash",
                        $"terminalId:{positionRecord.TerminalId}", async () =>
                        {
                            var downMemberService =
                                serviceScope.ServiceProvider.GetService<IDownMemberService>();
                            return await downMemberService.GetEntityAsync(positionRecord.TerminalId);
                        });
                    if (entity != null)
                    {
                        if (positionRecord.ReceiveFrom == 6 || positionRecord.ReceiveFrom == 8)
                        {
                            if (entity.PositionTime > positionRecord.PositionTime ||
                                positionRecord.PositionTime > DateTime.Now) return;
                        }

                        await UpdateDownMember(serviceScope, positionRecord);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }
        }

        private async Task UpdateDownMember(IServiceScope serviceScope, Position positionRecord)
        {
            var downMemberService = serviceScope.ServiceProvider.GetService<IDownMemberService>();
            var downMember = PositionMapTo(positionRecord);
            var result = await downMemberService.UpdateAsync(downMember, positionRecord);
            if (result > 0)
            {
                await _cacheProvider.SetHashedAsync("downMembers:hash", $"terminalId:{downMember.TerminalId}",
                    downMember);
            }
        }
    }
}
