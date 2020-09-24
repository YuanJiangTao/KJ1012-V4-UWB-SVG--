﻿using System;
using System.Threading.Tasks;
using CachingFramework.Redis.Contracts.Providers;
using Microsoft.Extensions.Logging;
using KJ1012.CollectionCenter.Protocol.Protocol;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Domain.Enums;
using Microsoft.Extensions.Options;
using KJ1012.Domain.Setting;

namespace KJ1012.CollectionCenter.Protocol.BusinessModule
{
    public class MemberPositionContinueModule : BaseMemberPositionModule<MemberPositionContinueGroupModel>, IGroupSubscribe<MemberPositionContinueGroupModel>
    {
        private readonly ILogger<MemberPositionContinueModule> _logger;

        public MemberPositionContinueModule(IServiceProvider serviceProvider,
            ICacheProvider cacheProvider,
            IOptionsMonitor<Setting> kj1012Setting,
            ILogger<MemberPositionContinueModule> logger) : base(serviceProvider, cacheProvider, kj1012Setting, logger)
        {
            _logger = logger;
        }

        public async Task ExecReceive(MemberPositionContinueGroupModel protocolModel)
        {
            try
            {
                if (protocolModel != null)
                {
                    var upOrDown = (UpOrDownEnum)(protocolModel.PositionWay >> 7);
                    if (upOrDown == UpOrDownEnum.Down)
                    {
                        await BasePositionReceive(protocolModel,protocolModel.TerminalId);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }
        }

    }
}
