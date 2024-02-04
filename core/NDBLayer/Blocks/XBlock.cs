using core.NDBLayer.ID;
using System;
using System.Collections.Generic;

namespace core.NDBLayer.Blocks
{
    public class XBlock
    {
        public List<Bid> DataBlockBids { get; set; } = new List<Bid>();
        public byte btype { get; set; }
        public byte cLevel { get; set; }
        public short cEnt { get; set; }
        public uint IcbTotal { get; set; }
        public byte[] rgbid { get; set; }
        public byte[] rgbPadding { get; set; }
        public BlockTrailer BlockTrailer { get; set; }
        public XBlock(byte[] xblockBytes, byte[] xblockTrailerBytes)
        {
            BlockTrailer = new BlockTrailer(xblockTrailerBytes);
            if (BlockTrailer.cb != xblockBytes.Length)
                throw new Exception("XBlock Data Size mismatch");
            btype = xblockBytes[0];
            cLevel = xblockBytes[1];
            cEnt = BitConverter.ToInt16(xblockBytes, 2);
            IcbTotal = BitConverter.ToUInt32(xblockBytes, 4);
            var rgbidLength = cEnt * 8;
            rgbid = new byte[rgbidLength];
            Array.Copy(xblockBytes, 8, rgbid, 0, rgbidLength);
            for (int i = 1; i <= cEnt; i++)
            {
                int position = (i - 1) * 8;
                var tempBid = BitConverter.ToUInt64(rgbid, position);
                DataBlockBids.Add(new Bid(tempBid));
            }
            if (rgbid.Length != rgbidLength)
                throw new Exception("XBlock , Something wrong in rgbid array");
            BlockTrailer = new BlockTrailer(xblockTrailerBytes);
        }
    }
}
