using System.Diagnostics;
using System.Security.Cryptography;
using SubStreamHelper;

namespace AAPacker
{
    /// <summary>
    ///     Pak Header Information
    /// </summary>
    public class AAPakFileHeader
    {
        private const int HeaderSize = 0x200;
        private const int FileInfoSize = 0x150;

        /// <summary>
        ///     Empty MD5 Hash to compare against
        /// </summary>
        public static readonly byte[] NullHash = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        ///     Empty MD5 Hash as a hex string to compare against
        /// </summary>
        public static string NullHashString = "".PadRight(32, '0');

        public static string LastAesError = string.Empty;

        /// <summary>
        ///     Default AES128 key used by XLGames for ArcheAge as encryption key for header and fileInfo data
        ///     32 1F 2A EE AA 58 4A B4 9A 6C 9E 09 D5 9E 9C 6F
        /// </summary>
        private readonly byte[] _xlGamesKey = 
            { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F };

        private readonly AAPak _owner;
        private long _addFileOffset;
        private byte[] _data = new byte[HeaderSize]; // decrypted header data

        /// <summary>
        ///     Number of unused "deleted" files inside this pak
        /// </summary>
        private uint _extraFileCount;

        /// <summary>
        ///     Memory stream that holds the encrypted file information + header part of the file
        /// </summary>
        public readonly MemoryStream FAT = new();

        /// <summary>
        ///     Number of used files inside this pak
        /// </summary>
        private uint _fileCount;

        public long FirstFileInfoOffset;
        public bool IsValid;

        /// <summary>
        ///     Current encryption key
        /// </summary>
        private byte[] _key;

        public byte[] RawData = new byte[HeaderSize]; // unencrypted header
        public const int Size = HeaderSize;

        /// <summary>
        ///     Creates a new Header Block for a Pak file
        /// </summary>
        /// <param name="owner">The AAPacker that this header belongs to</param>
        public AAPakFileHeader(AAPak owner)
        {
            _owner = owner;
            SetCustomKey(_xlGamesKey);
        }

        /// <summary>
        ///     If you want to use custom keys on your pak file, use this function to change the key that is used for
        ///     encryption/decryption of the FAT and header data
        /// </summary>
        /// <param name="newKey"></param>
        protected internal void SetCustomKey(byte[] newKey)
        {
            _key = new byte[newKey.Length];
            newKey.CopyTo(_key, 0);
        }

        /// <summary>
        ///     Reverts back to the original encryption key, this function is also automatically called when closing a file
        /// </summary>
        protected internal void SetDefaultKey()
        {
            _xlGamesKey.CopyTo(_key, 0);
        }

        /// <summary>
        ///     Encrypts or Decrypts a byte array using AES128 CBC -
        ///     SourceCode: https://stackoverflow.com/questions/44782910/aes128-decryption-in-c-sharp
        /// </summary>
        /// <param name="message">Byte array to process</param>
        /// <param name="key">Encryption key to use</param>
        /// <param name="doEncryption">False = Decrypt, True = Encrypt</param>
        /// <returns>Returns a new byte array containing the processed data</returns>
        public static byte[] EncryptAes(byte[] message, byte[] key, bool doEncryption)
        {
            try
            {
                var aes = Aes.Create();
                aes.Key = key;
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                var cipher = doEncryption ? aes.CreateEncryptor() : aes.CreateDecryptor();

                return cipher.TransformFinalBlock(message, 0, message.Length);
            }
            catch (Exception x)
            {
                LastAesError = x.Message;
                return null;
            }
        }

        /// <summary>
        /// Helper function for encrypting a stream
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="doEncryption"></param>
        /// <param name="leaveOpen"></param>
        /// <returns></returns>
        public static bool EncryptStreamAes(Stream source, Stream target, byte[] key, bool doEncryption, bool leaveOpen = false)
        {
            try
            {
                var aes = Aes.Create();
                aes.Key = key;
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                var cipher = doEncryption ? aes.CreateEncryptor() : aes.CreateDecryptor();

                // Create the streams used for encryption.
                var csEncrypt = new CryptoStream(target, cipher, CryptoStreamMode.Write);
                source.CopyTo(csEncrypt);
                if (!leaveOpen)
                    csEncrypt.Dispose();

                return true;
            }
            catch (Exception x)
            {
                LastAesError = x.Message;
                return false;
            }
        }

