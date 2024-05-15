namespace KazDev.UniqueID
{
    public class MUIDGenerator
    {
        DateTime dateTime;
        DateTime lastDate;
        byte machineID = 0;
        ushort machineIDushort = 0;
        public byte MachineID { get { return machineID; } }
        AutoResetEvent nextMUIDEvent = new AutoResetEvent(true);
        ushort nextA, nextB, nextC;
        ushort lastA, lastB, lastC, lastD;

        public MUIDGenerator() { }
        public MUIDGenerator(byte _machineID) { machineID = _machineID; machineIDushort = (ushort)(_machineID * 256); }

        public MUID NewID()
        {
            nextMUIDEvent.WaitOne();
            MUID mUID;
            dateTime = DateTime.UtcNow;
            if (dateTime.Ticks > (lastDate.Ticks + 10000))
                lastDate = dateTime;
            NextMilliseconds:
            nextA = (ushort)(lastDate.Year * 12 + lastDate.Month);
            nextB = (ushort)(lastDate.Day * 1440 + lastDate.Hour * 60 + lastDate.Minute);
            nextC = (ushort)(lastDate.Second * 1000 + lastDate.Millisecond);

            if (nextA > lastA)
            {
                lastA = nextA;
                lastB = nextB;
                lastC = nextC;
                lastD = 0;
            }
            else if (nextB > lastB)
            {
                lastB = nextB;
                lastC = nextC;
                lastD = 0;
            }
            else if (nextC > lastC)
            {
                lastC = nextC;
                lastD = 0;
            }
            lastD++;

            if (machineIDushort > 0)
                lastD %= 256;

            if (lastD == 0)
            {
                lastDate = lastDate.AddMilliseconds(1);
                goto NextMilliseconds;
            }
            lastD += machineIDushort;
            mUID = new MUID(lastA, lastB, lastC, lastD);
            nextMUIDEvent.Set();
            return mUID;
        }
    }
}
