using System.ComponentModel;

namespace core.NDBLayer.Pages.Base
{
    public enum PageType : byte
    {
        [Description("Block BTree Page")]
        ptypeBBT = 0x80,
        [Description("Node BTree Page")]
        ptypeNBT = 0x81,
        [Description("Free Map Page")]
        ptypeFMap = 0x82,
        [Description("Allocation Page Map Page")]
        ptypePMap = 0x83,
        [Description("Allocation Map Page")]
        ptypeAMap = 0x84,
        [Description("Free Page Map Page")]
        ptypeFPMap = 0x85,
        [Description("Density List Page")]
        ptypeDL = 0x86
    }
    public enum wSig : short
    {
        ptypeFMap = 0x0000,
        ptypePMap = 0x0000,
        ptypeAMap = 0x0000,
        ptypeFPMap = 0x0000,
    }
}