        /// <summary>
        ///     Locate and load the encrypted FAT data into memory
        /// </summary>
        /// <returns>Returns true on success</returns>
        public bool LoadRawFAT()
        {
            try
            {
                // Read all File Table Data into Memory
                FAT.SetLength(0);

                var totalFileInfoSize = (_fileCount + _extraFileCount) * FileInfoSize;
                _owner._gpFileStream.Seek(0, SeekOrigin.End);
                FirstFileInfoOffset = _owner._gpFileStream.Position;

                // Search for the first file location, it needs to be aligned to a 0x200 size block
                FirstFileInfoOffset -= HeaderSize;
                FirstFileInfoOffset -= totalFileInfoSize;
                var dif = FirstFileInfoOffset % 0x200;
                // Align to previous block of 512 bytes
                FirstFileInfoOffset -= dif;

                _owner._gpFileStream.Position = FirstFileInfoOffset;

                var fat = new SubStream(_owner._gpFileStream, FirstFileInfoOffset, _owner._gpFileStream.Length - FirstFileInfoOffset);
                fat.CopyTo(FAT);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Writes current files info back into FAT (encrypted)
        /// </summary>
        /// <returns>Returns true on success</returns>
        public bool WriteToFAT()
        {
            if (_owner.PakType == PakFileType.CSV)
                return false;

            // Read all File Table Data into Memory
            FAT.SetLength(0);

            const int bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));
            var ms = new MemoryStream(bufSize); // Could probably do without the intermediate memoryStream, but it's easier to process
            var writer = new BinaryWriter(ms);

            // Init File Counts
            var totalFileCount = _owner.Files.Count + _owner.ExtraFiles.Count;
            var filesToGo = _owner.Files.Count;
            var extrasToGo = _owner.ExtraFiles.Count;
            var fileIndex = 0;
            var extrasIndex = 0;
            for (var i = 0; i < totalFileCount; i++)
            {
                ms.Position = 0;
                AAPakFileInfo pfi;

                if (_owner.PakType is PakFileType.TypeA or PakFileType.TypeF)
                {
                    // TypeA has files first, extra files after that
                    if (filesToGo > 0)
                    {
                        filesToGo--;
                        pfi = _owner.Files[fileIndex];
                        fileIndex++;
                    }
                    else if (extrasToGo > 0)
                    {
                        extrasToGo--;
                        pfi = _owner.ExtraFiles[extrasIndex];
                        extrasIndex++;
                    }
                    else
                    {
                        // If we get here, your PC cannot math and something went wrong
                        break;
                    }
                }
                else if (_owner.PakType == PakFileType.TypeB)
                {
                    // TypeB has files first, extra files after that
                    if (extrasToGo > 0)
                    {
                        extrasToGo--;
                        pfi = _owner.ExtraFiles[extrasIndex];
                        extrasIndex++;
                    }
                    else if (filesToGo > 0)
                    {
                        filesToGo--;
                        pfi = _owner.Files[fileIndex];
                        fileIndex++;
                    }
                    else
                    {
                        // If we get here, your PC cannot math and something went wrong
                        break;
                    }
                }
                else
                {
                    // Unsupported Type somehow
                    throw new Exception("Don't know how to write this FAT: " + _owner.PakType);
                }

                if (_owner.PakType == PakFileType.Custom)
                {
                    // Manually write the string for filename
                    for (var c = 0; c < 0x108; c++)
                    {
                        byte ch = 0;
                        if (c < pfi.name.Length)
                            ch = (byte)pfi.name[c];
                        writer.Write(ch);
                    }

                    writer.Write(pfi.offset);
                    writer.Write(pfi.size);
                    writer.Write(pfi.sizeDuplicate);
                    writer.Write(pfi.paddingSize);
                    writer.Write(pfi.md5);
                    writer.Write(pfi.dummy1);
                    writer.Write(pfi.createTime);
                    writer.Write(pfi.modifyTime);
                    writer.Write(pfi.dummy2);
                }
                else
                if (_owner.PakType == PakFileType.TypeA)
                {
                    // Manually write the string for filename
                    for (var c = 0; c < 0x108; c++)
                    {
                        byte ch = 0;
                        if (c < pfi.name.Length)
                            ch = (byte)pfi.name[c];
                        writer.Write(ch);
                    }

                    writer.Write(pfi.offset);
                    writer.Write(pfi.size);
                    writer.Write(pfi.sizeDuplicate);
                    writer.Write(pfi.paddingSize);
                    writer.Write(pfi.md5);
                    writer.Write(pfi.dummy1);
                    writer.Write(pfi.createTime);
                    writer.Write(pfi.modifyTime);
                    writer.Write(pfi.dummy2);
                }
                else if (_owner.PakType == PakFileType.TypeB)
                {
                    writer.Write(pfi.paddingSize);
                    writer.Write(pfi.md5);
                    writer.Write(pfi.dummy1);
                    writer.Write(pfi.size);

                    // Manually write the string for filename
                    for (var c = 0; c < 0x108; c++)
                    {
                        byte ch = 0;
                        if (c < pfi.name.Length)
                            ch = (byte)pfi.name[c];
                        writer.Write(ch);
                    }

                    writer.Write(pfi.sizeDuplicate);
                    writer.Write(pfi.offset);
                    writer.Write(pfi.modifyTime);
                    writer.Write(pfi.createTime);
                    writer.Write(pfi.dummy2);
                }
                else if (_owner.PakType == PakFileType.TypeF)
                {
                    writer.Write(pfi.dummy2);
                    // Manually write the string for filename
                    for (var c = 0; c < 0x108; c++)
                    {
                        byte ch = 0;
                        if (c < pfi.name.Length)
                            ch = (byte)pfi.name[c];
                        writer.Write(ch);
                    }

                    writer.Write(pfi.offset);
                    writer.Write(pfi.size);
                    writer.Write(pfi.sizeDuplicate);
                    writer.Write(pfi.paddingSize);
                    writer.Write(pfi.md5);
                    writer.Write(pfi.dummy1);
                    writer.Write(pfi.createTime);
                    writer.Write(pfi.modifyTime); // For TypeF this is typically zero
                }
                else
                {
                    throw new Exception("I don't know how to write this file format: " + _owner.PakType);
                }

                // encrypt and write our new file into the FAT memory stream
                var decryptedFileData = new byte[bufSize];
                ms.Position = 0;
                ms.Read(decryptedFileData, 0, bufSize);
                var rawFileData = EncryptAes(decryptedFileData, _key, true); // encrypt header data
                FAT.Write(rawFileData, 0, bufSize);
            }

            ms.Dispose();

            // Calculate padding to header
            var dif = FAT.Length % 0x200;
            if (dif > 0)
            {
                var pad = 0x200 - dif;
                FAT.SetLength(FAT.Length + pad);
                FAT.Position = FAT.Length;
            }

            // Update header info
            _fileCount = (uint)_owner.Files.Count;
            _extraFileCount = (uint)_owner.ExtraFiles.Count;
            // Stretch size for header
            FAT.SetLength(FAT.Length + HeaderSize);
            // Encrypt the Header data
            EncryptHeaderData();
            // Write encrypted header
            FAT.Write(RawData, 0, 0x20);

            return true;
        }

