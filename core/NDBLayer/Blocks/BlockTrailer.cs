using core.NDBLayer.ID;
using System;

namespace core.NDBLayer.Blocks
{
    public class BlockTrailer
    {
        public Bid Bid { get; set; }
        public short cb { get; set; }
        public short wSig { get; set; }
        public int dwCRC { get; set; }
        public long bid { get; set; }
        public BlockTrailer(byte[] blockTrailerDataBytes)
        {
            if (blockTrailerDataBytes.Length != 16)
                throw new Exception("Block Trailer Data Bytes Lenght Error");
            cb = BitConverter.ToInt16(blockTrailerDataBytes, 0);
            wSig = BitConverter.ToInt16(blockTrailerDataBytes, 2);
            dwCRC = BitConverter.ToInt32(blockTrailerDataBytes, 4);
            bid = BitConverter.ToInt64(blockTrailerDataBytes, 8);
            Bid = new Bid((ulong)bid);
        }
    }
}
