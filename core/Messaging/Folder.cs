using core.LTP.PropertyContext;
using core.LTP.TableContext;
using core.NDBLayer;
using core.NDBLayer.ID;
using core.NDBLayer.Pages.BTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace core.Messaging
{
    public class Folder : IEnumerable<IPMItem>
    {
        public PropertyContext PropertyContext { get; set; }
        public TableContext HierarchyTable { get; set; }
        public TableContext ContentsTable { get; set; }
        public TableContext AssociatedContentsTable { get; set; }
        public List<Folder> SubFolders { get; set; }
        public string DisplayName { get; set; }
        public List<string> Path { get; set; }
        private List<IBTPageEntry> NodeBTPageEntries;
        private List<IBTPageEntry> BlockBTPageEntries;
        public Folder(Nid nid, List<string> path, List<IBTPageEntry> nodeBTPageEntries, List<IBTPageEntry> blockBTPageEntries)
        {
            this.NodeBTPageEntries = nodeBTPageEntries;
            this.BlockBTPageEntries = blockBTPageEntries;
            var pcNid = ((nid._Nid >> 5) << 5) | 0x02;
            var hirearchyTableNid = ((nid._Nid >> 5) << 5) | 0x0D;
            var contentsTableNid = ((nid._Nid >> 5) << 5) | 0x0E;
            var associatedContentsTableNid = ((nid._Nid >> 5) << 5) | 0x0F;

            this.PropertyContext = new PropertyContext(GetNodeDataFromNid(pcNid, nodeBTPageEntries, blockBTPageEntries));
            this.DisplayName = Encoding.Unicode.GetString(this.PropertyContext.Properties[(ushort)FolderProperty.PidTagDisplayName].Data);
            this.Path = new List<string>(path);
            this.Path.Add(DisplayName);
            this.HierarchyTable = new TableContext(GetNodeDataFromNid(hirearchyTableNid, nodeBTPageEntries, blockBTPageEntries));
            var hasSubFolder = BitConverter.ToBoolean(this.PropertyContext.Properties[(ushort)FolderProperty.PidTagSubfolders].Data);
            if (hasSubFolder && this.HierarchyTable.ReverseRowIndex.Count > 0)
            {
                this.SubFolders = new List<Folder>();
                foreach (var row in this.HierarchyTable.ReverseRowIndex)
                {
                    this.SubFolders.Add(new Folder(new Nid(row.Value), this.Path, nodeBTPageEntries, blockBTPageEntries));
                }
            }
            this.ContentsTable = new TableContext(GetNodeDataFromNid(contentsTableNid, nodeBTPageEntries, blockBTPageEntries));
            this.AssociatedContentsTable = new TableContext(GetNodeDataFromNid(associatedContentsTableNid, nodeBTPageEntries, blockBTPageEntries));
        }
        public NodeDataDTO GetNodeDataFromNid(ulong nid, List<IBTPageEntry> nodeBTPageEntries, List<IBTPageEntry> blockBTPageEntries)
        {
            NodeBTreeEntry nodeBTreeEntry = NDB.GetNodeBTreeEntryFromNid(nid, nodeBTPageEntries);
            NodeDataDTO nodeData = NDB.GetNodeDataFromNodeBTreeEntry(nodeBTreeEntry, blockBTPageEntries);
            return nodeData;
        }

        public IEnumerator<IPMItem> GetEnumerator()
        {
            foreach (var row in this.ContentsTable.ReverseRowIndex)
            {
                NodeDataDTO node = GetNodeDataFromNid(row.Value, this.NodeBTPageEntries, this.BlockBTPageEntries);
                var curItem = new IPMItem(node);
                yield return new MessageObject(curItem, node);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
