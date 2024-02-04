using System;

namespace core.NDBLayer.Blocks
{
    public class SLEntry
    {
        public ulong nid { get; set; }
        public ulong bidData { get; set; }
        public ulong bidSub { get; set; }
        public SLEntry(byte[] slEntryBytes)
        {
            if (slEntryBytes.Length != 24)
                throw new Exception("SlEntry, byte length error");
            nid = BitConverter.ToUInt64(slEntryBytes, 0);
            bidData = BitConverter.ToUInt64(slEntryBytes, 8);
            bidSub = BitConverter.ToUInt64(slEntryBytes, 16);
        }
    }
}