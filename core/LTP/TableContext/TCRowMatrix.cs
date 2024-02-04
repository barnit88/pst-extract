using core.LTP.BTreeOnHeap;
using core.LTP.HeapNode;
using core.NDBLayer;
using core.NDBLayer.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.LTP.TableContext
{
    public class TCRowMatrix
    {
        public TableContext TableContext;
        public List<DataBlock> TCRMData;

        public List<TCRowData> Rows;
        public Dictionary<uint, TCRowData> RowXREF;
        public TCRowMatrix(TableContext tableContext, BTH heap)
        {
            this.Rows = new List<TCRowData>();
            this.RowXREF = new Dictionary<uint, TCRowData>();
            this.TCRMData = new List<DataBlock>();

            this.TableContext = tableContext;
            var rowMatrixHNID = this.TableContext.TCHeader.HIDRowMatrixLocation;
            if (rowMatrixHNID.hid == 0)
                return;

            if ((rowMatrixHNID.hid & 0x1F) == 0)//HID
            {
                var blockData = HeapOnNode.GetHNHIDBytes(this.TableContext.HeapOnNode, rowMatrixHNID);
                this.TCRMData.Add(new DataBlock(blockData));
            }
            else
            {
                var subNodeData = this.TableContext.HeapOnNode.Node.SubNodeData.FirstOrDefault(x => x.NodeID == rowMatrixHNID.hid);
                if (subNodeData != null)
                    this.TCRMData = subNodeData.NodeData;
                else
                {
                    var tempSubNodes = new Dictionary<ulong, NodeDataDTO>();
                    foreach (var nod in this.TableContext.HeapOnNode.Node.SubNodeData)
                        tempSubNodes.Add(nod.NodeID & 0xffffffff, nod);
                    this.TCRMData = tempSubNodes[rowMatrixHNID.hid].NodeData;
                }
            }
            var rowSize = this.TableContext.TCHeader.rgib[3];
            foreach (var row in this.TableContext.RowIndexBTH.Properties)
            {
                var rowIndex = BitConverter.ToUInt32(row.Value.Data, 0);
                var blockTrailerSize = 16;
                var maxBlockSize = 8192 - blockTrailerSize;
                var recordsPerBlock = maxBlockSize / rowSize;
                var blockIndex = (int)rowIndex / recordsPerBlock;
                var indexInBlock = rowIndex % recordsPerBlock;
                var curRow = new TCRowData(this.TCRMData[blockIndex].data, this.TableContext, heap,
                                                 (int)indexInBlock * rowSize);
                this.RowXREF.Add(BitConverter.ToUInt32(row.Key, 0), curRow);
                this.Rows.Add(curRow);
            }
        }
    }
}