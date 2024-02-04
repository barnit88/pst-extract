using core.NDBLayer.ID;
using System;
using System.Collections.Generic;

namespace core.NDBLayer.Blocks
{
    public class XXBlock
    {
        public List<Bid> XBlockBids { get; set; } = new List<Bid>();
        public byte btype { get; set; }
        public byte cLevel { get; set; }
        public ushort cEnt { get; set; }
        public uint IcbTotal { get; set; }
        public byte[] rgbid { get; set; }
        public byte[] rgbPadding { get; set; }
        public BlockTrailer BlockTrailer { get; set; }
        public XXBlock(byte[] xxBlockBytes, byte[] xxblockTrailerDataBytes)
        {
            btype = xxBlockBytes[0];
            cLevel = xxBlockBytes[1];
            if (!(btype == 0x01 && cLevel == 0x01))
                throw new Exception("XXBlock, btype and clevel match error");
            cEnt = BitConverter.ToUInt16(xxBlockBytes, 2);
            IcbTotal = BitConverter.ToUInt32(xxBlockBytes, 4);
            var rgbidLength = cEnt * 8;
            rgbid = new byte[rgbidLength];
            Array.Copy(xxBlockBytes, 8, rgbid, 0, rgbidLength);
            for (int i = 1; i <= cEnt; i++)
            {
                int position = (i - 1) * 8;
                var tempBid = BitConverter.ToUInt64(rgbid, position);
                XBlockBids.Add(new Bid(tempBid));
            }
            if (rgbid.Length != rgbidLength)
                throw new Exception("XBlock , Something wrong in rgbid array");
            BlockTrailer = new BlockTrailer(xxblockTrailerDataBytes);
        }
    }
}
