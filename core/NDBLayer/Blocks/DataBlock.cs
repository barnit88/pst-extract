using core.NDBLayer.Headers;
using System;

namespace core.NDBLayer.Blocks
{
    public class DataBlock
    {
        public byte[] data { get; set; }
        public byte[] padding { get; set; }
        public BlockTrailer BlockTrailer { get; set; }
        public DataBlock(byte[] dataBlockBytes, byte[] blockTrailerDataBytes, bCryptMethodType encodignType)
        {
            data = dataBlockBytes;
            BlockTrailer = new BlockTrailer(blockTrailerDataBytes);
            if (data.Length != BlockTrailer.cb)
                throw new Exception("Data Block. Data Size Mismatch ");
            DatatEncoder.CryptPermute(data, data.Length, false, encodignType);
        }
        public DataBlock(byte[] dataBlockBytes)
        {
            data = dataBlockBytes;
            if (data.Length != BlockTrailer.cb)
                throw new Exception("Data Block. Data Size Mismatch ");
        }
    }
}