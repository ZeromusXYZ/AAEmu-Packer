﻿using System.Collections.Generic;

namespace AAPacker;

/// <summary>
/// Possible elements for Pak Header
/// </summary>
public enum AAPakFileHeaderElement
{
    /// <summary>
    /// Any byte value
    /// </summary>
    AnyByte,
    /// <summary>
    /// Byte must be zero
    /// </summary>
    NullByte,
    /// <summary>
    /// Header information
    /// </summary>
    Header,
    /// <summary>
    /// Number of normal files in the pak
    /// </summary>
    FilesCount,
    /// <summary>
    /// Number of "extra" files in the pak
    /// </summary>
    ExtraFilesCount
}

/// <summary>
/// Possible elements for File Meta data
/// </summary>
public enum AAPakFileInfoElement
{
    /// <summary>
    /// The filename
    /// </summary>
    FileName,
    /// <summary>
    /// Starting offset in the pak
    /// </summary>
    Offset,
    /// <summary>
    /// Actual file size
    /// </summary>
    Size,
    /// <summary>
    /// Duplicate of file size (possibly compressed size)?
    /// </summary>
    SizeDuplicate,
    /// <summary>
    /// Number of bytes used a padding
    /// </summary>
    PaddingSize,
    /// <summary>
    /// MD5 checksum value
    /// </summary>
    Md5,
    /// <summary>
    /// Unused1
    /// </summary>
    Dummy1,
    /// <summary>
    /// Last Modified Time
    /// </summary>
    ModifyTime,
    /// <summary>
    /// File created time
    /// </summary>
    CreateTime,
    /// <summary>
    /// Ununsed2
    /// </summary>
    Dummy2,
}

/// <summary>
/// Reader class defining how file meta data should be read and written for AAPak
/// </summary>
public class AAPakFileFormatReader
{
    /// <summary>
    /// Class containing structure on how to read/write the data
    /// </summary>
    /// <param name="initializeWithDefaults">If true, pre-fills the data</param>
    public AAPakFileFormatReader(bool initializeWithDefaults)
    {
        // Changed default constructor to fix issues related to arrays and json populating
        if (initializeWithDefaults)
        {
            ReaderName = "Default";
            IsDefault = true;
            HeaderEncryptionKey = XlGamesKey;
            HeaderBytes = new byte[] { 0x57, 0x49, 0x42, 0x4F };
            ReadOrder = new()
            {
                AAPakFileHeaderElement.Header,
                AAPakFileHeaderElement.NullByte,
                AAPakFileHeaderElement.NullByte,
                AAPakFileHeaderElement.NullByte,
                AAPakFileHeaderElement.NullByte,
                AAPakFileHeaderElement.FilesCount,
                AAPakFileHeaderElement.ExtraFilesCount,
                AAPakFileHeaderElement.NullByte,
                AAPakFileHeaderElement.NullByte,
                AAPakFileHeaderElement.NullByte,
                AAPakFileHeaderElement.NullByte
            };
            InvertFileCounter = false;
            FileInfoReadOrder = new()
            {
                AAPakFileInfoElement.FileName,
                AAPakFileInfoElement.Offset,
                AAPakFileInfoElement.Size,
                AAPakFileInfoElement.SizeDuplicate,
                AAPakFileInfoElement.PaddingSize,
                AAPakFileInfoElement.Md5,
                AAPakFileInfoElement.Dummy1,
                AAPakFileInfoElement.CreateTime,
                AAPakFileInfoElement.ModifyTime,
                AAPakFileInfoElement.Dummy2,
            };
        }
    }

    /// <summary>
    /// Default AES128 key used by XLGames for ArcheAge as encryption key for header and fileInfo data
    /// </summary>
    public static readonly byte[] XlGamesKey = { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F };

    /// <summary>
    /// Name of this reader
    /// </summary>
    public string ReaderName { get; set; } = "None";

    /// <summary>
    /// Marks if this Reader has been created with initializeWithDefaults enabled
    /// </summary>
    public bool IsDefault { get; private set; }

    /// <summary>
    /// Encryption Key to use for header data
    /// </summary>
    public byte[] HeaderEncryptionKey { get; set; }

    /// <summary>
    /// Header identification bytes (4)
    /// Default for XLGames is "WIBO" (0x57 0x49 0x42 0x4F)
    /// </summary>
    public byte[] HeaderBytes { get; set; }

    /// <summary>
    /// Read order of elements for the header
    /// </summary>
    public List<AAPakFileHeaderElement> ReadOrder { get; set; }

    /// <summary>
    /// Set to true if the FAT stores Extra Files before Normal Files
    /// </summary>
    public bool InvertFileCounter { get; set; }

    /// <summary>
    /// Read order for File Info in FAT entry
    /// </summary>
    public List<AAPakFileInfoElement> FileInfoReadOrder { get; set; }

    /// <summary>
    /// Default values to use for Dummy1 on new entries
    /// </summary>
    public uint DefaultDummy1 { get; set; }

    /// <summary>
    /// Default values to use for Dummy2 on new entries
    /// </summary>
    public uint DefaultDummy2 { get; set; }
}