using System;

namespace core.Messaging
{
    public class NameId
    {
        public Guid Guid { get; set; }
        public UInt32 dwPropertyID { get; set; }
        public bool N { get; set; }
        public byte wGuid { get; set; }
        public UInt16 wPropIdx { get; set; }
        public NameId(byte[] dataBytes, int offset, NamedPropertyLookup lookup)
        {

            this.dwPropertyID = BitConverter.ToUInt32(dataBytes, offset);
            this.N = (dataBytes[offset + 4] & 0x1) == 1;
            var guidType = BitConverter.ToUInt16(dataBytes, offset + 4) >> 1;
            if (guidType == 1)
                this.Guid = new Guid("00020328-0000-0000-C000-000000000046");//PS-MAPI
            else if (guidType == 2)
                this.Guid = new Guid("00020329-0000-0000-C000-000000000046");//PS_PUBLIC_STRINGS
            else
                this.Guid = new Guid(lookup._GUIDs.RangeSubset((guidType - 3) * 16, 16));
            this.wPropIdx = (UInt16)(0x8000 + BitConverter.ToUInt16(dataBytes, offset + 6));
        }
    }
}
