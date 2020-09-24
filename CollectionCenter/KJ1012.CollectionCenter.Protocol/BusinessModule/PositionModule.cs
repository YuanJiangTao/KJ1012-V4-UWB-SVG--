using Microsoft.Extensions.Logging;
using KJ1012.CollectionCenter.Protocol.Protocol;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain.Enums;
using KJ1012.Domain.Setting;
using Microsoft.Extensions.Options;
using KJ1012.Core.Mapper;

namespace KJ1012.CollectionCenter.Protocol.BusinessModule
{
    public class PositionModule : IGroupSubscribe<PositionGroupModel>
    {
        private readonly IEngine _engine;
        private readonly IOptionsMonitor<Setting> _kj1012Setting;
        private readonly ILogger<PositionModule> _logger;


        public PositionModule(IEngine engine,
            IOptionsMonitor<Setting> kj1012Setting,
            ILogger<PositionModule> logger)
        {
            _engine = engine;
            _kj1012Setting = kj1012Setting;
            _logger = logger;
        }


        public async Task ExecReceive(PositionGroupModel protocolModel)
        {
            try
            {
                var terminalIdType = GetTerminalIdType(protocolModel.TerminalId);
                switch (terminalIdType)
                {
                    case TerminalTypeEnum.Member:
                        {
                            await ExecMemberPosition(protocolModel);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }
        }
        private async Task ExecPosition<T>(PositionGroupModel protocolModel) where T : PositionGroupModel
        {
            if (protocolModel != null)
            {
                var positionGroupModel = protocolModel.MapTo<PositionGroupModel, T>();
                var groupSubscribe = _engine.GetServices<IGroupSubscribe<T>>()
                    .Where(w => w.GetType() != typeof(PositionModule))
                    .ToList();
                foreach (var groupReceiveModule in groupSubscribe)
                {
                    //异步方法同步执行
                    _ = groupReceiveModule.ExecReceive(positionGroupModel);
                }
            }
        }
        /// <summary>
        /// 执行人员轨迹业务逻辑
        /// </summary>
        private async Task ExecMemberPosition(PositionGroupModel protocolModel)
        {
            await ExecPosition<MemberPositionGroupModel>(protocolModel);
        }

        private TerminalTypeEnum GetTerminalIdType(int terminalId)
        {
            var setting = _kj1012Setting.CurrentValue.TerminalIdRange;
            var numberRange = setting.FirstOrDefault(w => w.Value.Min <= terminalId && w.Value.Max >= terminalId);
            int.TryParse(numberRange.Key, out var key);
            if (key >0)
            {
                return (TerminalTypeEnum)key;
            }

            return TerminalTypeEnum.Member;
        }
    }
}
