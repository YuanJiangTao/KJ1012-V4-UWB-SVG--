using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public interface IProtocol
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
        bool IsMatch(byte[] bytes);
        byte[] Receive(byte[] bytes);
        byte[] Receive(string address, byte[] bytes);
        int ProtocolLength { get; }

        int ProtocolId { get; }
    }
}