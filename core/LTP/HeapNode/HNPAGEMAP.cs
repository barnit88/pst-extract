using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace core.LTP.HeapNode
{
    public class HNPAGEMAP
    {
        public UInt16 cAlloc { get; set; }
        public UInt16 cFree { get; set; }
        public List<UInt16> rgibAlloc { get; set; } = new List<UInt16>();
        public HNPAGEMAP(byte[] dataBytes, int offset)
        {
            this.cAlloc = BitConverter.ToUInt16(dataBytes, offset);
            this.cFree = BitConverter.ToUInt16(dataBytes, offset + 2);
            //Allocation table. This contains cAlloc + 1 entries. 
            for (int i =0; i < this.cAlloc + 1; i++) {
                var rgibAllocStartOffset = 4 + (i * 2);
                this.rgibAlloc.Add(BitConverter.ToUInt16(dataBytes, offset + rgibAllocStartOffset));
            }
        }
    }
}
