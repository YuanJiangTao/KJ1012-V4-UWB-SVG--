using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace KJ1012.CollectionCenter.SocketService
{
   public class SocketEventPool : ISocketEventPool
    {
        private readonly Stack<SocketAsyncEventArgs> _pool;


        public SocketEventPool(int capacity)
        {
            _pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentNullException(
                $"Items added to a SocketAsyncEventArgsPool cannot be null"); }
            lock (_pool)
            {
                _pool.Push(item);
            }
        }

        // Removes a SocketAsyncEventArgs instance from the pool
        // and returns the object removed from the pool
        public SocketAsyncEventArgs Pop()
        {
            lock (_pool)
            {
                return _pool.Pop();
            }
        }

        // The number of SocketAsyncEventArgs instances in the pool
        public int Count => _pool.Count;

        public void Clear()
        {
            _pool.Clear();
        }
    }
}
