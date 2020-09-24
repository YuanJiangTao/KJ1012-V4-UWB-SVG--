using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Core.Caching;
using KJ1012.Core.Helper;
using KJ1012.Core.Infrastructure;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public class PositionGroupProtocol : BaseProtocol<PositionGroupModel>
    {
        private readonly IEngine _engine;
        private readonly IStaticCacheManager _cacheManager;

        public override int ProtocolLength => 10;

        public override int ProtocolId => 1;
        public PositionGroupProtocol(IEngine engine, IStaticCacheManager cacheManager,
            ITypeFinder typeFinder) : base(engine, typeFinder)
        {
            _engine = engine;
            _cacheManager = cacheManager;
        }

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureServices<MemberPositionGroupModel>(services, configuration);
            base.ConfigureServices(services, configuration);
        }

        protected override PositionGroupModel ExecProtocolParsing(byte[] receiveBytes)
        {
            return new PositionGroupModel
            {
                ProtocolId = ProtocolId,
                TerminalState = receiveBytes[1],
                TerminalId = CommonHelper.BytesToInt(receiveBytes[2], receiveBytes[3]),
                PositionWay = receiveBytes[4],
                Station = CommonHelper.BytesToInt(receiveBytes[5], receiveBytes[6]),
                Distance = CommonHelper.BytesToInt(receiveBytes[7], receiveBytes[8]),
                PositionTime = DateTime.Now
            };
        }

        protected override void PublishToModule(PositionGroupModel locationGroupModel)
        {
            //定位数据为空时直接返回
            if (locationGroupModel == null) return;
            //判断内存缓存是否有刚处理过该标识卡号的定位数据，如果有则不再处理
            var isExist = _cacheManager.Get<int>(locationGroupModel.TerminalId.ToString());
            if (isExist != 0) return;
            _cacheManager.Set(locationGroupModel.TerminalId.ToString(), locationGroupModel.TerminalId, 5);
            var groupSubscribe = _engine.GetServices<IGroupSubscribe<PositionGroupModel>>();
            foreach (var groupReceiveModule in groupSubscribe)
            {
                //异步方法同步执行
                groupReceiveModule.ExecReceive(locationGroupModel);
            }
        }
    }
}

