namespace KJ1012.CollectionCenter.SocketSend
{
    public interface ISocketSendServer
    {
        void SendMessage(string id, byte[] bytes, bool isAddCheck = true);
        void SendMessage(string id, string message, bool isAddCheck = true);
        void SendMessage(int deviceType, int deviceNum, byte[] bytes, bool isAddCheck = true);
    }
}
