using System;
using System.Collections.Generic;
using System.Text;

namespace VGA.Tools.ProducerConsumer
{
    public class ThreadSafeQueue<TType>
    {
        private Queue<TType> mInternalQueue = new Queue<TType>();
        private readonly object synchronizer = new object();
        public TType Dequeue()
        {
            TType result;
            lock(synchronizer)
            {
                result = mInternalQueue.Dequeue();
            }
            return result;
        }

        public void Enqueue(TType item)
        {
            lock(synchronizer)
            {
                mInternalQueue.Enqueue(item);
            }
        }

        public bool TryDequeue(out TType item)
        {
            bool result = false;
            item = default(TType);
            lock(synchronizer)
            {
                if (mInternalQueue.Count > 0)
                {
                    item = mInternalQueue.Dequeue();
                    result = true;
                }
            }
            return result;
        }

        public int Count
        {
            get
            {
                int count = 0;
                lock(synchronizer)
                {
                    count = mInternalQueue.Count;
                }
                return count;
            }
        }
    }
}
