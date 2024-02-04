namespace core.NDBLayer.ID
{
    public class Bid
    {
        public ExternalOrInternalBid ExternalOrInternalBid { get; set; }
        public bool IsInternal { get; set; }
        public ulong BId { get; set; }//64 bit unsigned integer 0 to +value
        public bool Ar { get; set; }
        public bool Bi { get; set; }
        public ulong bidIndex { get; set; }
        public Bid(ulong bid)
        {

            BId = ResetLeastSignificantBit(bid);
            Ar = false;
            var bicalc = BId & 0x02;//Taking 2nd most least significant bit
            IsInternal = bicalc > 0;
            bidIndex = BId >> 2 & 0x3FFFFFFFFFFFFFFF;//Shifting and taking 62 bits
            ExternalOrInternalBid = IsInternal ?
                    ExternalOrInternalBid.Internal : ExternalOrInternalBid.External;
        }
        private ulong ResetLeastSignificantBit(ulong value)
        {
            ulong sixtyFourBitWithZeroAsLeastSignificantBit = 0xfffffffffffffffe;
            var normalizedBID = value & sixtyFourBitWithZeroAsLeastSignificantBit;
            return normalizedBID;
        }
    }
}