using core.LTP.BTreeOnHeap;
using core.LTP.HeapNode;
using core.NDBLayer;
using System;
using System.Collections.Generic;

namespace core.LTP.PropertyContext
{
    public class PropertyContext
    {
        public Dictionary<UInt16, ExchangeProperty> Properties;//Copied
        public BTH BTH { get; set; }
        public UInt16 wPropId { get; set; }
        public UInt16 wPropType { get; set; }
        public UInt32 dwValueHnid { get; set; }
        public PropertyContext(NodeDataDTO node)
        {
            var heapOnNode = new HeapOnNode(node);
            this.BTH = new BTH(heapOnNode);
            this.Properties = BTH.GetExchangeProperties();
        }
    }
}