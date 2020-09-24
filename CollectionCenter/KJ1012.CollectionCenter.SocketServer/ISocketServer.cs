using System.Net;
using KJ1012.Domain;

namespace KJ1012.CollectionCenter.SocketService
{
    public interface ISocketServer
    {

        event SocketServer.OnClientClose ClientClose;
        event SocketServer.OnClientConnected ClientConnected;
        event SocketServer.OnError Error;
        event SocketServer.OnListening Listening;
        event SocketServer.OnReceiveData ReceiveData;

        void Init();
        void SetKeepAlive(bool isUseKeepAlive, int keepalivetime, int keepaliveinterval);
        bool Start(IPEndPoint localEndPoint);
        void Stop();
        void CloseClient(UserToken token);
    }
}