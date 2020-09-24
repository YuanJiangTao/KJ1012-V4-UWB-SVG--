namespace KJ1012.CollectionCenter.MqttClient.Protocol
{
    public interface ISendProtocol
    {
        bool IsMatch(int protocolId);

        void Receive(byte[] bytes);
    }
}