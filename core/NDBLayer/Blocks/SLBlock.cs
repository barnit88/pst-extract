using System;
using System.Collections.Generic;

namespace core.NDBLayer.Blocks
{
    public class SLBlock
    {
        public List<SLEntry> SLEntries { get; set; } = new List<SLEntry>();
        public byte btype { get; set; }
        public byte cLevel { get; set; }
        public ushort cEnt { get; set; }
        public uint dwPadding { get; set; }
        public byte[] rgentries { get; set; }
        public byte[] rgbPadding { get; set; }
        public BlockTrailer blockTrailer { get; set; }
        public SLBlock(byte[] slBlockBytes, byte[] slBlockTrailerDataBytes)
        {
            btype = slBlockBytes[0];
            cLevel = slBlockBytes[1];
            if (!(btype == 0x02 && cLevel == 0x00))
                throw new Exception("SLBlock, btype and clevel match error");
            cEnt = BitConverter.ToUInt16(slBlockBytes, 2);
            dwPadding = 0;
            var rgEntriesSize = cEnt * 24;
            rgentries = new byte[rgEntriesSize];
            Array.Copy(slBlockBytes, 8, rgentries, 0, rgEntriesSize);
            for (int i = 1; i <= cEnt; i++)
            {
                byte[] temp = new byte[24];
                int position = (i - 1) * 24;
                Array.Copy(rgentries, position, temp, 0, 24);
                SLEntries.Add(new SLEntry(temp));
            }
            blockTrailer = new BlockTrailer(slBlockTrailerDataBytes);
        }
    }
}
