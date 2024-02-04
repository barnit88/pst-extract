using core.NDBLayer.ID;
using System;

namespace core.NDBLayer.Pages.BTree
{
    public class NodeBTreeEntry : IBTPageEntry
    {
        public BTreePageEntriesType BTreePageEntriesType { get; set; } = BTreePageEntriesType.NBTENTRY;
        public Nid Nid { get; set; }
        public Bid Bid { get; set; }
        public Bid BidSubNode { get; set; }
        public SpecialInternalNID SpecialInternalNID { get; set; }
        public NidType NidType { get; set; }
        #region Flags
        public ulong nid;
        public ulong bidData;
        public ulong bidSub;
        public uint nidParent;
        public uint dwPadding;
        #endregion
        public NodeBTreeEntry(byte[] nodeBTreeDataBytes)
        {
            nid = BitConverter.ToUInt64(nodeBTreeDataBytes, 0);
            SpecialInternalNID = (SpecialInternalNID)nid;
            NidType = (NidType)(nid & 0x1f);//Lowest five bits
            bidData = BitConverter.ToUInt64(nodeBTreeDataBytes, 8);
            bidSub = BitConverter.ToUInt64(nodeBTreeDataBytes, 16);
            nidParent = BitConverter.ToUInt32(nodeBTreeDataBytes, 24);
            dwPadding = 0;
            Nid = new Nid(nid);
            Bid = new Bid(bidData);
            BidSubNode = new Bid(bidSub);
        }
    }
}