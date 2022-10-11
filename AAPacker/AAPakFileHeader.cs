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
        public static byte[] NullHash { get; } = new byte[16];

        /// <summary>
        ///     Empty MD5 Hash as a hex string to compare against
        /// </summary>
        public static string NullHashString { get; } = "".PadRight(32, '0');

        /// <summary>
        ///     Exception error message from the last thrown encoding/decoding error
        /// </summary>
        public static string LastAesError { get; set; } = string.Empty;

        /// <summary>
        ///     Default AES128 key used by XLGames for ArcheAge as encryption key for header and fileInfo data
        ///     32 1F 2A EE AA 58 4A B4 9A 6C 9E 09 D5 9E 9C 6F
        /// </summary>
        private byte[] XlGamesKey { get; }= 
            { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F };

        /// <summary>
        /// Reference to owning pakFile object
        /// </summary>
        private AAPak Owner { get; }

        /// <summary>
        /// Offset in pakFile where to start adding new files
        /// </summary>
        private long AddFileOffset { get; set; }

        /// <summary>
        /// Decrypted Header data
        /// </summary>
        private byte[] Data { get; set; } = new byte[HeaderSize];

        /// <summary>
        ///     Number of unused "deleted" files inside this pak
        /// </summary>
        private uint ExtraFileCount { get; set; }

        /// <summary>
        ///     Memory stream that holds the encrypted file information + header part of the file
        /// </summary>
        public MemoryStream FAT { get; } = new();

        /// <summary>
        ///     Number of used files inside this pak
        /// </summary>
        private uint FileCount { get; set; }

        /// <summary>
        /// Offset in pakFile where the meta data of the first file in the list is stored
        /// </summary>
        public long FirstFileInfoOffset { get; set; }

        /// <summary>
        /// Is this header valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        ///     Current encryption key
        /// </summary>
        private byte[] Key { get; set; }

        /// <summary>
        ///     Unencrypted header
        /// </summary>
        public byte[] RawData { get; set; } = new byte[HeaderSize];

        /// <summary>
        ///     Header Size
        /// </summary>
        public static int Size { get; } = HeaderSize;

        /// <summary>
        ///     Creates a new Header Block for a Pak file
        /// </summary>
        /// <param name="owner">The AAPacker that this header belongs to</param>
        public AAPakFileHeader(AAPak owner)
        {
            Owner = owner;
            SetCustomKey(XlGamesKey);
        }

        /// <summary>
        ///     If you want to use custom keys on your pak file, use this function to change the key that is used for
        ///     encryption/decryption of the FAT and header data
        /// </summary>
        /// <param name="newKey"></param>
        protected internal void SetCustomKey(byte[] newKey)
        {
            Key = new byte[newKey.Length];
            newKey.CopyTo(Key, 0);
        }

        /// <summary>
        ///     Reverts back to the original encryption key, this function is also automatically called when closing a file
        /// </summary>
        protected internal void SetDefaultKey()
        {
            XlGamesKey.CopyTo(Key, 0);
        }

        // SourceCode: https://stackoverflow.com/questions/44782910/aes128-decryption-in-c-sharp
        /// <summary>
        ///     Encrypts or Decrypts a byte array using AES128 CBC     
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
        ///     Helper function for encrypting a stream
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

                var totalFileInfoSize = (FileCount + ExtraFileCount) * FileInfoSize;
                Owner.GpFileStream.Seek(0, SeekOrigin.End);
                FirstFileInfoOffset = Owner.GpFileStream.Position;

                // Search for the first file location, it needs to be aligned to a 0x200 Size block
                FirstFileInfoOffset -= HeaderSize;
                FirstFileInfoOffset -= totalFileInfoSize;
                var dif = FirstFileInfoOffset % 0x200;
                // Align to previous block of 512 bytes
                FirstFileInfoOffset -= dif;

                Owner.GpFileStream.Position = FirstFileInfoOffset;

                var fat = new SubStream(Owner.GpFileStream, FirstFileInfoOffset, Owner.GpFileStream.Length - FirstFileInfoOffset);
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
            if (Owner.PakType == PakFileType.Csv)
                return false;

            // Read all File Table Data into Memory
            FAT.SetLength(0);

            const int bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));
            var ms = new MemoryStream(bufSize); // Could probably do without the intermediate memoryStream, but it's easier to process
            var writer = new BinaryWriter(ms);

            // Init File Counts
            var totalFileCount = Owner.Files.Count + Owner.ExtraFiles.Count;
            var filesToGo = Owner.Files.Count;
            var extrasToGo = Owner.ExtraFiles.Count;
            var fileIndex = 0;
            var extrasIndex = 0;
            for (var i = 0; i < totalFileCount; i++)
            {
                ms.Position = 0;
                AAPakFileInfo pfi;

                if (Owner.PakType is PakFileType.TypeA or PakFileType.TypeF)
                {
                    // TypeA has files first, extra files after that
                    if (filesToGo > 0)
                    {
                        filesToGo--;
                        pfi = Owner.Files[fileIndex];
                        fileIndex++;
                    }
                    else if (extrasToGo > 0)
                    {
                        extrasToGo--;
                        pfi = Owner.ExtraFiles[extrasIndex];
                        extrasIndex++;
                    }
                    else
                    {
                        // If we get here, your PC cannot math and something went wrong
                        break;
                    }
                }
                else if (Owner.PakType == PakFileType.TypeB)
                {
                    // TypeB has files first, extra files after that
                    if (extrasToGo > 0)
                    {
                        extrasToGo--;
                        pfi = Owner.ExtraFiles[extrasIndex];
                        extrasIndex++;
                    }
                    else if (filesToGo > 0)
                    {
                        filesToGo--;
                        pfi = Owner.Files[fileIndex];
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
                    throw new Exception("Don't know how to write this FAT: " + Owner.PakType);
                }

                if (Owner.PakType == PakFileType.Custom)
                {
                    // Manually write the string for filename
                    for (var c = 0; c < 0x108; c++)
                    {
                        byte ch = 0;
                        if (c < pfi.Name.Length)
                            ch = (byte)pfi.Name[c];
                        writer.Write(ch);
                    }

                    writer.Write(pfi.Offset);
                    writer.Write(pfi.Size);
                    writer.Write(pfi.SizeDuplicate);
                    writer.Write(pfi.PaddingSize);
                    writer.Write(pfi.Md5);
                    writer.Write(pfi.Dummy1);
                    writer.Write(pfi.CreateTime);
                    writer.Write(pfi.ModifyTime);
                    writer.Write(pfi.Dummy2);
                }
                else
                if (Owner.PakType == PakFileType.TypeA)
                {
                    // Manually write the string for filename
                    for (var c = 0; c < 0x108; c++)
                    {
                        byte ch = 0;
                        if (c < pfi.Name.Length)
                            ch = (byte)pfi.Name[c];
                        writer.Write(ch);
                    }

                    writer.Write(pfi.Offset);
                    writer.Write(pfi.Size);
                    writer.Write(pfi.SizeDuplicate);
                    writer.Write(pfi.PaddingSize);
                    writer.Write(pfi.Md5);
                    writer.Write(pfi.Dummy1);
                    writer.Write(pfi.CreateTime);
                    writer.Write(pfi.ModifyTime);
                    writer.Write(pfi.Dummy2);
                }
                else if (Owner.PakType == PakFileType.TypeB)
                {
                    writer.Write(pfi.PaddingSize);
                    writer.Write(pfi.Md5);
                    writer.Write(pfi.Dummy1);
                    writer.Write(pfi.Size);

                    // Manually write the string for filename
                    for (var c = 0; c < 0x108; c++)
                    {
                        byte ch = 0;
                        if (c < pfi.Name.Length)
                            ch = (byte)pfi.Name[c];
                        writer.Write(ch);
                    }

                    writer.Write(pfi.SizeDuplicate);
                    writer.Write(pfi.Offset);
                    writer.Write(pfi.ModifyTime);
                    writer.Write(pfi.CreateTime);
                    writer.Write(pfi.Dummy2);
                }
                else if (Owner.PakType == PakFileType.TypeF)
                {
                    writer.Write(pfi.Dummy2);
                    // Manually write the string for filename
                    for (var c = 0; c < 0x108; c++)
                    {
                        byte ch = 0;
                        if (c < pfi.Name.Length)
                            ch = (byte)pfi.Name[c];
                        writer.Write(ch);
                    }

                    writer.Write(pfi.Offset);
                    writer.Write(pfi.Size);
                    writer.Write(pfi.SizeDuplicate);
                    writer.Write(pfi.PaddingSize);
                    writer.Write(pfi.Md5);
                    writer.Write(pfi.Dummy1);
                    writer.Write(pfi.CreateTime);
                    writer.Write(pfi.ModifyTime); // For TypeF this is typically zero
                }
                else
                {
                    throw new Exception("I don't know how to write this file format: " + Owner.PakType);
                }

                // encrypt and write our new file into the FAT memory stream
                var decryptedFileData = new byte[bufSize];
                ms.Position = 0;
                _ = ms.Read(decryptedFileData, 0, bufSize);
                var rawFileData = EncryptAes(decryptedFileData, Key, true); // encrypt header data
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
            FileCount = (uint)Owner.Files.Count;
            ExtraFileCount = (uint)Owner.ExtraFiles.Count;
            // Stretch Size for header
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
            const int bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));

            // Check aa.bms QuickBMS file for reference
            FAT.Position = 0;
            var ms = new MemoryStream(bufSize); // Could probably do without the intermediate memoryStream, but it's easier to process
            var reader = new BinaryReader(ms);

            // Read the Files
            Owner.Files.Clear();
            Owner.ExtraFiles.Clear();
            var totalFileCount = FileCount + ExtraFileCount;
            var filesToGo = FileCount;
            var extraToGo = ExtraFileCount;
            var fileIndexCounter = -1;
            var deletedIndexCounter = -1;
            for (uint i = 0; i < totalFileCount; i++)
            {
                // Read and decrypt a fileInfo block
                var rawFileData = new byte[bufSize]; // decrypted header data
                _ = FAT.Read(rawFileData, 0, bufSize);
                var decryptedFileData = EncryptAes(rawFileData, Key, false);

                // Read decrypted data into a AAPakFileInfo
                ms.SetLength(0);
                ms.Write(decryptedFileData, 0, bufSize);
                ms.Position = 0;
                var pfi = new AAPakFileInfo();
                if (Owner.PakType == PakFileType.Custom)
                {
                    // Manually read the string for filename
                    pfi.Name = "";
                    for (var c = 0; c < 0x108; c++)
                    {
                        var ch = reader.ReadByte();
                        if (ch != 0)
                            pfi.Name += (char)ch;
                        else
                            break;
                    }

                    ms.Position = 0x108;
                    pfi.Offset = reader.ReadInt64();
                    pfi.Size = reader.ReadInt64();
                    pfi.SizeDuplicate = reader.ReadInt64();
                    pfi.PaddingSize = reader.ReadInt32();
                    pfi.Md5 = reader.ReadBytes(16);
                    pfi.Dummy1 = reader.ReadUInt32(); // observed 0x00000000
                    pfi.CreateTime = reader.ReadInt64();
                    pfi.ModifyTime = reader.ReadInt64();
                    pfi.Dummy2 = reader.ReadUInt64(); // unused ?
                }
                else
                if (Owner.PakType == PakFileType.TypeA)
                {
                    // Manually read the string for filename
                    pfi.Name = "";
                    for (var c = 0; c < 0x108; c++)
                    {
                        var ch = reader.ReadByte();
                        if (ch != 0)
                            pfi.Name += (char)ch;
                        else
                            break;
                    }

                    ms.Position = 0x108;
                    pfi.Offset = reader.ReadInt64();
                    pfi.Size = reader.ReadInt64();
                    pfi.SizeDuplicate = reader.ReadInt64();
                    pfi.PaddingSize = reader.ReadInt32();
                    pfi.Md5 = reader.ReadBytes(16);
                    pfi.Dummy1 = reader.ReadUInt32(); // observed 0x00000000
                    pfi.CreateTime = reader.ReadInt64();
                    pfi.ModifyTime = reader.ReadInt64();
                    pfi.Dummy2 = reader.ReadUInt64(); // unused ?
                }
                else if (Owner.PakType == PakFileType.TypeB)
                {
                    pfi.PaddingSize = reader.ReadInt32();
                    pfi.Md5 = reader.ReadBytes(16);
                    pfi.Dummy1 = reader.ReadUInt32(); // 0x80000000
                    pfi.Size = reader.ReadInt64();
                    // Manually read the string for filename
                    pfi.Name = "";
                    for (var c = 0; c < 0x108; c++)
                    {
                        var ch = reader.ReadByte();
                        if (ch != 0)
                            pfi.Name += (char)ch;
                        else
                            break;
                    }

                    ms.Position = 0x128;
                    pfi.SizeDuplicate = reader.ReadInt64();
                    pfi.Offset = reader.ReadInt64();
                    pfi.ModifyTime = reader.ReadInt64();
                    pfi.CreateTime = reader.ReadInt64();
                    pfi.Dummy2 = reader.ReadUInt64(); // unused ?
                }
                else if (Owner.PakType == PakFileType.TypeF)
                {
                    pfi.Dummy2 = reader.ReadUInt64(); // unused ?
                    // Manually read the string for filename
                    pfi.Name = "";
                    for (var c = 0; c < 0x108; c++)
                    {
                        var ch = reader.ReadByte();
                        if (ch != 0)
                            pfi.Name += (char)ch;
                        else
                            break;
                    }

                    ms.Position = 0x110;

                    pfi.Offset = reader.ReadInt64();
                    pfi.Size = reader.ReadInt64();
                    pfi.SizeDuplicate = reader.ReadInt64();
                    pfi.PaddingSize = reader.ReadInt32();
                    pfi.Md5 = reader.ReadBytes(16);
                    pfi.Dummy1 = reader.ReadUInt32(); // observed 0x00000000
                    pfi.CreateTime = reader.ReadInt64();
                    pfi.ModifyTime = reader.ReadInt64(); // For TypeF this is typically zero
                }

                if (Owner.PakType is PakFileType.TypeA or PakFileType.TypeF or PakFileType.Custom)
                {
                    // TypeA has files first and extra files last
                    if (filesToGo > 0)
                    {
                        fileIndexCounter++;
                        pfi.EntryIndexNumber = fileIndexCounter;

                        filesToGo--;
                        Owner.Files.Add(pfi);
                    }
                    else if (extraToGo > 0)
                    {
                        // "Extra" Files. It looks like these are old deleted files renamed to "__unused__"
                        // There might be more to these, but can't be sure at this moment, looks like they are 512 byte blocks on my pak files
                        deletedIndexCounter++;
                        pfi.DeletedIndexNumber = deletedIndexCounter;

                        extraToGo--;
                        Owner.ExtraFiles.Add(pfi);
                    }
                }
                else if (Owner.PakType == PakFileType.TypeB)
                {
                    // TypeB has extra files first and normal files last
                    if (extraToGo > 0)
                    {
                        fileIndexCounter++;
                        pfi.EntryIndexNumber = fileIndexCounter;

                        extraToGo--;
                        Owner.ExtraFiles.Add(pfi);
                    }
                    else if (filesToGo > 0)
                    {
                        deletedIndexCounter++;
                        pfi.DeletedIndexNumber = deletedIndexCounter;

                        filesToGo--;
                        Owner.Files.Add(pfi);
                    }
                }

                // determine the newest date, use both creating and modify date for this
                try
                {
                    var fTime = DateTime.FromFileTime(pfi.CreateTime);
                    if (fTime > Owner.NewestFileDate)
                        Owner.NewestFileDate = fTime;
                }
                catch
                {
                    // Just ignore this
                }

                try
                {
                    var fTime = DateTime.FromFileTime(pfi.ModifyTime);
                    if (fTime > Owner.NewestFileDate)
                        Owner.NewestFileDate = fTime;
                }
                catch
                {
                    // Just ignore this
                }

                // Update our "end of file data" location if needed
                if (pfi.Offset + pfi.Size + pfi.PaddingSize > AddFileOffset)
                    AddFileOffset = pfi.Offset + pfi.Size + pfi.PaddingSize;
            }

            ms.Dispose();
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

        /// <summary>
        /// Check is a header is valid using a specific pakFile reader
        /// </summary>
        /// <param name="reader">pakFile reader object</param>
        /// <param name="raw">Raw header data to check</param>
        /// <returns></returns>
        private bool ValidateHeaderWithReader(AAPakFileFormatReader reader, byte[] raw)
        {
            Data = EncryptAes(RawData, reader.HeaderEncryptionKey, false);
            var cursor = 0;
            var fCount = 0u;
            var eCount = 0u;

            byte ReadByte()
            {
                var res = Data[cursor];
                cursor++;
                return res;
            }

            uint ReadUInt32()
            {
                var res = BitConverter.ToUInt32(Data, cursor);
                cursor += 4;
                return res;
            }
            
            foreach (var element in reader.ReadOrder)
            {
                // Check if still inside valid range
                if (cursor >= Data.Length)
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

            Owner.PakType = PakFileType.Custom;
            Key = reader.HeaderEncryptionKey;
            FileCount = fCount;
            ExtraFileCount = eCount;
            return true;
        }

        /// <summary>
        ///     Decrypt the current header data to get the file counts depending on header type
        /// </summary>
        public void DecryptHeaderData()
        {
            if (Owner.Reader != null)
            {
                IsValid = ValidateHeaderWithReader(Owner.Reader, RawData);
                if (IsValid)
                    return;
            }
            
            // custom reader didn't work, try the default styles
            Data = EncryptAes(RawData, Key, false);

            // A valid header/footer is check by it's identifier
            if (Data[0] == 'W' && Data[1] == 'I' && Data[2] == 'B' && Data[3] == 'O')
            {
                // W I B O = 0x57 0x49 0x42 0x4F
                Owner.PakType = PakFileType.TypeA;
                FileCount = BitConverter.ToUInt32(Data, 8);
                ExtraFileCount = BitConverter.ToUInt32(Data, 12);
                IsValid = true;
            }
            else if (Data[8] == 'I' && Data[9] == 'D' && Data[10] == 'E' && Data[11] == 'J')
            {
                // I D E J = 0x49 0x44 0x45 0x4A
                Owner.PakType = PakFileType.TypeB;
                FileCount = BitConverter.ToUInt32(Data, 12);
                ExtraFileCount = BitConverter.ToUInt32(Data, 0);
                IsValid = true;
            }
            else if (Data[0] == 'Z' && Data[1] == 'E' && Data[2] == 'R' && Data[3] == 'O')
            {
                // Z E R O = 0x5A 0x45 0x52 0x4F
                Owner.PakType = PakFileType.TypeF;
                FileCount = BitConverter.ToUInt32(Data, 8);
                ExtraFileCount = BitConverter.ToUInt32(Data, 12);
                IsValid = true;
            }
            else
            {
                // Doesn't look like this is a pak file, the file is corrupted, or is in a unknown layout/format
                FileCount = 0;
                ExtraFileCount = 0;
                IsValid = false;

                if (Owner.DebugMode)
                {
                    var hex = ByteArrayToHexString(Key, "", "");
                    File.WriteAllBytes("game_pak_failed_header_" + hex + ".key", Data);
                }
            }
        }

        /// <summary>
        ///     Encrypt the current header data
        /// </summary>
        private void EncryptHeaderData()
        {
            var ms = new MemoryStream();
            ms.Write(Data, 0, HeaderSize);
            ms.Position = 0;
            var writer = new BinaryWriter(ms);

            if (Owner.PakType == PakFileType.TypeA)
            {
                writer.Write((byte)'W');
                writer.Write((byte)'I');
                writer.Write((byte)'B');
                writer.Write((byte)'O');
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write(FileCount);
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(ExtraFileCount);
            }
            else if (Owner.PakType == PakFileType.TypeB)
            {
                writer.Write(ExtraFileCount);
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write((byte)'I');
                writer.Write((byte)'D');
                writer.Write((byte)'E');
                writer.Write((byte)'J');
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(FileCount);
            }
            else if (Owner.PakType == PakFileType.TypeF)
            {
                writer.Write((byte)'Z');
                writer.Write((byte)'E');
                writer.Write((byte)'R');
                writer.Write((byte)'O');
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write(FileCount);
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(ExtraFileCount);
            }

            ms.Position = 0;
            ms.Read(Data, 0, HeaderSize);
            ms.Dispose();
            // Encrypted our stored data into rawData
            RawData = EncryptAes(Data, Key, true);
        }
    }
}