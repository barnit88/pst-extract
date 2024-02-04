using System;

namespace core.LTP.TableContext
{
    public class TCOLDESC
    {
        public Int32 tag { get; set; }
        public Int16 ibData { get; set; }
        public byte cbData { get; set; }
        public byte iBit { get; set; }
        public TCOLDESC(byte[] dataBytes, int offset = 0)
        {
            this.tag = BitConverter.ToInt32(dataBytes, offset);
            this.ibData = BitConverter.ToInt16(dataBytes, offset + 4);
            this.cbData = dataBytes[offset + 6];
            this.iBit = dataBytes[offset + 1];
        }
    }
}
