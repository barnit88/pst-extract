using core.LTP.HeapNode;
using System;

namespace core.LTP.BTreeOnHeap
{
    public class BTHIndexEntry
    {
        public byte[] Key { get; set; }
        public HID HidNextLevel { get; set; }
        private byte[] key { get; set; }
        private UInt32 hidNextLevel { get; set; }
        public BTHIndexEntry(byte[] data, BTHHeader header, int offset)
        {
            int keySize = (int)header.CbKey;
            Array.Copy(data, offset, this.Key, 0, header.CbKey);
            this.key = this.Key;
            int hidOffset = offset + keySize;
            byte[] tempHidData = new byte[4];
            this.hidNextLevel = BitConverter.ToUInt32(data, hidOffset);
            Array.Copy(data, hidOffset, tempHidData, 0, 4);
            this.HidNextLevel = new HID(tempHidData);
        }
    }
}