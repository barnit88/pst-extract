using System;
using System.IO.MemoryMappedFiles;

namespace core.NDBLayer.Headers.Unicode
{
    public class UnicodeHeader
    {
        public bool IsDataEncoded { get; set; } = false;
        public bCryptMethodType EncodingFriendlyName { get; set; }
        public string EncodingAlgorithm { get; set; }
        public UnicodeRoot Root { get; set; }
        long bidUnused { get; set; }
        byte[] bidNextP { get; set; } = new byte[8];
        int dwUnique { get; set; }//4 byte
        int[] rgnid { get; set; } = new int[32];//128 byte
        long qwUnused { get; set; }
        byte[] root { get; set; } = new byte[72];
        int dwAlign { get; set; }
        byte[] rgbFM { get; set; } = new byte[128];
        byte[] rgbFP { get; set; } = new byte[128];
        byte bSentinel { get; set; }
        byte bCryptMethod { get; set; }
        short rgbReserved { get; set; } // 2 byte
        long bidNextB { get; set; } // 8 bytes
        int dwCRCFull { get; set; }//CRC check 4 bytes
        byte[] rgbReserved2 { get; set; } = new byte[3];//3 bytes Readers Ignore
        byte bReserved { get; set; }//1 byte Readers Ignore
        byte[] rgbReserved3 { get; set; } = new byte[32];//32 bytes Readers Ignore

        public UnicodeHeader(MemoryMappedViewAccessor view, int offset)//offset = 24
        {
            int currentOffset = offset;
            bidUnused = view.ReadInt64(currentOffset);
            currentOffset += 8;
            view.ReadArray(currentOffset, bidNextP, 0, 8);
            currentOffset += 8;
            dwUnique = view.ReadInt32(currentOffset);
            currentOffset += 4;
            Readrgnid(view);
            currentOffset += 128;
            qwUnused = view.ReadInt64(currentOffset);
            currentOffset += 8;
            view.ReadArray(currentOffset, root, 0, 72);
            Root = new UnicodeRoot(view, currentOffset);
            currentOffset += 72;
            dwAlign = view.ReadInt32(currentOffset);
            currentOffset += 4;
            view.ReadArray(currentOffset, rgbFM, 0, 128);
            currentOffset += 128;
            view.ReadArray(currentOffset, rgbFP, 0, 128);
            currentOffset += 128;
            CheckbSentinel(view);
            currentOffset += 1;
            CheckbCryptMethod(view);
            currentOffset += 1;
            rgbReserved = view.ReadInt16(currentOffset);
            currentOffset += 2;
            bidNextB = view.ReadInt64(currentOffset);
            currentOffset += 8;
            dwCRCFull = view.ReadInt32(currentOffset);
            currentOffset += 4;
            view.ReadArray(currentOffset, rgbReserved2, 0, 3);
            currentOffset += 3;
            bReserved = view.ReadByte(currentOffset);
            currentOffset += 1;
            view.ReadArray(currentOffset, rgbReserved3, 0, 32);
            currentOffset += 32;
            if (currentOffset != 564)
                throw new Exception("Byte Length mismatch");



        }
        private void CheckbSentinel(MemoryMappedViewAccessor view)
        {
            byte bSentinelValue = 0x80;
            int offset = 512;
            bSentinel = view.ReadByte(offset);
            if (bSentinelValue == bSentinel)
                return;
            throw new Exception("Invalid bSentinel Value");
        }
        private void CheckbCryptMethod(MemoryMappedViewAccessor view)
        {
            int offset = 513;
            bCryptMethod = view.ReadByte(offset);
            if (bCryptMethod == (byte)bCryptMethodType.NDB_CRYPT_NONE)
            {
                EncodingFriendlyName = bCryptMethodType.NDB_CRYPT_NONE;
                EncodingAlgorithm = "Data Blocks are not encoded";
                return;
            }
            if (bCryptMethod == (byte)bCryptMethodType.NDB_CRYPT_PERMUTE)
            {
                IsDataEncoded = true;
                EncodingFriendlyName = bCryptMethodType.NDB_CRYPT_PERMUTE;
                EncodingAlgorithm = "Encoded with Permutation algorithm";
                return;
            }
            if (bCryptMethod == (byte)bCryptMethodType.NDB_CRYPT_CYCLIC)
            {
                IsDataEncoded = true;
                EncodingFriendlyName = bCryptMethodType.NDB_CRYPT_CYCLIC;
                EncodingAlgorithm = "Encoded with Cyclic algorithm ";
                return;
            }
            if (bCryptMethod == (byte)bCryptMethodType.NDB_CRYPT_EDPCRYPTED)
            {
                IsDataEncoded = true;
                EncodingFriendlyName = bCryptMethodType.NDB_CRYPT_EDPCRYPTED;
                EncodingAlgorithm = "Encrypted with Windows Information Protection";
                return;
            }
            throw new Exception("Invalid bCryptMethod");
        }
        private void Readrgnid(MemoryMappedViewAccessor view)
        {
            int position = 44;
            for (int i = 0; i < rgnid.Length; i++)
            {
                rgnid[i] = view.ReadInt32(position);
                position += 4;
            }

        }
    }
}