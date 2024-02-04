using core.NDBLayer.Blocks;
using System.Collections.Generic;

namespace core.NDBLayer
{
    public class NodeDataDTO
    {
        public ulong ParentNodeID { get; set; }
        public ulong NodeID { get; set; }
        public ulong NodeBlockId { get; set; }
        public ulong SubNodeBlockId { get; set; }
        public List<DataBlock> NodeData { get; set; }
        public List<NodeDataDTO> SubNodeData { get; set; }
    }
}
