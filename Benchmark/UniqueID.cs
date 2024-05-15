using KazDev.UniqueID;

namespace Benchmark
{
    static class UniqueID
    {
        static int seconds = 1;

        public static void Example() {
            Console.WriteLine("-MUID             -");
            MUIDGenerator mUIDGenerator = new MUIDGenerator(1);
            MUID mUIDStruct = mUIDGenerator.NewID();
            Console.WriteLine("-ToString         -");
            Console.WriteLine(mUIDStruct.ToString());
            Console.WriteLine("-ToHexArray       -");
            mUIDStruct.ToHexArray().ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-ToUshortArray    -");
            mUIDStruct.ToUshortArray().ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-ToUintArray      -");
            mUIDStruct.ToUintArray().ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-ToByteArray      -");
            mUIDStruct.ToByteArray().ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-ToUlong          -");
            Console.WriteLine(mUIDStruct.ToUlong());
            Console.WriteLine("-GetDateTime      -");
            Console.WriteLine(mUIDStruct.GetDateTime());
            Console.WriteLine("-GetLocalDateTime -");
            Console.WriteLine(mUIDStruct.GetLocalDateTime());
            Console.WriteLine("");
        }
        public static void Start()
        {
            MUIDGenerator mUIDGenerator;
            AutoResetEvent mEvent = new AutoResetEvent(true);

            Console.WriteLine("Benchmark");
            Console.WriteLine();

            Console.WriteLine("Single Thread / Without Machine ID");
            mUIDGenerator = new MUIDGenerator(0);
            BenchmarkLoop(mUIDGenerator, out int count0);

            Console.WriteLine("Single Thread / With Machine ID");
            mUIDGenerator = new MUIDGenerator(1);
            BenchmarkLoop(mUIDGenerator, out int count1);

            Console.WriteLine("Multi Thread (" + Environment.ProcessorCount + ") / Without Machine ID");
            mUIDGenerator = new MUIDGenerator(0);
            int countAll = 0;
            Thread[] threads = new Thread[Environment.ProcessorCount];
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                threads[i] = new Thread(() =>
                {
                    BenchmarkLoop(mUIDGenerator, out int count, false);
                    mEvent.WaitOne();
                    countAll += count;
                    mEvent.Set();
                });
            };
            threads.ToList().ForEach(thread => thread.Start());
            threads.ToList().ForEach(thread => thread.Join());
            Console.WriteLine("Stop multi thread " + DateTime.UtcNow);
            Console.WriteLine("ops/sec: " + countAll / seconds);
            Console.WriteLine();

            Console.WriteLine("Multi Thread (" + Environment.ProcessorCount + ") / With Machine ID");
            mUIDGenerator = new MUIDGenerator(1);
            countAll = 0;
            threads = new Thread[Environment.ProcessorCount];
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                threads[i] = new Thread(() =>
                {
                    BenchmarkLoop(mUIDGenerator, out int count, false);
                    mEvent.WaitOne();
                    countAll += count;
                    mEvent.Set();
                });
            };
            threads.ToList().ForEach(thread => thread.Start());
            threads.ToList().ForEach(thread => thread.Join());
            Console.WriteLine("Stop multi thread " + DateTime.UtcNow);
            Console.WriteLine("ops/sec: " + countAll / seconds);
            Console.WriteLine();

            Console.WriteLine("Multi Thread (" + Environment.ProcessorCount + ") / Without Machine ID / Multi generator");
            countAll = 0;
            threads = new Thread[Environment.ProcessorCount];
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                MUIDGenerator mMUIDGenerator = new MUIDGenerator(0);
                threads[i] = new Thread(() =>
                {
                    BenchmarkLoop(mMUIDGenerator, out int count, false);
                    mEvent.WaitOne();
                    countAll += count;
                    mEvent.Set();
                });
            };
            threads.ToList().ForEach(thread => thread.Start());
            threads.ToList().ForEach(thread => thread.Join());
            Console.WriteLine("Stop multi thread " + DateTime.UtcNow);
            Console.WriteLine("ops/sec: " + countAll / seconds);
            Console.WriteLine();

            Console.WriteLine("Multi Thread (" + Environment.ProcessorCount + ") / With Machine ID / Multi generator");
            countAll = 0;
            threads = new Thread[Environment.ProcessorCount];
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                MUIDGenerator mMUIDGenerator = new MUIDGenerator(1);
                threads[i] = new Thread(() =>
                {
                    BenchmarkLoop(mMUIDGenerator, out int count, false);
                    mEvent.WaitOne();
                    countAll += count;
                    mEvent.Set();
                });
            };
            threads.ToList().ForEach(thread => thread.Start());
            threads.ToList().ForEach(thread => thread.Join());
            Console.WriteLine("Stop multi thread " + DateTime.UtcNow);
            Console.WriteLine("ops/sec: " + countAll / seconds);
            Console.WriteLine();
        }
        static void BenchmarkLoop(MUIDGenerator _mUIDGenerator, out int _count, bool _show = true)
        {
            MUID mUIDStruct = new MUID();
            DateTime utcNow;
            int count = 0;
            Console.WriteLine("Start " + DateTime.UtcNow);
            utcNow = DateTime.UtcNow;
            utcNow = utcNow.AddSeconds(seconds);
            while (DateTime.UtcNow < utcNow)
            {
                mUIDStruct = _mUIDGenerator.NewID();
                count++;
            }
            _count = count;

            Console.WriteLine("Stop " + DateTime.UtcNow);
            if (_show)
            {
                Console.WriteLine("ops/sec: " + _count / seconds);
                Console.WriteLine("Last MUID " + mUIDStruct.ToString());
                Console.WriteLine("Last MUID " + mUIDStruct.GetDateTime());
            }
            Console.WriteLine("Time diffrence " + (mUIDStruct.GetDateTime() - DateTime.UtcNow) / seconds);
            if (_show)
                Console.WriteLine();
        }
    }
}
