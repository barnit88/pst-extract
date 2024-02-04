using core.NDBLayer.ID;
using System;

namespace core.NDBLayer.BREF
{
    public class Bref
    {
        public ExternalOrInternalBid ExternalOrInternalBid { get; set; }
        public Bid BId { get; set; } = null;
        public ulong bid { get; set; }
        public ulong ib { get; set; }
        public Bref(byte[] brefData)
        {
            if (brefData.Length != 16)
                throw new Exception("BREF Byte Length error");
            var bidData = BitConverter.ToUInt64(brefData);
            ib = BitConverter.ToUInt64(brefData, 8);
            BId = new Bid(bidData);
            bid = BId.BId;
            ExternalOrInternalBid = BId.ExternalOrInternalBid;
        }
    }
}