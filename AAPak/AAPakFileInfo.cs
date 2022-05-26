using System;
using System.Runtime.InteropServices;

namespace AAPakEditor
{
    /// <summary>
    /// File Details Block
    /// </summary>
    public class AAPakFileInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x108)] public string name;
        public Int64 offset;
        public Int64 size;
        public Int64 sizeDuplicate; // maybe compressed data size ? if used, observed always same as size
        public Int32 paddingSize; // number of bytes of free space left until the next blocksize of 512 (or space until next file)
        public byte[] md5; // this should be 16 bytes
        public UInt32 dummy1; // looks like padding, mostly 0 or 0x80000000 observed, possible file flags ?
        public Int64 createTime;
        public Int64 modifyTime;
        public UInt64 dummy2; // looks like padding to fill out the block, observed 0
        // The following are not part of the structure but used by the program
        public int entryIndexNumber = -1;
        public int deletedIndexNumber = -1;
    }
}