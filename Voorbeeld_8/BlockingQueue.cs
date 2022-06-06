using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voorbeeld_8
{
    public sealed class BlockingQueue<T> :  IDisposable
    {
        bool _disposed;
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly Semaphore _pool = new Semaphore(0, int.MaxValue);
        private readonly object _lock = new object();

        public void Enqueue(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);
                _pool.Release();
            }
        }

        public T Dequeue()
        {
            _pool.WaitOne();
            lock (_lock)
            {
                return _queue.Dequeue();
            }
        }

        public T[] ToArray()
        {
            return _queue.ToArray();
        }     

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }        

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _queue?.Clear();               
            }
        }        
    }
}
