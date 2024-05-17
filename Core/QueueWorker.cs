using System.Collections.Concurrent;

namespace KazDev.Core
{
    public class QueueWorker<T>
    {
        bool isRunning = false;

        Thread workerThread;
        ConcurrentQueue<T> itemQueue;
        AutoResetEvent workerEvent = new(true);
        ManualResetEvent nextItemEvent = new(false);

        public delegate void DequeueMethod(T _item);
        DequeueMethod dequeueMethod;

        public QueueWorker(DequeueMethod _dequeueMethod)
        {
            dequeueMethod = _dequeueMethod;
            workerThread = new Thread(StartQueue) { Name = "QueueWorker" };
            itemQueue = new ConcurrentQueue<T>();
        }

        public void Start()
        {
            workerEvent.WaitOne();
            if (!isRunning)
            {
                isRunning = true;
                workerThread.Start();
            }
            workerEvent.Set();
        }
        public void Stop()
        {
            workerEvent.WaitOne();
            if (isRunning)
            {
                isRunning = false;
                nextItemEvent.Set();
                workerThread = new Thread(StartQueue) { Name = "QueueWorker" };
                itemQueue = new ConcurrentQueue<T>();
            }
            workerEvent.Set();
        }

        void StartQueue()
        {
            T item;
            while (isRunning)
            {
                if (itemQueue.TryDequeue(out item))
                    dequeueMethod(item);
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
