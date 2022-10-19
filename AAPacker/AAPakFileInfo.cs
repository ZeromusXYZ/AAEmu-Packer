using System.Runtime.InteropServices;

namespace AAPacker;

/// <summary>
/// File Details Block
/// </summary>
public class AAPakFileInfo
{
    /// <summary>
    /// Original file creation time
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// Index of this deleted file
    /// </summary>
    public int DeletedIndexNumber { get; set; } = -1;

    /// <summary>
    /// Unknown value 1, mostly 0 or 0x80000000 observed, possible file flags ?
    /// </summary>
    public uint Dummy1 { get; set; }

    /// <summary>
    /// Unknown value 2, observed to be 0, seems to be unused
    /// </summary>
    public ulong Dummy2 { get; set; }

    /// <summary>
    /// Index in the normal files list
    /// </summary>
    public int EntryIndexNumber { get; set; } = -1;

    /// <summary>
    /// MD5 Hash byte array (should be 16 bytes)
    /// </summary>
    public byte[] Md5 { get; set; }

    /// <summary>
    /// Original file modified time
    /// </summary>
    public long ModifyTime { get; set; }

    /// <summary>
    /// Filename inside of the pakFile
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Offset in bytes of the starting location inside the pakFile
    /// </summary>
    public long Offset { get; set; }

    /// <summary>
    /// Number of bytes of free space left until the next blockSize of 512 (or space until next file)
    /// </summary>
    public int PaddingSize { get; set; }

    /// <summary>
    /// Original fileSize
    /// </summary>
    public long Size { get; set; }
    /// <summary>
    /// Duplicate of the original fileSize? Possibly file after decompression?
    /// Always observed as being the same as fileSize
    /// </summary>
    public long SizeDuplicate { get; set; }
}