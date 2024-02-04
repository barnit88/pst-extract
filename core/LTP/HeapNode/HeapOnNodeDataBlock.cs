using core.NDBLayer.Blocks;
using System;

namespace core.LTP.HeapNode
{
    public class HeapOnNodeDataBlock
    {
        public DataBlock DataBlock { get; set; }
        public UInt16 PageOffset { get; set; }
        public HNHDR HNHeader { get; set; } = null;
        public HNPAGEHDR HNPageHeader { get; set; } = null;
        public HNBITMAPHDR HNBITMAPHeader { get; set; } = null;
        public HNPAGEMAP HNPageMap { get; set; }

        public HeapOnNodeDataBlock(int blockIndex, DataBlock dataBlock)
        {
            this.DataBlock = dataBlock;
            var dataBytes = dataBlock.data;
            this.PageOffset = BitConverter.ToUInt16(dataBytes, 0);
            this.HNPageMap = new HNPAGEMAP(dataBytes, this.PageOffset);
            if (blockIndex == 0)
                this.HNHeader = new HNHDR(dataBytes);
            else if (blockIndex % 128 == 8)
                this.HNBITMAPHeader = new HNBITMAPHDR(dataBytes);
            else
                this.HNPageHeader = new HNPAGEHDR(dataBytes);
        }
        public byte[] GetAllocation(HID hidUserRoot)
        {
            var offsetBegining = this.HNPageMap.rgibAlloc[(int)hidUserRoot.hidIndex - 1];
            var offsetEnd = this.HNPageMap.rgibAlloc[(int)hidUserRoot.hidIndex];
            byte[] allocatiionData = new byte[offsetEnd - offsetBegining];
            Array.Copy(this.DataBlock.data, offsetBegining, allocatiionData, 0, offsetEnd - offsetBegining);
            return allocatiionData;
        }
    }
}