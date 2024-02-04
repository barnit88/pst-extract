using core.NDBLayer.BREF;
using core.NDBLayer.Headers;
using core.NDBLayer.Pages.Base;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace core.NDBLayer.Pages.BTree
{
    public class BTreePage : Page
    {
        public const string PageTypeName = "Block Tree Page(BTPage)";
        public BTreeType BTreeType { get; set; }
        public List<IBTPageEntry> BTPageEntries { get; set; }
        public BTreePageEntriesType BTreePageEntriesType { get; set; }
        #region Flags
        protected byte[] rgentries;
        protected byte cEnt;
        protected byte cEntMax;
        protected byte cbEnt;
        protected byte cLevel;
        protected int dwPadding;
        protected byte[] pageTrailer;
        #endregion
        public BTreePage(MemoryMappedFile mmf, Bref bref, BTreeType bTreeType,bCryptMethodType encodignType)
        {
            BTreeType = bTreeType;
            using (MemoryMappedViewAccessor view =
              mmf.CreateViewAccessor((long)bref.ib, PageSize))
            {
                rgentries = new byte[488];
                view.ReadArray(0, rgentries, 0, 488);
                cEnt = view.ReadByte(488);//Number of BTree Entries
                cEntMax = view.ReadByte(489);//Maximum Number of BTree Entries that can fit inside the page
                cbEnt = view.ReadByte(490);//Size of each BTree Entry
                cLevel = view.ReadByte(491);//The depth level of this page
                dwPadding = view.ReadInt32(492);
                pageTrailer = new byte[16];
                view.ReadArray(PageSize - 16, pageTrailer, 0, 16);
                if (cLevel == 0)
                {
                    if (bTreeType == BTreeType.NBT)
                        BTreePageEntriesType = BTreePageEntriesType.NBTENTRY;
                    else if (bTreeType == BTreeType.BBT)
                        BTreePageEntriesType = BTreePageEntriesType.BBTENTRY;
                }
                else
                    BTreePageEntriesType = BTreePageEntriesType.BTENTRY;
                PageTrailer = new PageTrailer(pageTrailerByte: pageTrailer, bref: bref);
                ConfigureBTreeEntries(view, mmf, bTreeType,encodignType);
            }
        }

        private void ConfigureBTreeEntries
            (MemoryMappedViewAccessor view, MemoryMappedFile file, BTreeType bTreeType, bCryptMethodType encodignType)
        {
            BTPageEntries = new List<IBTPageEntry>();
            for (var i = 0; i < cEnt; i++)
            {
                byte[] curEntryBytes = new byte[cbEnt];
                view.ReadArray(i * cbEnt, curEntryBytes, 0, cbEnt);
                if (BTreePageEntriesType == BTreePageEntriesType.NBTENTRY)
                    BTPageEntries.Add(new NodeBTreeEntry(curEntryBytes));
                else if (BTreePageEntriesType == BTreePageEntriesType.BBTENTRY)
                    BTPageEntries.Add(new BlockBTreeEntry(curEntryBytes, file, encodignType));
                else if (BTreePageEntriesType == BTreePageEntriesType.BTENTRY)
                    BTPageEntries.Add(new BTEntry(file, curEntryBytes,
                        bTreeType == BTreeType.NBT ? BTreeEntryType.NBTreeEntry : BTreeEntryType.BBTreeEntry, 
                        encodignType));
            }
        }
        private void ConfigurecbEnt(byte cLevel)
        {
            if (cLevel == 0) { }

        }
    }
}
