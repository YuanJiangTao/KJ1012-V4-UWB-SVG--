using System.Net.Sockets;

namespace KJ1012.CollectionCenter.SocketService
{
    public interface IBufferManager
    {
        void FreeBuffer(SocketAsyncEventArgs args);
        void InitBuffer();
        bool SetBuffer(SocketAsyncEventArgs args);
    }
}