using System;

namespace core.LTP.HeapNode
{
    public class HNPAGEHDR
    {
        public ushort ibHnpm { get; set; }
        public HNPAGEHDR(byte[] dataBytes)
        {
            this.ibHnpm = BitConverter.ToUInt16(dataBytes, 0);
        }
    }
}
