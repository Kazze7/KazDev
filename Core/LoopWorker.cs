namespace KazDev.Core
{
    public abstract class LoopWorker
    {
        public abstract void OnStart();
        public abstract void OnStop();
        public abstract void Loop();

        bool isRunning = false;

        public void Start()
        {
            if (isRunning) return;
            isRunning = true;
            OnStart();
            Thread thread = new Thread(ThreadLoop) { Name = "LoopWorker" };
            thread.Start();
        }
        public void Stop()
        {
            if (!isRunning) return;
            isRunning = false;
            OnStop();
        }

        void ThreadLoop()
        {
            while (isRunning)
                Loop();
        }
    }
}
