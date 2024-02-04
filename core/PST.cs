using core.NDBLayer.Headers;
using core.NDBLayer.Pages.BTree;
using System.IO.MemoryMappedFiles;

namespace core
{
    public class PST
    {
        public Header Header { get; set; }
        public BTreePage NodeBTPage { get; set; }
        public BTreePage BlockBTPage { get; set; }
        public MemoryMappedFile MemoryMappedPSTFile { get; set; }
        public PST(MemoryMappedFile file)
        {
            this.MemoryMappedPSTFile = file;
            this.Header = new Header(file);
            if (this.Header.IsUnicode)
            {
                //Reference to this NDB Layer Pages are stored in root of the header of the PST file
                this.NodeBTPage =
                    new BTreePage(file, this.Header.UnicodeHeader.Root.NBTBREF, BTreeType.NBT,
                    this.Header.UnicodeHeader.EncodingFriendlyName);

                this.BlockBTPage =
                    new BTreePage(file, this.Header.UnicodeHeader.Root.BBTBREF, BTreeType.BBT,
                    this.Header.UnicodeHeader.EncodingFriendlyName);
            }
        }
    }
}