using core.LTP.HeapNode;
using System.Collections.Generic;

namespace core.LTP.BTreeOnHeap
{
    public class BTHData
    {
        public List<BTHDataEntry> BTHDataEntries { get; set; }
        public byte[] key { get; set; }
        public byte[] data { get; set; }
        public BTHData(HID hid, BTH tree)
        {
            this.data = HeapOnNode.GetHNHIDBytes(tree.HeapOnNode, hid);
            this.BTHDataEntries = new List<BTHDataEntry>();
            var keyValueLengthInBytes = (int)(tree.BTHHeader.CbKey + tree.BTHHeader.CbEnt);
            for (int i = 0; i < this.data.Length; i += keyValueLengthInBytes)
                this.BTHDataEntries.Add(new BTHDataEntry(this.data, i, tree));
        }
    }
}