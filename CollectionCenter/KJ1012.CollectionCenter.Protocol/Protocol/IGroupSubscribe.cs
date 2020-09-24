using System.Threading.Tasks;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;

namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public interface IGroupSubscribe<in TModel> where TModel : BaseProtocolModel
    {
        Task ExecReceive(TModel protocolModel);
    }
}
