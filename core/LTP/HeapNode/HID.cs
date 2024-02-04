using System;

namespace core.LTP.HeapNode
{
    public class HID
    {
        public UInt32 hid { get; set; }
        public ulong hidType { get; set; }
        public ulong hidIndex { get; set; }
        public ushort hidBlockIndex { get; set; }
        public HID(byte[] dataBytes)
        {
            int offset = 0;
            var temp = BitConverter.ToUInt32(dataBytes, offset);
            this.hid = temp;
            this.hidType = temp & 0x1F;
            this.hidIndex = (temp >> 5) & 0x7FF;
            this.hidBlockIndex = BitConverter.ToUInt16(dataBytes, offset + 2);
        }
    }
}
