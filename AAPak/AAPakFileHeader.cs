using System;
using System.IO;
using System.Security.Cryptography;
using SubStreamHelper;

namespace AAPakEditor
{
    /// <summary>
    /// Pak Header Information
    /// </summary>
    public class AAPakFileHeader
    {
        /// <summary>
        /// Default AES128 key used by XLGames for ArcheAge as encryption key for header and fileinfo data
        /// 32 1F 2A EE AA 58 4A B4 9A 6C 9E 09 D5 9E 9C 6F
        /// </summary>
        private readonly byte[] XLGamesKey = new byte[] { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F };
        /// <summary>
        /// Current encryption key
        /// </summary>
        private byte[] key ;
        protected static readonly int headerSize = 0x200;
        protected static readonly int fileInfoSize = 0x150;
        /// <summary>
        /// Memory stream that holds the encrypted file information + header part of the file
        /// </summary>
        public MemoryStream FAT = new MemoryStream();

        public AAPak _owner;
        public int Size = headerSize;
        public long FirstFileInfoOffset = 0;
        public long AddFileOffset = 0;
        public byte[] rawData = new byte[headerSize]; // unencrypted header
        public byte[] data = new byte[headerSize]; // decrypted header data
        public bool isValid;
        /// <summary>
        /// Number of used files inside this pak
        /// </summary>
        public uint fileCount = 0;
        /// <summary>
        /// Number of unused "deleted" files inside this pak
        /// </summary>
        public uint extraFileCount = 0;

        /// <summary>
        /// Empty MD5 Hash to compare against
        /// </summary>
        public static byte[] nullHash = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// Empty MD5 Hash as a hex string to compare against
        /// </summary>
        public static string nullHashString = "".PadRight(32, '0');
        public static string LastAESError = string.Empty;

        /// <summary>
        /// Creates a new Header Block for a Pak file
        /// </summary>
        /// <param name="owner">The AAPak that this header belongs to</param>
        public AAPakFileHeader(AAPak owner)
        {
            _owner = owner;
            SetCustomKey(XLGamesKey);
        }

        ~AAPakFileHeader()
        {
            // FAT.Dispose();
        }

        /// <summary>
        /// If you want to use custom keys on your pak file, use this function to change the key that is used for encryption/decryption of the FAT and header data
        /// </summary>
        /// <param name="newKey"></param>
        public void SetCustomKey(byte[] newKey)
        {
            key = new byte[newKey.Length];
            newKey.CopyTo(key, 0);
        }

        /// <summary>
        /// Reverts back to the original encryption key, this function is also automatically called when closing a file
        /// </summary>
        public void SetDefaultKey()
        {
            XLGamesKey.CopyTo(key, 0);
        }

        /// <summary>
        /// Encrypts or Decrypts a byte array using AES128 CBC - 
        /// SourceCode: https://stackoverflow.com/questions/44782910/aes128-decryption-in-c-sharp
        /// </summary>
        /// <param name="message">Byte array to process</param>
        /// <param name="key">Encryption key to use</param>
        /// <param name="doEncryption">False = Decrypt, True = Encrypt</param>
        /// <returns>Returns a new byte array containing the processed data</returns>
        public static byte[] EncryptAES(byte[] message, byte[] key, bool doEncryption)
        {
            try
            {
                Aes aes = new AesManaged();
                aes.Key = key;
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cipher;

                if (doEncryption)
                    cipher = aes.CreateEncryptor();
                else
                    cipher = aes.CreateDecryptor();

                return cipher.TransformFinalBlock(message, 0, message.Length);
            }
            catch (Exception x)
            {
                LastAESError = x.Message;
                return null;
            }
        }

