using core.LTP.HeapNode;
using System;

namespace core.LTP.BTreeOnHeap
{
    public class BTHHeader
    {
        public uint CbKey { get; set; }
        public uint CbEnt { get; set; }
        public uint BIdxLevels { get; set; }
        public HID HidRoot { get; set; }
        private byte bType { get; set; }
        private byte cbKey { get; set; }
        private byte cbEnt { get; set; }
        private byte bIdxLevels { get; set; }
        private byte[] hidRoot { get; set; }
        public BTHHeader(byte[] dataBytes)
        {
            this.bType = dataBytes[0];
            this.cbKey = dataBytes[1];
            this.cbEnt = dataBytes[2];
            this.bIdxLevels = dataBytes[3];
            this.hidRoot = new byte[4];
            Array.Copy(dataBytes, 4, this.hidRoot, 0, 4);
            this.HidRoot = new HID(this.hidRoot);
            this.CbKey = (uint)this.cbKey;
            this.CbEnt = (uint)this.cbEnt;
            this.BIdxLevels = (uint)this.bIdxLevels;
        }
    }
}