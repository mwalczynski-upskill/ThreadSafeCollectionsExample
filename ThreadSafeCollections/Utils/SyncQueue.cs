namespace ThreadSafeCollections.Utils
{
    using System.Collections.Generic;
    using System.Threading;

    public class SyncQueue<T>
    {
        private readonly object _sync = new object();
        private readonly Queue<T> _queue = new Queue<T>();

        public void Enqueue(T item)
        {
            lock (_sync)
            {
                _queue.Enqueue(item);
                Monitor.PulseAll(_sync);
            }
        }

        public T Dequeue()
        {
            lock (_sync)
            {
                while (_queue.Count == 0)
                {
                    Monitor.Wait(_sync);
                }

                return _queue.Dequeue();
            }
        }

        public int Count()
        {
            lock (_sync)
            {
                return _queue.Count;
            }
        }
    }
}
