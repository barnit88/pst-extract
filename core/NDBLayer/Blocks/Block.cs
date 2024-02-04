using core.NDBLayer.BREF;
using core.NDBLayer.Headers;
using core.NDBLayer.ID;
using System;
using System.IO.MemoryMappedFiles;

namespace core.NDBLayer.Blocks
{

    public class Block
    {
        public XXBlock XXBlock { get; set; } = null;
        public XBlock XBlock { get; set; } = null;
        public DataBlock DataBlock { get; set; } = null;
        public SLBlock SLBlock { get; set; } = null;
        public SIBlock SIBlock { get; set; } = null;
        public BlockType BlockType { get; set; }
        public byte[] data { get; set; }
        public byte[] padding { get; set; }
        public BlockTrailer BlockTrailer { get; set; }
        public byte btype { get; set; }
        public byte cLevel { get; set; }

        public Block(MemoryMappedFile file, Bref bref, ushort blockDataSize, bCryptMethodType encodignType)
        {
            decimal blockDataAndTrailerSize = blockDataSize + 16;
            decimal tempValue = blockDataAndTrailerSize / 64;
            var multipleOfSixtyFourRequiredForActualBlockSize =
                Math.Ceiling(tempValue);
            long actualDataBlockSize = (long)multipleOfSixtyFourRequiredForActualBlockSize * 64;
            int paddingSize = (int)actualDataBlockSize - 16 - blockDataSize;
            padding = new byte[paddingSize];
            using (MemoryMappedViewAccessor view = file.CreateViewAccessor((long)bref.ib, actualDataBlockSize))
            {
                byte[] blockTrailerDataBytes = new byte[16];
                long blockTrailerDataStartPosition = actualDataBlockSize - 16;
                view.ReadArray(blockTrailerDataStartPosition, blockTrailerDataBytes, 0, 16);
                BlockTrailer = new BlockTrailer(blockTrailerDataBytes);
                byte[] blockDataBytes = new byte[blockDataSize];
                view.ReadArray(0, blockDataBytes, 0, (int)blockDataSize);
                if (bref.ExternalOrInternalBid == ExternalOrInternalBid.External)
                {
                    //External. This is a Data Block
                    BlockType = BlockType.DATABLOCK;
                    DataBlock = new DataBlock(blockDataBytes, blockTrailerDataBytes, encodignType);
                }
                else if (bref.ExternalOrInternalBid == ExternalOrInternalBid.Internal)
                {
                    //Intermediate Block need further calculation for type of block
                    //Determining which type of internal block :- XBLOCK,XXBLOCK,SLBLOCK,SIBLOCK
                    btype = view.ReadByte(0);
                    cLevel = view.ReadByte(1);
                    // XBLOCK or XXBLOCK
                    if (btype == 0x01)
                    {
                        // XBLOCK
                        if (cLevel == 0x01)
                        {

                            BlockType = BlockType.XBLOCK;
                            XBlock = new XBlock(blockDataBytes, blockTrailerDataBytes);
                        }
                        // XXBLOCK
                        else if (cLevel == 0x02)
                        {
                            BlockType = BlockType.XXBLOCK;
                            XXBlock = new XXBlock(blockDataBytes, blockTrailerDataBytes);
                        }
                    }
                    // SLBLOCK OR SIBLOCK
                    else if (btype == 0x02)
                    {
                        //SLBLOCK
                        if (cLevel == 0x00)
                        {
                            BlockType = BlockType.SLBLOCK;
                            SLBlock = new SLBlock(blockDataBytes, blockTrailerDataBytes);
                        }
                        //SIBLOCK
                        if (cLevel == 0x01)
                        {
                            BlockType = BlockType.SIBLOCK;
                            SIBlock = new SIBlock(blockDataBytes, blockTrailerDataBytes);
                        }
                    }
                }
            }
        }
    }
}