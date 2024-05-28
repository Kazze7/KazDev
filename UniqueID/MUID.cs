using System;

namespace KazDev.UniqueID
{
    public struct MUID
    {
        ushort a;
        ushort b;
        ushort c;
        ushort d;

        public MUID(ushort _a, ushort _b, ushort _c, ushort _d)
        {
            a = _a;
            b = _b;
            c = _c;
            d = _d;
        }
        public MUID(ulong _mUID)
        {
            byte[] bytes = BitConverter.GetBytes(_mUID);
            a = BitConverter.ToUInt16(bytes.ToArray(), 6);
            b = BitConverter.ToUInt16(bytes.ToArray(), 4);
            c = BitConverter.ToUInt16(bytes.ToArray(), 2);
            d = BitConverter.ToUInt16(bytes.ToArray(), 0);
        }
        public MUID(string _mUID)
        {
            ulong ulongMUID = ulong.Parse(_mUID.Replace("-", ""), System.Globalization.NumberStyles.HexNumber);
            byte[] bytes = BitConverter.GetBytes(ulongMUID);
            a = BitConverter.ToUInt16(bytes.ToArray(), 6);
            b = BitConverter.ToUInt16(bytes.ToArray(), 4);
            c = BitConverter.ToUInt16(bytes.ToArray(), 2);
            d = BitConverter.ToUInt16(bytes.ToArray(), 0);
        }

        public override string ToString()
        {
            return a.ToString("X4") + "-" + b.ToString("X4") + "-" + c.ToString("X4") + "-" + d.ToString("X4");
        }
        public string[] ToHexArray()
        {
            return new string[] { a.ToString("X4"), b.ToString("X4"), c.ToString("X4"), d.ToString("X4") };
        }
        public ushort[] ToUshortArray()
        {
            return new ushort[] { a, b, c, d };
        }
        public uint[] ToUintArray()
        {
            uint[] uints = new uint[2];
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(b));
            bytes.AddRange(BitConverter.GetBytes(a));
            uints[0] = BitConverter.ToUInt32(bytes.ToArray());
            bytes.Clear();
            bytes.AddRange(BitConverter.GetBytes(d));
            bytes.AddRange(BitConverter.GetBytes(c));
            uints[1] = BitConverter.ToUInt32(bytes.ToArray());
            return uints;
        }
        public byte[] ToByteArray()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(d));
            bytes.AddRange(BitConverter.GetBytes(c));
            bytes.AddRange(BitConverter.GetBytes(b));
            bytes.AddRange(BitConverter.GetBytes(a));
            return bytes.ToArray();
        }
        public ulong ToUlong()
        {
            return BitConverter.ToUInt64(ToByteArray());
        }
        public DateTime GetDateTime()
        {
            return new DateTime(a / 12, a % 12, b / 1440, b / 60 % 24, b % 60, c / 1000, c % 1000);
        }
        public DateTime GetLocalDateTime()
        {
            return GetDateTime().ToLocalTime();
        }
    }
}
