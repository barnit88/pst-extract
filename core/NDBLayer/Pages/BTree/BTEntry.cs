using core.NDBLayer.BREF;
using core.NDBLayer.Headers;
using System;
using System.IO.MemoryMappedFiles;

namespace core.NDBLayer.Pages.BTree
{
    public class BTEntry : IBTPageEntry
    {
        public BTreePageEntriesType BTreePageEntriesType { get; set; } = BTreePageEntriesType.BTENTRY;
        public BTreePage bTreePage { get; set; }
        public BTreeEntryType BTreeEntryType { get; set; }
        public Bref Bref { get; set; }

        #region Flags
        public ulong btkey;
        public byte[] BREF = new byte[16];
        #endregion 

        public BTEntry(MemoryMappedFile mmf, byte[] btentryBytes, BTreeEntryType bTreeEntryType,bCryptMethodType encodignType)
        {
            BTreeEntryType = bTreeEntryType;
            btkey = BitConverter.ToUInt64(btentryBytes, 0);
            byte[] brefData = new byte[16];
            Array.Copy(btentryBytes, 8, brefData, 0, 16);
            Bref = new Bref(brefData);
            if (bTreeEntryType == BTreeEntryType.NBTreeEntry)
                bTreePage = new BTreePage(mmf, Bref, BTreeType.NBT, encodignType);
            else if (bTreeEntryType == BTreeEntryType.BBTreeEntry)
                bTreePage = new BTreePage(mmf, Bref, BTreeType.BBT, encodignType);
        }
    }
}
