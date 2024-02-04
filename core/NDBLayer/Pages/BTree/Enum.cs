using System.ComponentModel;

namespace core.NDBLayer.Pages.BTree
{
    public enum BTreeType
    {
        [Description("Node BTree")]
        NBT,
        [Description("Block BTree")]
        BBT
    }
    public enum BTreeEntryType
    {
        [Description("Node BTree")]
        NBTreeEntry,
        [Description("Block BTree")]
        BBTreeEntry
    }
    public enum BTreePageEntriesType : byte
    {
        [Description("Node BTree Entry")]
        NBTENTRY,
        [Description("Block BTree Entry")]
        BBTENTRY,
        [Description("BTree Entry")]
        BTENTRY
    }
    public enum UnicodergentriesType : byte
    {
        [Description("Node BTree Entry")]
        NBTENTRY = 32,
        [Description("Block BTree Entry")]
        BBTENTRY = 24,
        [Description("BTree Entry")]
        BTENTRY = 24
    }
    public enum AnsirgentriesType : byte
    {
        [Description("Node BTree Entry")]
        NBTENTRY = 16,
        [Description("Block BTree Entry")]
        BBTENTRY = 12,
        [Description("BTree Entry")]
        BTENTRY = 12
    }
}
