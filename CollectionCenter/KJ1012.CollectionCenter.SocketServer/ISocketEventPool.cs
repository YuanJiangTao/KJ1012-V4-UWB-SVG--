using System.Net.Sockets;

namespace KJ1012.CollectionCenter.SocketService
{
    public interface ISocketEventPool
    {
        int Count { get; }

        void Clear();
        SocketAsyncEventArgs Pop();
        void Push(SocketAsyncEventArgs item);
    }
}