        public static bool EncryptStreamAES(Stream source, Stream target, byte[] key, bool doEncryption, bool leaveOpen = false)
        {
            try
            {
                Aes aes = new AesManaged();
                aes.Key = key;
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cipher;

                if (doEncryption)
                    cipher = aes.CreateEncryptor();
                else
                    cipher = aes.CreateDecryptor();

                // Create the streams used for encryption.

                CryptoStream csEncrypt = new CryptoStream(target, cipher, CryptoStreamMode.Write);
                source.CopyTo(csEncrypt);
                if (!leaveOpen)
                    csEncrypt.Dispose();

                /*
                using (CryptoStream csEncrypt = new CryptoStream(target, cipher, CryptoStreamMode.Write))
                {
                    source.CopyTo(csEncrypt);
                }
                */
                return true;
            }
            catch (Exception x)
            {
                LastAESError = x.Message;
                return false;
            }
        }

        public static bool EncryptStreamAESWithIV(Stream source, Stream target, byte[] key, byte[] customIV, bool doEncryption)
        {
            try
            {
                Aes aes = new AesManaged();
                aes.Key = key;
                aes.IV = customIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cipher;

                if (doEncryption)
                    cipher = aes.CreateEncryptor();
                else
                    cipher = aes.CreateDecryptor();

                // Create the streams used for encryption.
                using (CryptoStream csEncrypt = new CryptoStream(target, cipher, CryptoStreamMode.Write))
                {
                    source.CopyTo(csEncrypt);
                }
                return true;
            }
            catch (Exception x)
            {
                LastAESError = x.Message;
                return false;
            }
        }

        /// <summary>
        /// Same as the EncryptAES but specifying a specific IV
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="customIV"></param>
        /// <param name="doEncryption"></param>
        /// <returns></returns>
        public static byte[] EncryptAESUsingIV(byte[] message, byte[] key, byte[] customIV, bool doEncryption)
        {
            try
            {
                Aes aes = new AesManaged();
                aes.Key = key;
                aes.IV = customIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cipher;

                if (doEncryption == true)
                    cipher = aes.CreateEncryptor();
                else
                    cipher = aes.CreateDecryptor();

                return cipher.TransformFinalBlock(message, 0, message.Length);
            }
            catch (Exception x)
            {
                LastAESError = x.Message;
                return null;
            }
        }

        /// <summary>
        /// Locate and load the encrypted FAT data into memory
        /// </summary>
        /// <returns>Returns true on success</returns>
        public bool LoadRawFAT()
        {
            // Read all File Table Data into Memory
            FAT.SetLength(0);

            long TotalFileInfoSize = (fileCount + extraFileCount) * fileInfoSize;
            _owner._gpFileStream.Seek(0, SeekOrigin.End);
            FirstFileInfoOffset = _owner._gpFileStream.Position;

            // Search for the first file location, it needs to be alligned to a 0x200 size block
            FirstFileInfoOffset -= headerSize;
            FirstFileInfoOffset -= TotalFileInfoSize;
            var dif = FirstFileInfoOffset % 0x200;
            // Align to previous block of 512 bytes
            FirstFileInfoOffset -= dif;

            _owner._gpFileStream.Position = FirstFileInfoOffset;

            SubStream _fat = new SubStream(_owner._gpFileStream, FirstFileInfoOffset, _owner._gpFileStream.Length - FirstFileInfoOffset);
            _fat.CopyTo(FAT);

            return true;
        }

        /// <summary>
        /// Writes current files info back into FAT (encrypted)
        /// </summary>
        /// <returns>Returns true on success</returns>
        public bool WriteToFAT()
        {
            if (_owner.PakType == PakFileType.CSV)
                return false;

            // Read all File Table Data into Memory
            FAT.SetLength(0);

            int bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));
            MemoryStream ms = new MemoryStream(bufSize); // Could probably do without the intermediate memorystream, but it's easier to process
            BinaryWriter writer = new BinaryWriter(ms);

