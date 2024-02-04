using core.NDBLayer.ID;
using System;

namespace core.Messaging
{
    public class EntryId
    {
        public Nid Nid { get; set; }
        public UInt32 rgbFlags { get; set; }
        public byte[] uid { get; set; }
        public UInt32 nid { get; set; }
        public EntryId(byte[] dataBytes, int offset = 0)
        {
            this.rgbFlags = BitConverter.ToUInt32(dataBytes, offset);
            this.uid = new byte[16];
            Array.Copy(dataBytes, offset + 4, this.uid, 0, 16);
            this.nid = BitConverter.ToUInt32(dataBytes, 20);
            this.Nid = new Nid(this.nid);
        }
    }
}
