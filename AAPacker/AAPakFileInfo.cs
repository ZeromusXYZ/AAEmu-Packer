using System.Runtime.InteropServices;

namespace AAPacker
{
    /// <summary>
    ///     File Details Block
    /// </summary>
    public class AAPakFileInfo
    {
        public long createTime;
        public int deletedIndexNumber = -1;
        public uint dummy1; // looks like padding, mostly 0 or 0x80000000 observed, possible file flags ?

        public ulong dummy2; // looks like padding to fill out the block, observed 0

        // The following are not part of the structure but used by the program
        public int entryIndexNumber = -1;
        public byte[] md5; // this should be 16 bytes
        public long modifyTime;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x108)]
        public string name;

        public long offset;

        public int
            paddingSize; // number of bytes of free space left until the next blocksize of 512 (or space until next file)

        public long size;
        public long sizeDuplicate; // maybe compressed data size ? if used, observed always same as size
    }
}