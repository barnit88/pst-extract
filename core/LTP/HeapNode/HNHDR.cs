using System;
using System.Linq;

namespace core.LTP.HeapNode
{
    public class HNHDR
    {
        public HID RootHId { get; set; }
        public HNClientSig ClientSig { get; set; }
        public ushort ibHnpm { get; set; }
        public byte bSig { get; set; }
        public byte bClientSig { get; set; }
        public UInt32 hidUserRoot { get; set; }
        public ulong rgbFillLevel { get; set; }//storing raw value for now
        public HNHDR(byte[] dataBytes)
        {
            this.hidUserRoot = BitConverter.ToUInt16(dataBytes, 0);
            this.bSig = dataBytes[2];
            this.bClientSig = dataBytes[3];
            this.hidUserRoot = BitConverter.ToUInt32(dataBytes, 4);
            this.ClientSig = (HNClientSig)this.bClientSig;
            this.RootHId = new HID(dataBytes.Skip(4).Take(4).ToArray());
        }
    }
}
