using core.LTP.BTreeOnHeap;
using core.LTP.PropertyContext;
using System;
using System.Collections;
using System.Collections.Generic;

namespace core.LTP.TableContext
{
    public class TCRowData : IEnumerable<ExchangeProperty>
    {
        public Dictionary<uint, byte[]> ColumnXREF;
        private BTH BTH { get; set; }
        public Int32 dwRowID { get; set; }
        public byte[] rgdwData { get; set; }
        public byte[] rgwData { get; set; }
        public byte[] rgbData { get; set; }
        public byte[] rgbCEB { get; set; }
        public TCRowData(byte[] bytes, TableContext context, BTH bth, int offset = 0)
        {
            this.ColumnXREF = new Dictionary<uint, byte[]>();
            this.BTH = bth;
            foreach (var col in context.TCHeader.ColumnsDescriptors)
                this.ColumnXREF.Add((uint)col.tag, bytes.RangeSubset(offset + col.ibData, col.cbData));
        }
        public IEnumerator<ExchangeProperty> GetEnumerator()
        {
            foreach (var col in this.ColumnXREF)
                yield return new ExchangeProperty((UInt16)(col.Key >> 16), (UInt16)(col.Key & 0xFFFF), this.BTH, col.Value);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
