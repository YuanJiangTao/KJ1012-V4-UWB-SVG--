namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public interface IProtocolFactory
    {
        IProtocol Create(byte[] bytes);
    }
}