            // Init File Counts
            var totalFileCount = _owner.files.Count + _owner.extraFiles.Count;
            var filesToGo = _owner.files.Count;
            var extrasToGo = _owner.extraFiles.Count;
            int fileIndex = 0;
            int extrasIndex = 0;
            for (int i = 0; i < totalFileCount; i++)
            {
                ms.Position = 0;

                AAPakFileInfo pfi = null;

                if ((_owner.PakType == PakFileType.TypeA) || (_owner.PakType == PakFileType.TypeF))
                {
                    // TypeA has files first, extra files after that
                    if (filesToGo > 0)
                    {
                        filesToGo--;
                        pfi = _owner.files[fileIndex];
                        fileIndex++;
                    }
                    else
                    if (extrasToGo > 0)
                    {
                        extrasToGo--;
                        pfi = _owner.extraFiles[extrasIndex];
                        extrasIndex++;
                    }
                    else
                    {
                        // If we get here, your PC cannot math and something went wrong
                        pfi = null;
                        break;
                    }
                }
                else
                if (_owner.PakType == PakFileType.TypeB)
                {
                    // TypeB has files first, extra files after that
                    if (extrasToGo > 0)
                    {
                        extrasToGo--;
                        pfi = _owner.extraFiles[extrasIndex];
                        extrasIndex++;
                    }
                    else
                    if (filesToGo > 0)
                    {
                        filesToGo--;
                        pfi = _owner.files[fileIndex];
                        fileIndex++;
                    }
                    else
                    {
                        // If we get here, your PC cannot math and something went wrong
                        pfi = null;
                        break;
                    }
                }
                else
                {
                    // Unsupported Type somehow
                    throw new Exception("Don't know how to write this FAT: " + _owner.PakType);
                }

                if (_owner.PakType == PakFileType.TypeA)
                {
                    // Manually write the string for filename
                    for (int c = 0; c < 0x108; c++)
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
                if (_owner.PakType == PakFileType.TypeB)
                {
                    writer.Write(pfi.paddingSize);
                    writer.Write(pfi.md5);
                    writer.Write(pfi.dummy1);
                    writer.Write(pfi.size);

                    // Manually write the string for filename
                    for (int c = 0; c < 0x108; c++)
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
                else
                if (_owner.PakType == PakFileType.TypeF)
                {
                    writer.Write(pfi.dummy2);
                    // Manually write the string for filename
                    for (int c = 0; c < 0x108; c++)
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
                byte[] decryptedFileData = new byte[bufSize];
                ms.Position = 0;
                ms.Read(decryptedFileData, 0, bufSize);
                byte[] rawFileData = EncryptAES(decryptedFileData, key, true); // encrypt header data
                FAT.Write(rawFileData, 0, bufSize);
            }
            ms.Dispose();

            // Calculate padding to header
            var dif = (FAT.Length % 0x200);
            if (dif > 0)
            {
                var pad = (0x200 - dif);
                FAT.SetLength(FAT.Length + pad);
                FAT.Position = FAT.Length;
            }
            // Update header info
            fileCount = (uint)_owner.files.Count;
            extraFileCount = (uint)_owner.extraFiles.Count;
            // Stretch size for header
            FAT.SetLength(FAT.Length + headerSize);
            // Encrypt the Header data
            EncryptHeaderData();
            // Write encrypted header
            FAT.Write(rawData, 0, 0x20);

            return true;
        }

        /// <summary>
        /// Read and decrypt the File Details Table that was loaded into the FAT MemoryStream
        /// </summary>
        public void ReadFileTable()
        {
            // Check aa.bms QuickBMS file for reference
            FAT.Position = 0;

            int bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));
            MemoryStream ms = new MemoryStream(bufSize); // Could probably do without the intermediate memorystream, but it's easier to process
            BinaryReader reader = new BinaryReader(ms);
            
            // Read the Files
            _owner.files.Clear();
            _owner.extraFiles.Clear();
            var totalFileCount = fileCount + extraFileCount;
            var filesToGo = fileCount;
            var extraToGo = extraFileCount;
            var fileIndexCounter = -1;
            var deletedIndexCounter = -1;
            for (uint i = 0; i < totalFileCount; i++)
            {
                // Read and decrypt a fileinfo block
                byte[] rawFileData = new byte[bufSize]; // decrypted header data
                FAT.Read(rawFileData, 0, bufSize);
                byte[] decryptedFileData = EncryptAES(rawFileData, key, false);

                // Read decrypted data into a AAPakFileInfo
                ms.SetLength(0);
                ms.Write(decryptedFileData, 0, bufSize);
                ms.Position = 0;
                AAPakFileInfo pfi = new AAPakFileInfo();
                if (_owner.PakType == PakFileType.TypeA)
                {
                    // Manually read the string for filename
                    pfi.name = "";
                    for (int c = 0; c < 0x108; c++)
                    {
                        byte ch = reader.ReadByte();
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
                if (_owner.PakType == PakFileType.TypeB)
                {
                    pfi.paddingSize = reader.ReadInt32();
                    pfi.md5 = reader.ReadBytes(16);
                    pfi.dummy1 = reader.ReadUInt32(); // 0x80000000
                    pfi.size = reader.ReadInt64();
                    // Manually read the string for filename
                    pfi.name = "";
                    for (int c = 0; c < 0x108; c++)
                    {
                        byte ch = reader.ReadByte();
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
                else
                if (_owner.PakType == PakFileType.TypeF)
                {
                    pfi.dummy2 = reader.ReadUInt64(); // unused ?
                    // Manually read the string for filename
                    pfi.name = "";
                    for (int c = 0; c < 0x108; c++)
                    {
                        byte ch = reader.ReadByte();
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
                else
                {
                    /*
                    using (var hf = File.OpenWrite("fileheader.bin"))
                    {
                        ms.CopyTo(hf);
                    }
                    ms.Position = 0;
                    */
                }

                if ((_owner.PakType == PakFileType.TypeA) || (_owner.PakType == PakFileType.TypeF))
                {
                    // TypeA has files first and extra files last
                    if (filesToGo > 0)
                    {
                        fileIndexCounter++;
                        pfi.entryIndexNumber = fileIndexCounter;

                        filesToGo--;
                        _owner.files.Add(pfi);
                    }
                    else
                    if (extraToGo > 0)
                    {
                        // "Extra" Files. It looks like these are old deleted files renamed to "__unused__"
                        // There might be more to these, but can't be sure at this moment, looks like they are 512 byte blocks on my paks
                        deletedIndexCounter++;
                        pfi.deletedIndexNumber = deletedIndexCounter;

                        extraToGo--;
                        _owner.extraFiles.Add(pfi);
                    }
                }
                else
                if (_owner.PakType == PakFileType.TypeB)
                {
                    // TypeB has extra files first and normal files last
                    if (extraToGo > 0)
                    {
                        fileIndexCounter++;
                        pfi.entryIndexNumber = fileIndexCounter;

                        extraToGo--;
                        _owner.extraFiles.Add(pfi);
                    }
                    else
                    if (filesToGo > 0)
                    {
                        deletedIndexCounter++;
                        pfi.deletedIndexNumber = deletedIndexCounter;

                        filesToGo--;
                        _owner.files.Add(pfi);
                    }
                }
                else
                {
                    // Call the police, illegal Types are invading our safespace
                }

                // determin the newest date, use both creating and modify date for this
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

                /*
                // Debug stuff
                if (pfi.name == "bin32/archeage.exe")
                {
                    ByteArrayToHexFile(decryptedFileData, "file-"+ i.ToString() + ".hex");
                    File.WriteAllBytes("file-" + i.ToString() + ".bin",decryptedFileData);
                }
                */

                // Update our "end of file data" location if needed
                if ((pfi.offset + pfi.size + pfi.paddingSize) > AddFileOffset)
                {
                    AddFileOffset = pfi.offset + pfi.size + pfi.paddingSize;
                }
            }

            ms.Dispose();
        }


        /// <summary>
        /// Helper function for debugging, write byte array as a hex text file
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        private static void ByteArrayToHexFile(byte[] bytes,string fileName)
        {
            string s = "";
            for(int i = 0; i < bytes.Length;i++)
            {
                s += bytes[i].ToString("X2") + " ";
                if ((i % 16) == 15)
                    s += "\r\n";
                else
                {
                    if ((i % 4) == 3)
                        s += " ";
                    if ((i % 8) == 7)
                        s += " ";
                }
            }
            File.WriteAllText(fileName, s);
        }

        /// <summary>
        /// Helper function for debugging, write byte array as a hex text file
        /// </summary>
        /// <param name="bytes"></param>
        public static string ByteArrayToHexString(byte[] bytes, string spacingText = " ", string lineFeed = "\r\n")
        {
            string s = "";
            for(int i = 0; i < bytes.Length;i++)
            {
                s += bytes[i].ToString("X2") + spacingText;
                if ((i % 16) == 15)
                    s += lineFeed;
                else
                {
                    if ((i % 4) == 3)
                        s += spacingText;
                    if ((i % 8) == 7)
                        s += spacingText;
                }
            }

            return s;
        }


        /// <summary>
        /// Decrypt the current header data to get the file counts
        /// </summary>
        public void DecryptHeaderData()
        {
            data = EncryptAES(rawData, key, false);

            // A valid header/footer is check by it's identifier
            if ((data[0] == 'W') && (data[1] == 'I') && (data[2] == 'B') && (data[3] == 'O'))
            {
                // W I B O = 0x57 0x49 0x42 0x4F
                _owner.PakType = PakFileType.TypeA;
                fileCount = BitConverter.ToUInt32(data, 8);
                extraFileCount = BitConverter.ToUInt32(data, 12);
                isValid = true;
            }
            else
            if ((data[8] == 'I') && (data[9] == 'D') && (data[10] == 'E') && (data[11] == 'J'))
            {
                // I D E J = 0x49 0x44 0x45 0x4A
                _owner.PakType = PakFileType.TypeB;
                fileCount = BitConverter.ToUInt32(data, 12);
                extraFileCount = BitConverter.ToUInt32(data, 0);
                isValid = true;
            }
            else
            if ((data[0] == 'Z') && (data[1] == 'E') && (data[2] == 'R') && (data[3] == 'O'))
            {
                // Z E R O = 0x5A 0x45 0x52 0x4F
                _owner.PakType = PakFileType.TypeF;
                fileCount = BitConverter.ToUInt32(data, 8);
                extraFileCount = BitConverter.ToUInt32(data, 12);
                isValid = true;
            }
            else
            {
                // Doesn't look like this is a pak file, the file is corrupted, or is in a unknown layout/format
                fileCount = 0;
                extraFileCount = 0;
                isValid = false;

                if (_owner.DebugMode)
                {
                    var hex = ByteArrayToHexString(key, "", "");
                    File.WriteAllBytes("game_pak_failed_header_" + hex + ".key", data);
                }
            }

        }

        /// <summary>
        /// Encrypt the current header data
        /// </summary>
        public void EncryptHeaderData()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, headerSize);
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);

            if (_owner.PakType == PakFileType.TypeA)
            {
                writer.Write((byte)'W');
                writer.Write((byte)'I');
                writer.Write((byte)'B');
                writer.Write((byte)'O');
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write(fileCount);
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(extraFileCount);
            }
            else
            if (_owner.PakType == PakFileType.TypeB)
            {
                writer.Write(extraFileCount);
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write((byte)'I');
                writer.Write((byte)'D');
                writer.Write((byte)'E');
                writer.Write((byte)'J');
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(fileCount);
            }
            else
            if (_owner.PakType == PakFileType.TypeF)
            {
                writer.Write((byte)'Z');
                writer.Write((byte)'E');
                writer.Write((byte)'R');
                writer.Write((byte)'O');
                writer.Seek(8, SeekOrigin.Begin);
                writer.Write(fileCount);
                writer.Seek(12, SeekOrigin.Begin);
                writer.Write(extraFileCount);
            }
            else
            {
                // I don't know what to do with something that shouldn't exist
            }

            ms.Position = 0;
            ms.Read(data, 0, headerSize);
            ms.Dispose();
            // Encrypted our stored data into rawData
            rawData = EncryptAES(data, key, true);
        }

    }
}