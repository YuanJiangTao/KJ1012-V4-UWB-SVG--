using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain;
using System.Linq;

namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public abstract class BaseProtocol<TModel> : IProtocol where TModel : BaseProtocolModel

    {
        private readonly IEngine _engine;
        private readonly ITypeFinder _typeFinder;

        protected BaseProtocol(IEngine engine, ITypeFinder typeFinder)
        {
            _engine = engine;
            _typeFinder = typeFinder;
        }

        public abstract int ProtocolLength { get; }
        public abstract int ProtocolId { get; }

        public virtual bool IsMatch(byte[] bytes)
        {
            return bytes.Length >= ProtocolLength && bytes[0] == ProtocolId &&
                   bytes[ProtocolLength - 1] == ConstDefine.CheckUserId;
        }
        protected void ConfigureServices<T>(IServiceCollection services, IConfiguration configuration) where T : BaseProtocolModel
        {
            var baseServices = _typeFinder.FindClassesOfType(typeof(IGroupSubscribe<T>));
            foreach (var baseService in baseServices)
            {
                services.AddTransient(typeof(IGroupSubscribe<T>), baseService);
            }
        }

        //注入所有实现了IGroupSubscribe的业务模块对象
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var baseServices = _typeFinder.FindClassesOfType(typeof(IGroupSubscribe<TModel>));
            foreach (var baseService in baseServices)
            {
                services.AddTransient(typeof(IGroupSubscribe<TModel>), baseService);
            }
        }

        public virtual byte[] Receive(byte[] bytes)
        {
            if (!IsMatch(bytes)) return bytes;

            var locationGroupModel = ExecProtocolParsing(bytes.Take(ProtocolLength).ToArray());
            PublishToModule(locationGroupModel);
            return bytes.Skip(ProtocolLength).ToArray();
        }
        public virtual byte[] Receive(string address, byte[] bytes)
        {
            if (!IsMatch(bytes)) return bytes;

            var locationGroupModel = ExecProtocolParsing(address, bytes);
            PublishToModule(locationGroupModel);
            return null;
        }
        protected abstract TModel ExecProtocolParsing(byte[] bytes);

        protected virtual TModel ExecProtocolParsing(string address, byte[] bytes)
        {
            return null;
        }

        protected virtual void PublishToModule(TModel groupModel)
        {
            var groupSubscribe = _engine.GetServices<IGroupSubscribe<TModel>>();
            foreach (var groupReceiveModule in groupSubscribe)
            {
                //异步方法同步执行
                groupReceiveModule.ExecReceive(groupModel);
            }
        }

    }
}
