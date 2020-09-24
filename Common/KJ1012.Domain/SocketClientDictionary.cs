using System.Collections.Concurrent;

namespace KJ1012.Domain
{
    public class SocketClientDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, ISocketClientDictionary<TKey, TValue>
    {
    }
}
