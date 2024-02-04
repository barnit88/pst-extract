using core.NDBLayer.Blocks;
using core.NDBLayer.BREF;
using core.NDBLayer.Headers;
using System;
using System.IO.MemoryMappedFiles;

namespace core.NDBLayer.Pages.BTree
{
    public class BlockBTreeEntry : IBTPageEntry
    {
        public BTreePageEntriesType BTreePageEntriesType { get; set; } = BTreePageEntriesType.BBTENTRY;
        public Bref Bref { get; set; }
        public Block Block { get; set; }


        #region Flags
        public byte[] BREF = new byte[16];
        public ushort cb;
        public ushort cRef;
        public uint dwPadding;
        #endregion
        public BlockBTreeEntry(byte[] blockBTreeDataBytes, MemoryMappedFile file, bCryptMethodType encodignType)
        {
            byte[] brefDataBytes = new byte[16];
            Array.Copy(blockBTreeDataBytes, 0, brefDataBytes, 0, 16);
            BREF = brefDataBytes;
            Bref = new Bref(brefDataBytes);
            cb = BitConverter.ToUInt16(blockBTreeDataBytes, 16);
            cRef = BitConverter.ToUInt16(blockBTreeDataBytes, 18);
            dwPadding = 0;
            Block = new Block(file, Bref, cb, encodignType);
        }
    }
}