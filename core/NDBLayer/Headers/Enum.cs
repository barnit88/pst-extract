namespace core.NDBLayer.Headers
{
    public enum HeaderType
    {
        Unicode = 0,
        Ansi = 1
    }
    public enum bCryptMethodType : byte
    {
        NDB_CRYPT_NONE = 0x00, //Data Blocks are not encoded
        NDB_CRYPT_PERMUTE = 0x01, //Encoded with Permutation algorithm
        NDB_CRYPT_CYCLIC = 0x02, //Encoded with Cyclic algorithm 
        NDB_CRYPT_EDPCRYPTED = 0x10 //Encrypted with Windows Information Protection

    }
}