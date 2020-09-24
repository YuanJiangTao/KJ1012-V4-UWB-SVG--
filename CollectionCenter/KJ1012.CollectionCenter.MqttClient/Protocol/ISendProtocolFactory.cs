namespace KJ1012.CollectionCenter.MqttClient.Protocol
{
    public interface ISendProtocolFactory
    {
        ISendProtocol Create(int protocolId);
    }
}