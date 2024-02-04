using core.NDBLayer.Headers.Ansi;
using core.NDBLayer.Headers.Unicode;
using System;
using System.IO.MemoryMappedFiles;

namespace core.NDBLayer.Headers
{
    public class Header
    {
        private int offset = 0;//First byte 0 byte
        private int length = 580;//580th byte . Total bytes occupied by header
        public UnicodeHeader UnicodeHeader { get; set; } = null;
        public AnsiHeader AnsiHeader { get; set; } = null;
        public string FileFormat { get; set; } = null;
        public bool IsUnicode { get; set; } = false;
        public bool IsAnsi { get; set; } = false;
        int dwMagic { get; set; }
        int dwCRCPartial { get; set; }
        short wMagicClient { get; set; }
        short wVer { get; set; }
        short wVerClient { get; set; }
        byte bPlatformCreate { get; set; }
        byte bPlatformAccess { get; set; }
        int dwReserved1 { get; set; }
        int dwReserved2 { get; set; }
        public Header(MemoryMappedFile memoryMappedFile)
        {
            using (var view = memoryMappedFile.CreateViewAccessor(offset, length))
            {
                CheckdwMagic(view);
                dwCRCPartial = view.ReadInt32(4);
                CheckwMagicClient(view);
                CheckwVer(view);
                CheckwVerClient(view);
                bPlatformCreate = view.ReadByte(14);
                bPlatformAccess = view.ReadByte(15);
                dwReserved1 = view.ReadInt32(16);
                dwReserved2 = view.ReadInt32(20);
                if (IsUnicode)
                    UnicodeHeader = new UnicodeHeader(view, 24);
                if (IsAnsi)
                    AnsiHeader = new AnsiHeader();
            }
        }
        private void CheckdwMagic(MemoryMappedViewAccessor view)
        {
            byte[] dwMagic = { 0x21, 0x42, 0x44, 0x4E };//!BDN
            int offset = 0;
            this.dwMagic = view.ReadInt32(offset);
            if (BitConverter.ToInt32(dwMagic, 0) == this.dwMagic)
                return;
            throw new Exception("Not a valid file");
        }
        private void CheckwMagicClient(MemoryMappedViewAccessor view)
        {
            int offset = 8;
            byte[] pst = { 0x53, 0x4d };//PST FIle
            byte[] pab = { 0x41, 0x42 };//PAB FIle
            byte[] ost = { 0x53, 0x4f };//OST FIle
            short pstFile = BitConverter.ToInt16(pst);//SM
            short pabFile = BitConverter.ToInt16(pab);//AB
            short ostFile = BitConverter.ToInt16(ost);//SO
            wMagicClient = view.ReadInt16(offset);
            if (pstFile == wMagicClient)
                return;

            throw new Exception("PST File only supported");
        }
        private void CheckwVer(MemoryMappedViewAccessor view)
        {
            int offset = 10;
            wVer = view.ReadInt16(offset);
            if (wVer == 14 || wVer == 15)
            {
                IsAnsi = true;
                FileFormat = "ANSI";
                return;
            }
            if (wVer >= 23 && wVer <= 36)
            {
                IsUnicode = true;
                FileFormat = "UNICODE";
                return;
            }
            throw new Exception("Invaid Encoding Type");
        }
        private void CheckwVerClient(MemoryMappedViewAccessor view)
        {
            int offset = 12;
            int wVerClientValue = 19;
            wVerClient = view.ReadInt16(offset);
            if (wVerClientValue == wVerClient)
                return;
            throw new Exception("Invalid wVerClient.");
        }
    }
}