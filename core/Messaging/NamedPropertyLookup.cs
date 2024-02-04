using core.LTP.PropertyContext;
using core.NDBLayer;
using core.NDBLayer.Pages.BTree;
using System.Collections.Generic;

namespace core.Messaging
{
    public class NamedPropertyLookup
    {
        public static ulong NODE_ID = 0x61;

        public PropertyContext PC;
        public Dictionary<ushort, NameId> Lookup;

        internal byte[] _GUIDs;
        internal byte[] _entries;
        internal byte[] _string;
        public NamedPropertyLookup(List<IBTPageEntry> nodeBTPageEntries, List<IBTPageEntry> blockBTPageEntries)
        {
            NodeBTreeEntry nodeBTreeEntry = NDB.GetNodeBTreeEntryFromNid(NamedPropertyLookup.NODE_ID, nodeBTPageEntries);
            NodeDataDTO node = NDB.GetNodeDataFromNodeBTreeEntry(nodeBTreeEntry, blockBTPageEntries);
            this.PC = new PropertyContext(node);
            this._GUIDs = this.PC.Properties[0x0002].Data;
            this._entries = this.PC.Properties[0x0003].Data;
            this._string = this.PC.Properties[0x0004].Data;
            this.Lookup = new Dictionary<ushort, NameId>();
            for (int i = 0; i < this._entries.Length; i += 8)
            {
                var cur = new NameId(this._entries, i, this);
                this.Lookup.Add(cur.wPropIdx, cur);
            }
        }
    }
}