        /// <summary>
        ///     Read and decrypt the File Details Table that was loaded into the FAT MemoryStream
        /// </summary>
        public void ReadFileTable()
        {
            // Check aa.bms QuickBMS file for reference
            FAT.Position = 0;

            var bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));
            var ms = new MemoryStream(bufSize); // Could probably do without the intermediate memoryStream, but it's easier to process
            var reader = new BinaryReader(ms);

            // Read the Files
            _owner.Files.Clear();
            _owner.ExtraFiles.Clear();
            var totalFileCount = _fileCount + _extraFileCount;
            var filesToGo = _fileCount;
            var extraToGo = _extraFileCount;
            var fileIndexCounter = -1;
            var deletedIndexCounter = -1;
            for (uint i = 0; i < totalFileCount; i++)
            {
                // Read and decrypt a fileInfo block
                var rawFileData = new byte[bufSize]; // decrypted header data
                FAT.Read(rawFileData, 0, bufSize);
                var decryptedFileData = EncryptAes(rawFileData, _key, false);

                // Read decrypted data into a AAPakFileInfo
                ms.SetLength(0);
                ms.Write(decryptedFileData, 0, bufSize);
                ms.Position = 0;
                var pfi = new AAPakFileInfo();
                if (_owner.PakType == PakFileType.Custom)
                {
                    // Manually read the string for filename
                    pfi.name = "";
                    for (var c = 0; c < 0x108; c++)
                    {
                        var ch = reader.ReadByte();
                        if (ch != 0)
                            pfi.name += (char)ch;
                        else
                            break;
                    }

                    ms.Position = 0x108;
                    pfi.offset = reader.ReadInt64();
                    pfi.size = reader.ReadInt64();
                    pfi.sizeDuplicate = reader.ReadInt64();
                    pfi.paddingSize = reader.ReadInt32();
                    pfi.md5 = reader.ReadBytes(16);
                    pfi.dummy1 = reader.ReadUInt32(); // observed 0x00000000
                    pfi.createTime = reader.ReadInt64();
                    pfi.modifyTime = reader.ReadInt64();
                    pfi.dummy2 = reader.ReadUInt64(); // unused ?
                }
                else
                if (_owner.PakType == PakFileType.TypeA)
                {
                    // Manually read the string for filename
                    pfi.name = "";
                    for (var c = 0; c < 0x108; c++)
                    {
                        var ch = reader.ReadByte();
                        if (ch != 0)
                            pfi.name += (char)ch;
                        else
                            break;
                    }

                    ms.Position = 0x108;
                    pfi.offset = reader.ReadInt64();
                    pfi.size = reader.ReadInt64();
                    pfi.sizeDuplicate = reader.ReadInt64();
                    pfi.paddingSize = reader.ReadInt32();
                    pfi.md5 = reader.ReadBytes(16);
                    pfi.dummy1 = reader.ReadUInt32(); // observed 0x00000000
                    pfi.createTime = reader.ReadInt64();
                    pfi.modifyTime = reader.ReadInt64();
                    pfi.dummy2 = reader.ReadUInt64(); // unused ?
                }
                else if (_owner.PakType == PakFileType.TypeB)
                {
                    pfi.paddingSize = reader.ReadInt32();
                    pfi.md5 = reader.ReadBytes(16);
                    pfi.dummy1 = reader.ReadUInt32(); // 0x80000000
                    pfi.size = reader.ReadInt64();
                    // Manually read the string for filename
                    pfi.name = "";
                    for (var c = 0; c < 0x108; c++)
                    {
                        var ch = reader.ReadByte();
                        if (ch != 0)
                            pfi.name += (char)ch;
                        else
                            break;
                    }

                    ms.Position = 0x128;
                    pfi.sizeDuplicate = reader.ReadInt64();
                    pfi.offset = reader.ReadInt64();
                    pfi.modifyTime = reader.ReadInt64();
                    pfi.createTime = reader.ReadInt64();
                    pfi.dummy2 = reader.ReadUInt64(); // unused ?
                }
                else if (_owner.PakType == PakFileType.TypeF)
                {
                    pfi.dummy2 = reader.ReadUInt64(); // unused ?
                    // Manually read the string for filename
                    pfi.name = "";
                    for (var c = 0; c < 0x108; c++)
                    {
                        var ch = reader.ReadByte();
                        if (ch != 0)
                            pfi.name += (char)ch;
                        else
                            break;
                    }

                    ms.Position = 0x110;

                    pfi.offset = reader.ReadInt64();
                    pfi.size = reader.ReadInt64();
                    pfi.sizeDuplicate = reader.ReadInt64();
                    pfi.paddingSize = reader.ReadInt32();
                    pfi.md5 = reader.ReadBytes(16);
                    pfi.dummy1 = reader.ReadUInt32(); // observed 0x00000000
                    pfi.createTime = reader.ReadInt64();
                    pfi.modifyTime = reader.ReadInt64(); // For TypeF this is typically zero
                }

                if (_owner.PakType is PakFileType.TypeA or PakFileType.TypeF or PakFileType.Custom)
                {
                    // TypeA has files first and extra files last
                    if (filesToGo > 0)
                    {
                        fileIndexCounter++;
                        pfi.entryIndexNumber = fileIndexCounter;

                        filesToGo--;
                        _owner.Files.Add(pfi);
                    }
                    else if (extraToGo > 0)
                    {
                        // "Extra" Files. It looks like these are old deleted files renamed to "__unused__"
                        // There might be more to these, but can't be sure at this moment, looks like they are 512 byte blocks on my pak files
                        deletedIndexCounter++;
                        pfi.deletedIndexNumber = deletedIndexCounter;

                        extraToGo--;
                        _owner.ExtraFiles.Add(pfi);
                    }
                }
                else if (_owner.PakType == PakFileType.TypeB)
                {
                    // TypeB has extra files first and normal files last
                    if (extraToGo > 0)
                    {
                        fileIndexCounter++;
                        pfi.entryIndexNumber = fileIndexCounter;

                        extraToGo--;
                        _owner.ExtraFiles.Add(pfi);
                    }
                    else if (filesToGo > 0)
                    {
                        deletedIndexCounter++;
                        pfi.deletedIndexNumber = deletedIndexCounter;

                        filesToGo--;
                        _owner.Files.Add(pfi);
                    }
                }

                // determine the newest date, use both creating and modify date for this
                try
                {
                    var fTime = DateTime.FromFileTime(pfi.createTime);
                    if (fTime > _owner.NewestFileDate)
                        _owner.NewestFileDate = fTime;
                }
                catch
                {
                    // Just ignore this
                }

                try
                {
                    var fTime = DateTime.FromFileTime(pfi.modifyTime);
                    if (fTime > _owner.NewestFileDate)
                        _owner.NewestFileDate = fTime;
                }
                catch
                {
                    // Just ignore this
                }

                // Update our "end of file data" location if needed
                if (pfi.offset + pfi.size + pfi.paddingSize > _addFileOffset)
                    _addFileOffset = pfi.offset + pfi.size + pfi.paddingSize;
            }

