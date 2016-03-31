// ********************************************************************
//  filename    :   ThreadSafeQueue.cs
//  purpose     :   provide a thread-safe FIFO container which can
//                  enqueue/dequeue, clear, count, and send to an array
//
//  written by Jonathan Melcher and Brennan MacGregor
//  last updated 2016-03-29
// ********************************************************************


using System.Collections.Generic;


namespace GenericContainers
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

        public void Clear()
        {
            lock (_syncRoot)
                _q.Clear();
        }

        public T[] ToArray()
        {
            lock (_syncRoot)
                return _q.ToArray();
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
