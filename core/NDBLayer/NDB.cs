using core.NDBLayer.Blocks;
using core.NDBLayer.ID;
using core.NDBLayer.Pages.BTree;
using System;
using System.Collections.Generic;

namespace core.NDBLayer
{
    public class NDB
    {
        public static NodeDataDTO GetNodeDataFromNodeBTreeEntry(NodeBTreeEntry nodeBTreeEntry,
            List<IBTPageEntry> blockBTPageEntries)
        {
            NodeDataDTO nodeData = new NodeDataDTO();
            nodeData.NodeID = nodeBTreeEntry.nid;
            nodeData.NodeBlockId = nodeBTreeEntry.bidData;
            nodeData.SubNodeBlockId = nodeBTreeEntry.bidSub;
            BlockBTreeEntry nodeBlockBTreeEntry = GetBlockBTreeEntryFromBid(nodeData.NodeBlockId, blockBTPageEntries);
            nodeData.NodeData = GetDataBlocksFromNodeBlockBTreeEntry(nodeBlockBTreeEntry, blockBTPageEntries);
            if (nodeData.SubNodeBlockId != 0)
            {
                BlockBTreeEntry subNodeBlockBTreeEntry = GetBlockBTreeEntryFromBid(nodeData.SubNodeBlockId, blockBTPageEntries);
                nodeData.SubNodeData = GetSubNodesDataBlockFromSubNodeBlockBTreeEntry(subNodeBlockBTreeEntry, blockBTPageEntries);
            }
            return nodeData;
        }
        private static NodeDataDTO GetNodeDataFromNodeBlockBTreeEntry
            (BlockBTreeEntry nodesBlockBTreeEntry,
            List<IBTPageEntry> blockBTPageEntries)
        {
            NodeDataDTO nodeData = new NodeDataDTO();
            List<DataBlock> dataBlocks = new List<DataBlock>();
            var currentBlock = nodesBlockBTreeEntry.Block;
            if (currentBlock.BlockType == BlockType.DATABLOCK)
                dataBlocks.Add(currentBlock.DataBlock);
            else if (currentBlock.BlockType == BlockType.XBLOCK)
                foreach (var bid in currentBlock.XBlock.DataBlockBids)
                    dataBlocks.Add(GetDataBlockFromBid(bid.BId, blockBTPageEntries));
            if (currentBlock.BlockType == BlockType.XXBLOCK)
            {
                foreach (var xBlockBid in currentBlock.XXBlock.XBlockBids)
                {
                    Block block = GetBlockBTreeEntryFromBid(xBlockBid.BId, blockBTPageEntries).Block;
                    if (block.BlockType != BlockType.XBLOCK)
                        throw new Exception("Block Type inside XXBlock is not of XBlock");
                    XBlock xBlock = block.XBlock;
                    foreach (var dataBlockBid in xBlock.DataBlockBids)
                        dataBlocks.Add(GetDataBlockFromBid(dataBlockBid.BId, blockBTPageEntries));
                }
            };
            nodeData.NodeData = dataBlocks;
            return nodeData;
        }
        public static List<DataBlock> GetDataBlocksFromNodeBlockBTreeEntry
            (BlockBTreeEntry nodesBlockBTreeEntry,
            List<IBTPageEntry> blockBTPageEntries)
        {
            List<DataBlock> dataBlocks = new List<DataBlock>();
            var currentBlock = nodesBlockBTreeEntry.Block;
            if (currentBlock.BlockType == BlockType.DATABLOCK)
                dataBlocks.Add(currentBlock.DataBlock);
            else if (currentBlock.BlockType == BlockType.XBLOCK)
                foreach (var bid in currentBlock.XBlock.DataBlockBids)
                    dataBlocks.Add(GetDataBlockFromBid(bid.BId, blockBTPageEntries));
            if (currentBlock.BlockType == BlockType.XXBLOCK)
            {
                foreach (var xBlockBid in currentBlock.XXBlock.XBlockBids)
                {
                    Block block = GetBlockBTreeEntryFromBid(xBlockBid.BId, blockBTPageEntries).Block;
                    if (block.BlockType != BlockType.XBLOCK)
                        throw new Exception("Block Type inside XXBlock is not of XBlock");
                    XBlock xBlock = block.XBlock;
                    foreach (var dataBlockBid in xBlock.DataBlockBids)
                        dataBlocks.Add(GetDataBlockFromBid(dataBlockBid.BId, blockBTPageEntries));
                }
            };
            return dataBlocks;
        }
        public static List<NodeDataDTO> GetSubNodesDataBlockFromSubNodeBlockBTreeEntry
            (BlockBTreeEntry subNodesBlockBTreeEntry, List<IBTPageEntry> blockBTPageEntries)
        {
            List<NodeDataDTO> subNodes = new List<NodeDataDTO>();
            Block subNodesBlock = subNodesBlockBTreeEntry.Block;
            if (!(subNodesBlock.BlockType == BlockType.SLBLOCK || subNodesBlock.BlockType == BlockType.SIBLOCK))
                throw new Exception("Not a valid SubNode Block");
            //Reference subnode of node
            if (subNodesBlock.BlockType == BlockType.SLBLOCK)
                subNodes.AddRange(GetNodeDatasFromSLBlock(subNodesBlock.SLBlock, blockBTPageEntries));
            if (subNodesBlock.BlockType == BlockType.SIBLOCK)
                subNodes.AddRange(GetNodeDatasFromSIBlock(subNodesBlock.SIBlock, blockBTPageEntries));
            return subNodes;
        }
        private static List<NodeDataDTO> GetNodeDatasFromSIBlock
            (SIBlock siBlock, List<IBTPageEntry> blockBTPageEntries)
        {
            List<NodeDataDTO> nodes = new List<NodeDataDTO>();
            foreach (var siEntry in siBlock.SIEntries)
                nodes.AddRange(GetNodeDatasFromSIEntry(siEntry, blockBTPageEntries));
            return nodes;
        }
        private static List<NodeDataDTO> GetNodeDatasFromSIEntry(SIEntry siEntry, List<IBTPageEntry> blockBTPageEntries)
        {
            Block block = GetBlockBTreeEntryFromBid(siEntry.bid, blockBTPageEntries).Block;
            if (block.BlockType != BlockType.SLBLOCK)
                throw new Exception("Block Type pointed by SIEntries is not of type SLBlock");
            SLBlock slBlock = block.SLBlock;
            return GetNodeDatasFromSLBlock(slBlock, blockBTPageEntries);
        }
        private static List<NodeDataDTO> GetNodeDatasFromSLBlock
            (SLBlock slBlock, List<IBTPageEntry> blockBTPageEntries)
        {
            List<NodeDataDTO> nodes = new List<NodeDataDTO>();
            foreach (var slEntry in slBlock.SLEntries)
                nodes.Add(GetNodeDataFromSLEntry(slEntry, blockBTPageEntries));
            return nodes;
        }
        private static NodeDataDTO GetNodeDataFromSLEntry(SLEntry slEntry, List<IBTPageEntry> blockBTPageEntries, ulong parentNid = 0)
        {
            NodeDataDTO node = new NodeDataDTO();
            node.NodeID = slEntry.nid;
            node.NodeBlockId = slEntry.bidData;
            node.SubNodeBlockId = slEntry.bidSub;
            BlockBTreeEntry currentNodesBlockBTreeEntry =
                    GetBlockBTreeEntryFromBid(slEntry.bidData, blockBTPageEntries);
            node.NodeData =
                    GetDataBlocksFromNodeBlockBTreeEntry(currentNodesBlockBTreeEntry, blockBTPageEntries);
            if (node.SubNodeBlockId != 0)
            {
                BlockBTreeEntry subNodeBlockBTreeEntry =
                    GetBlockBTreeEntryFromBid(slEntry.bidSub, blockBTPageEntries);
                node.SubNodeData =
                    GetSubNodesDataBlockFromSubNodeBlockBTreeEntry(subNodeBlockBTreeEntry, blockBTPageEntries);
            }
            return node;
        }
        public static DataBlock GetDataBlockFromBid(ulong bid, List<IBTPageEntry> blockBTPageEntries)
        {
            BlockBTreeEntry blockBTreeEntry = GetBlockBTreeEntryFromBid(bid, blockBTPageEntries);
            if (blockBTreeEntry.Block.BlockType != BlockType.DATABLOCK)
                throw new Exception("Block is not a Data Block");
            return blockBTreeEntry.Block.DataBlock;
        }
        public static NodeBTreeEntry GetNodeBTreeEntryFromNid(ulong nid, List<IBTPageEntry> nodeBTPageEntries)
        {
            for (int i = 0; i < nodeBTPageEntries.Count; i++)
            {
                var currentEntry = nodeBTPageEntries[i];
                var nextEntry = nodeBTPageEntries[nodeBTPageEntries.Count == i + 1 ? i : i + 1];

                if (currentEntry.BTreePageEntriesType == BTreePageEntriesType.BTENTRY)
                {
                    var currentBTPageEntry = (BTEntry)currentEntry;
                    var nextBTPageEntry = (BTEntry)nextEntry;
                    if (nid >= currentBTPageEntry.btkey && nid < nextBTPageEntry.btkey)
                        return GetNodeBTreeEntryFromNid(nid, currentBTPageEntry.bTreePage.BTPageEntries);
                    else if (i == nodeBTPageEntries.Count - 1 && nid >= nextBTPageEntry.btkey)
                        return GetNodeBTreeEntryFromNid(nid, currentBTPageEntry.bTreePage.BTPageEntries);
                }
                else if (currentEntry.BTreePageEntriesType == BTreePageEntriesType.NBTENTRY)
                {
                    var nodeBTPageEntry = (NodeBTreeEntry)currentEntry;
                    if (nodeBTPageEntry.nid == nid)
                        return nodeBTPageEntry;
                }
                else if (currentEntry.BTreePageEntriesType == BTreePageEntriesType.BBTENTRY)
                    throw new Exception("GetNidFromBid | Unexpected BBTEntry ");
                else
                    throw new Exception("GetNidFromBid | BTPageEntryType error");
            }
            throw new Exception("GetNodeBTreeEntryFromNid | No any NodeBTree found for provided Nid");
        }
        public static BlockBTreeEntry GetBlockBTreeEntryFromBid(ulong bid, List<IBTPageEntry> blockBTPageEntries)
        {
            bid = bid & 0xfffffffffffffffe;
            for (int i = 0; i < blockBTPageEntries.Count; i++)
            {
                var currentEntry = blockBTPageEntries[i];
                var nextEntry = blockBTPageEntries[blockBTPageEntries.Count == i + 1 ? i : i + 1];

                if (currentEntry.BTreePageEntriesType == BTreePageEntriesType.BTENTRY)
                {
                    var currentBTPageEntry = (BTEntry)currentEntry;
                    var nextBTPageEntry = (BTEntry)nextEntry;
                    if (bid >= currentBTPageEntry.btkey && bid < nextBTPageEntry.btkey)
                        return GetBlockBTreeEntryFromBid(bid, currentBTPageEntry.bTreePage.BTPageEntries);
                    else if (i == blockBTPageEntries.Count - 1 && bid >= nextBTPageEntry.btkey)
                        return GetBlockBTreeEntryFromBid(bid, currentBTPageEntry.bTreePage.BTPageEntries);
                }
                else if (currentEntry.BTreePageEntriesType == BTreePageEntriesType.BBTENTRY)
                {
                    var blockBTPageEntry = (BlockBTreeEntry)currentEntry;
                    if (blockBTPageEntry.Bref.bid == bid)
                        return blockBTPageEntry;
                }
                else if (currentEntry.BTreePageEntriesType == BTreePageEntriesType.NBTENTRY)
                    throw new Exception("GetNidFromBid | Unexpected BBTEntry ");
                else
                    throw new Exception("GetNidFromBid | BTPageEntryType error");
            }
            throw new Exception("GetNidFromBid | No any NodeBTree found for provided Nid");
        }
    }
}
