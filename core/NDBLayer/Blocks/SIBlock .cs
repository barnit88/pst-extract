using System;
using System.Collections.Generic;

namespace core.NDBLayer.Blocks
{
    public class SIBlock
    {
        public List<SIEntry> SIEntries { get; set; } = new List<SIEntry>();
        public byte btype { get; set; }
        public byte cLevel { get; set; }
        public ushort cEnt { get; set; }
        public uint dwPadding { get; set; }
        public byte[] rgentries { get; set; }
        public byte[] rgbPadding { get; set; }
        public BlockTrailer blockTrailer { get; set; }
        public SIBlock(byte[] siBlockBytes, byte[] siBlockTrailerDataBytes)
        {
            btype = siBlockBytes[0];
            cLevel = siBlockBytes[1];
            if (!(btype == 0x02 && cLevel == 0x01))
                throw new Exception("SIBlock, btype and clevel match error");
            cEnt = BitConverter.ToUInt16(siBlockBytes, 2);
            dwPadding = 0;
            var rgEntriesSize = cEnt * 16;
            rgentries = new byte[rgEntriesSize];
            Array.Copy(siBlockBytes, 8, rgentries, 0, rgEntriesSize);
            for (int i = 1; i <= cEnt; i++)
            {
                byte[] temp = new byte[16];
                int position = (i - 1) * 16;
                Array.Copy(rgentries, position, temp, 0, 16);
                SIEntries.Add(new SIEntry(temp));
            }
            blockTrailer = new BlockTrailer(siBlockTrailerDataBytes);
        }

    }
}
