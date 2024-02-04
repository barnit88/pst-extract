using System;
using System.Linq;

namespace core.LTP.HeapNode
{
    public class HNBITMAPHDR
    {
        public UInt16 ibHnpm { get; set; }
        public byte[] rgbFillLevel { get; set; }
        public HNBITMAPHDR(byte[] dataBytes)
        {
            this.ibHnpm = BitConverter.ToUInt16(dataBytes, 0);
            this.rgbFillLevel = dataBytes.Skip(2).Take(64).ToArray();
        }
    }
}