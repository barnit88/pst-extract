using System;

namespace core.LTP.BTreeOnHeap
{
    public class BTHDataEntry
    {
        public byte[] Key { get; set; }
        public byte[] Data { get; set; }
        public BTHDataEntry(byte[] data, int offset, BTH tree)
        {
            int keyOffset = offset;
            int keySize = (int)tree.BTHHeader.CbKey;
            this.Key = new byte[keySize];
            Array.Copy(data, keyOffset, this.Key, 0, keySize);
            int dataOffset = keyOffset + keySize;
            int dataSize = (int)(tree.BTHHeader.CbEnt);
            this.Data = new byte[dataSize];
            Array.Copy(data, dataOffset, this.Data, 0, dataSize);
        }
    }
}
