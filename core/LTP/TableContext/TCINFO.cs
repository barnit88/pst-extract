using core.LTP.HeapNode;
using System;
using System.Collections.Generic;

namespace core.LTP.TableContext
{
    public class TCINFO
    {
        public HID HIDRowMatrixLocation { get; set; }
        public HID HIDRowIndexLocation { get; set; }
        public List<TCOLDESC> ColumnsDescriptors { get; set; } = new List<TCOLDESC>();
        public byte bType { get; set; }
        public byte cCols { get; set; }
        public Int16[] rgib { get; set; } = new Int16[4];
        public UInt32 hidRowIndex { get; set; }
        public UInt32 hnidRows { get; set; }
        public UInt32 hidIndex { get; set; }
        public byte[] rgTCOLDESC { get; set; }
        public TCINFO(byte[] dataBytes)
        {
            this.bType = dataBytes[0];
            this.cCols = dataBytes[1];
            for (int i = 0; i < 4; i++)
            {
                int tempOffset = (i * 2) + 2;
                this.rgib[i] = BitConverter.ToInt16(dataBytes, tempOffset);
            }
            this.hidRowIndex = BitConverter.ToUInt32(dataBytes, 10);
            byte[] tempHidRowIndexBytes = new byte[4];
            Array.Copy(dataBytes, 10, tempHidRowIndexBytes, 0, 4);
            this.HIDRowIndexLocation = new HID(tempHidRowIndexBytes);
            this.hnidRows = BitConverter.ToUInt32(dataBytes, 14);
            byte[] tempHidRowMatrixBytes = new byte[4];
            Array.Copy(dataBytes, 14, tempHidRowMatrixBytes, 0, 4);
            this.HIDRowMatrixLocation = new HID(tempHidRowMatrixBytes);
            this.hidIndex = BitConverter.ToUInt32(dataBytes, 18);//Depreciated value.Should be ignored
            for (int i = 0; i < this.cCols; i++)
            {
                int colDescOffset = 22 + (i * 8);
                this.ColumnsDescriptors.Add(new TCOLDESC(dataBytes, colDescOffset));
            }
        }
    }
}