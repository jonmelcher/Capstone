using System;
using System.Collections.Generic;


namespace Capstone
{
    public class ThreadSafeQueue<T>
    {
        private object _syncRoot;
        private Queue<T> _q;

        public ThreadSafeQueue()
        {
            _syncRoot = new object();
            _q = new Queue<T>();
        }

        public void Enqueue(T item)
        {
            lock (_syncRoot)
                _q.Enqueue(item);
        }

        public T Dequeue()
        {
            lock (_syncRoot)
                return _q.Dequeue();
        }

        public int Count
        {
            get
            {
                lock (_syncRoot)
                    return _q.Count;
            }
        }
    }
}