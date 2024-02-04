using System;

namespace core.NDBLayer.Blocks
{
    public class SIEntry
    {
        public ulong nid { get; set; }
        public ulong bid { get; set; }
        public SIEntry(byte[] siEntryBytes)
        {
            if (siEntryBytes.Length != 16)
                throw new Exception("SIEntry, byte length error");
            nid = BitConverter.ToUInt64(siEntryBytes, 0);
            bid = BitConverter.ToUInt64(siEntryBytes, 8);
        }
    }
}
