using core.LTP.BTreeOnHeap;
using core.LTP.HeapNode;
using core.NDBLayer;
using System;
using System.Collections.Generic;

namespace core.LTP.TableContext
{
    public class TableContext
    {
        public TCINFO TCHeader { get; set; }
        public List<TCOLDESC> TCDescriptor { get; set; } = new List<TCOLDESC>();
        public HeapOnNode HeapOnNode { get; set; }
        public BTH RowIndexBTH { get; set; }
        public NodeDataDTO nodeData { get; set; }
        public TCRowMatrix TCRowMatrix { get; set; }
        public Dictionary<uint, uint> ReverseRowIndex;
        public TableContext(NodeDataDTO nodeData)
        {
            this.nodeData = nodeData;
            this.HeapOnNode = new HeapOnNode(nodeData);
            var tcinfoHID = this.HeapOnNode.HeapOnNodeDataBlocks[0].HNHeader.RootHId;
            var tcinfoHIDbytes = HeapOnNode.GetHNHIDBytes(this.HeapOnNode, tcinfoHID);
            this.TCHeader = new TCINFO(tcinfoHIDbytes);
            this.RowIndexBTH = new BTH(this.HeapOnNode, this.TCHeader.HIDRowIndexLocation);
            this.ReverseRowIndex = new Dictionary<uint, uint>();
            foreach (var prop in this.RowIndexBTH.Properties)
            {
                var temp = BitConverter.ToUInt32(prop.Value.Data, 0);
                this.ReverseRowIndex.Add(temp, BitConverter.ToUInt32(prop.Key, 0));
            }
            this.TCRowMatrix = new TCRowMatrix(this, this.RowIndexBTH);
        }
    }
}
