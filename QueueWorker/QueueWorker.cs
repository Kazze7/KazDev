using System.Collections.Concurrent;

namespace KazDev.QueueWorker
{
    public class QueueWorker<T>
    {
        bool isRunning = false;

        Thread workerThread;
        ManualResetEvent nextItemEvent = new ManualResetEvent(false);
        ConcurrentQueue<T> itemQueue = new ConcurrentQueue<T>();

        public delegate void DequeueMethod(T _item);
        DequeueMethod dequeueMethod;

        public QueueWorker(DequeueMethod _dequeueMethod)
        {
            dequeueMethod = _dequeueMethod;
        }

        public void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                workerThread = new Thread(StartQueue);
                workerThread.Start();
            }
        }
        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                nextItemEvent.Set();
                workerThread?.Join();
                itemQueue = new ConcurrentQueue<T>();
            }
        }

        void StartQueue()
        {
            T item;
            while (isRunning)
            {
                if (itemQueue.TryDequeue(out item))
                {
                    dequeueMethod(item);
                }
                else
                {
                    nextItemEvent.WaitOne();
                    nextItemEvent.Reset();
                }
            }
        }

        public void Enqueue(T _item)
        {
            itemQueue.Enqueue(_item);
            nextItemEvent.Set();
        }
        public void Enqueue(ref T _item)
        {
            itemQueue.Enqueue(_item);
            nextItemEvent.Set();
        }
    }
}
