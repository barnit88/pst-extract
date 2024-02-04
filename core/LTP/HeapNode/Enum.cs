using System.ComponentModel;

namespace core.LTP.HeapNode
{
    public enum HNClientSig
    {
        [Description("Reserved")]
        bTypeReserved1 = 0x6C,
        [Description("Table Context(TC/HN)")]
        bTypeTC = 0x7C,
        [Description("Reserved")]
        bTypeReserved2 = 0x8C,
        [Description("Reserved")]
        bTypeReserved3 = 0x9C,
        [Description("Reserved")]
        bTypeReserved4 = 0xA5,
        [Description("Reserved")]
        bTypeReserved5 = 0xAC,
        [Description("BTree-on-Heap(BTH)")]
        bTypeBTH = 0xB5,
        [Description("Property Context(PC/BTH)")]
        bTypePC = 0xBC,
        [Description("Reserved")]
        bTypeReserved6 = 0xCC
    }
}