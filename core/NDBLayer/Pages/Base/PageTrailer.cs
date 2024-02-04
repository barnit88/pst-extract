using core.NDBLayer.BREF;
using System;

namespace core.NDBLayer.Pages.Base
{
    public class PageTrailer
    {
        public PageType PageType { get; set; }

        #region Flags
        protected byte ptype;
        protected byte ptypeRepeat;
        protected ushort wSig;
        protected int dwCRC;
        public ulong Bid { get; set; }
        ulong bid;
        #endregion
        public PageTrailer(byte[] pageTrailerByte, Bref bref)
        {
            PageType = (PageType)pageTrailerByte[0];
            if (pageTrailerByte.Length != 16)
                throw new Exception("Page Trailer Byte Lenth Error");
            ptype = pageTrailerByte[0];
            PageType = (PageType)ptype;
            ptypeRepeat = pageTrailerByte[1];
            wSig = BitConverter.ToUInt16(pageTrailerByte, 2);
            dwCRC = BitConverter.ToInt32(pageTrailerByte, 4);
            bid = BitConverter.ToUInt64(pageTrailerByte, 8);
            if (bid != ComputeBid(PageType, bref))
                throw new Exception("Unicode Page Trailer Error. Bid Mismatch");
        }
        private ulong ComputeBid(PageType pageType, Bref bref)
        {
            if ((byte)pageType == 0x82)//ptypeFMap
                return bref.ib;
            else if ((byte)pageType == 0x83)//ptypePMap
                return bref.ib;
            else if ((byte)pageType == 0x84)//ptypeAMap
                return bref.ib;
            else if ((byte)pageType == 0x85)//ptypeFPMap
                return bref.ib;
            else
                return bref.bid;
        }
        private void ComputePageType(byte ptype)
        {
            if (ptype == 0x80)
                PageType = PageType.ptypeBBT;
            else if (ptype == 0x81)
                PageType = PageType.ptypeNBT;
            else if (ptype == 0x82)
                PageType = PageType.ptypeFMap;
            else if (ptype == 0x83)
                PageType = PageType.ptypePMap;
            else if (ptype == 0x84)
                PageType = PageType.ptypeAMap;
            else if (ptype == 0x84)
                PageType = PageType.ptypeFPMap;
            else if (ptype == 0x86)
                PageType = PageType.ptypeDL;
            throw new Exception("Invalid Page Type Exception");
        }
        private void CheckPTypeRepeat()
        {
            if (ptypeRepeat != ptype)
            {
                throw new Exception("Page Trailer ptype not match with ptypeRepeat");
            }
        }
        private void ComputeSig(ulong bid, ulong ib)
        {
            ib ^= bid;
            var wsig = (ushort)(ib >> 16 ^ (ushort)ib);
        }
    }
}
