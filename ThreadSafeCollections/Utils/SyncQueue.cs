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
            try
            {
                // Hold lock as queue is shared between multiple threads
                Monitor.Enter(_sync);

                _queue.Enqueue(item);

                //Notify other threads that the job is done
                Monitor.Pulse(_sync);
            }
            finally
            {
                // Release lock
                Monitor.Exit(_sync);
            }
        }

        public T Dequeue()
        {
            try
            {
                // Hold lock as queue is shared between multiple threads
                Monitor.Enter(_sync);

                while (_queue.Count == 0)
                {
                    // Will wait here till other thread notify with Monitor.Pulse or Monitor.PulseAll
                    Monitor.Wait(_sync);
                }

                return _queue.Dequeue();
            }
            finally
            {
                // Release lock
                Monitor.Exit(_sync);
            }
        }

        public int Count()
        {
            try
            {
                // Hold lock as queue is shared between multiple threads
                Monitor.Enter(_sync);

                return _queue.Count;
            }
            finally
            {
                // Release lock
                Monitor.Exit(_sync);
            }
        }
    }
}
