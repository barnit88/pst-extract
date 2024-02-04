using core.LTP.HeapNode;
using System;
using System.Collections.Generic;

namespace core.LTP.BTreeOnHeap
{
    public class BTHIndex
    {
        public int Level { get; set; }
        public BTHData BTHData { get; set; } = null;
        public List<BTHIndex> BTHIndexes { get; set; } = null;
        public List<BTHIndexEntry> BTHIndexEntries { get; set; } = null;
        public HID HidRoot { get; set; } = null;
        public byte[] key { get; set; }
        public UInt32 hidNextLevel { get; set; }
        public BTHIndex(BTH bTreeOnHeap, int levels)
        {
            this.Level = levels;//Levels
            this.HidRoot = bTreeOnHeap.BTHHeader.HidRoot;
            if (this.HidRoot.hidIndex == 0)
                return;
            this.BTHIndexEntries = new List<BTHIndexEntry>();
            if (Level == 0)
                this.BTHData = new BTHData(HidRoot, bTreeOnHeap);
            else
            {
                var tempBytes = HeapOnNode.GetHNHIDBytes(bTreeOnHeap.HeapOnNode, HidRoot);
                for (int i = 0; i < tempBytes.Length; i += (int)bTreeOnHeap.BTHHeader.CbKey + 4)
                    this.BTHIndexEntries.Add(new BTHIndexEntry(tempBytes, bTreeOnHeap.BTHHeader, i));
                this.BTHIndexes = new List<BTHIndex>();
                foreach (var entry in this.BTHIndexEntries)
                    this.BTHIndexes.Add(new BTHIndex(bTreeOnHeap, this.Level - 1));
            }
        }
    }
}