            ms.Dispose();
        }

        /// <summary>
        ///     Helper function for debugging, write byte array as a hex text file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        private static void ByteArrayToHexFile(byte[] bytes, string fileName)
        {
            var s = "";
            for (var i = 0; i < bytes.Length; i++)
            {
                s += bytes[i].ToString("X2") + " ";
                if (i % 16 == 15)
                {
                    s += "\r\n";
                }
                else
                {
                    if (i % 4 == 3)
                        s += " ";
                    if (i % 8 == 7)
                        s += " ";
                }
            }

            File.WriteAllText(fileName, s);
        }

        /// <summary>
        ///     Helper function for debugging, write byte array as a hex text file
        /// </summary>
        /// <param name="bytes">Input byte array</param>
        /// <param name="spacingText">String to use for spacing between bytes</param>
        /// <param name="lineFeed">String to use as newline every 16 bytes</param>
        public static string ByteArrayToHexString(byte[] bytes, string spacingText = " ", string lineFeed = "\r\n")
        {
            var s = "";
            for (var i = 0; i < bytes.Length; i++)
            {
                s += bytes[i].ToString("X2") + spacingText;
                if (i % 16 == 15)
                {
                    s += lineFeed;
                }
                else
                {
                    if (i % 4 == 3)
                        s += spacingText;
                    if (i % 8 == 7)
                        s += spacingText;
                }
            }

            return s;
        }

        private bool ValidateHeaderWithReader(AAPakFileFormatReader reader, byte[] raw)
        {
            _data = EncryptAes(RawData, reader.HeaderEncryptionKey, false);
            var cursor = 0;
            var fCount = 0u;
            var eCount = 0u;

            byte ReadByte()
            {
                var res = _data[cursor];
                cursor++;
                return res;
            }

            uint ReadUInt32()
            {
                var res = BitConverter.ToUInt32(_data, cursor);
                cursor += 4;
                return res;
            }
            
            foreach (var element in reader.ReadOrder)
            {
                // Check if still inside valid range
                if (cursor >= _data.Length)
                    return false;
                
                switch (element)
                {
                    case AAPakFileHeaderElement.AnyByte: 
                        _ = ReadByte();
                        break;
                    case AAPakFileHeaderElement.NullByte: 
                        var zero = ReadByte();
                        if (zero != 0)
                            return false; // Expected value is not 0x00
                        break;
                    case AAPakFileHeaderElement.Header:
                        for (var i = 0; i < reader.HeaderBytes.Length; i++)
                        {
                            var b = ReadByte();
                            if (b != reader.HeaderBytes[i]) 
                                return false; // invalid header string data
                        }
                        break;
                    case AAPakFileHeaderElement.FilesCount:
                        fCount = ReadUInt32();
                        break;
                    case AAPakFileHeaderElement.ExtraFilesCount:
                        eCount = ReadUInt32();
                        break;
                }
            }

            _owner.PakType = PakFileType.Custom;
            _key = reader.HeaderEncryptionKey;
            _fileCount = fCount;
            _extraFileCount = eCount;
            return true;
        }

        /// <summary>
        ///     Decrypt the current header data to get the file counts
        /// </summary>
        public void DecryptHeaderData()
        {
            if (_owner.Reader != null)
            {
                IsValid = ValidateHeaderWithReader(_owner.Reader, RawData);
                if (IsValid)
                    return;
            }
            
            // custom reader didn't work, try the default styles
            _data = EncryptAes(RawData, _key, false);

            // A valid header/footer is check by it's identifier
            if (_data[0] == 'W' && _data[1] == 'I' && _data[2] == 'B' && _data[3] == 'O')
            {
                // W I B O = 0x57 0x49 0x42 0x4F
                _owner.PakType = PakFileType.TypeA;
                _fileCount = BitConverter.ToUInt32(_data, 8);
                _extraFileCount = BitConverter.ToUInt32(_data, 12);
                IsValid = true;
            }
            else if (_data[8] == 'I' && _data[9] == 'D' && _data[10] == 'E' && _data[11] == 'J')
            {
                // I D E J = 0x49 0x44 0x45 0x4A
                _owner.PakType = PakFileType.TypeB;
                _fileCount = BitConverter.ToUInt32(_data, 12);
                _extraFileCount = BitConverter.ToUInt32(_data, 0);
                IsValid = true;
            }
            else if (_data[0] == 'Z' && _data[1] == 'E' && _data[2] == 'R' && _data[3] == 'O')
            {
                // Z E R O = 0x5A 0x45 0x52 0x4F
                _owner.PakType = PakFileType.TypeF;
                _fileCount = BitConverter.ToUInt32(_data, 8);
                _extraFileCount = BitConverter.ToUInt32(_data, 12);
                IsValid = true;
            }
            else
            {
                // Doesn't look like this is a pak file, the file is corrupted, or is in a unknown layout/format
                _fileCount = 0;
                _extraFileCount = 0;
                IsValid = false;

                if (_owner.DebugMode)
                {
                    var hex = ByteArrayToHexString(_key, "", "");
                    File.WriteAllBytes("game_pak_failed_header_" + hex + ".key", _data);
                }
            }
        }

        /// <summary>
        ///     Encrypt the current header data
        /// </summary>
        private void EncryptHeaderData()
        {
            var ms = new MemoryStream();
            ms.Write(_data, 0, HeaderSize);
            ms.Position = 0;
            var writer = new BinaryWriter(ms);

            if (_owner.PakType == PakFileType.TypeA)
            {
                writer.Write((byte)'W');
                writer.Write((byte)'I');
                writer.Write((byte)'B');
                writer.Write((byte)'O');
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write(_fileCount);
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(_extraFileCount);
            }
            else if (_owner.PakType == PakFileType.TypeB)
            {
                writer.Write(_extraFileCount);
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write((byte)'I');
                writer.Write((byte)'D');
                writer.Write((byte)'E');
                writer.Write((byte)'J');
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(_fileCount);
            }
            else if (_owner.PakType == PakFileType.TypeF)
            {
                writer.Write((byte)'Z');
                writer.Write((byte)'E');
                writer.Write((byte)'R');
                writer.Write((byte)'O');
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write(_fileCount);
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(_extraFileCount);
            }

            ms.Position = 0;
            ms.Read(_data, 0, HeaderSize);
            ms.Dispose();
            // Encrypted our stored data into rawData
            RawData = EncryptAes(_data, _key, true);
        }
    }
}