using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using SubStreamHelper;

namespace AAPakEditor
{
    
    [StructLayout(LayoutKind.Explicit)]
    public struct AAPakFileInfo
    {
        /*
        getdstring NAME 0x108 MEMORY_FILE
        get OFFSET longlong MEMORY_FILE
        get SIZE longlong MEMORY_FILE
        get XSIZE longlong MEMORY_FILE     # used for encryption alignment?
        get ZSIZE long MEMORY_FILE         # ???
        getdstring DUMMY 16 MEMORY_FILE    # MD5 Hash
        get DUMMY1 long MEMORY_FILE        # ???
        get TIMESTAMP longlong MEMORY_FILE # Creation TIMESTAMP
        get TIMESTAMP longlong MEMORY_FILE # Modification TIMESTAMP
        get DUMMY2 longlong MEMORY_FILE    # ???
        */
        [FieldOffset(0x000)] [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x108)] public string name;
        [FieldOffset(0x108)] public Int64 offset;
        [FieldOffset(0x110)] public Int64 size;
        [FieldOffset(0x118)] public Int64 sizeDuplicate; // maybe compressed data size ? if used
        [FieldOffset(0x120)] public Int32 paddingSize; // ??
        [FieldOffset(0x124)] public byte[] md5;
        [FieldOffset(0x134)] public Int32 dummy1; // looks like padding, always 0 ?
        [FieldOffset(0x138)] public Int64 createTime;
        [FieldOffset(0x140)] public Int64 modifyTime;
        //[FieldOffset(0x138)] public System.Runtime.InteropServices.ComTypes.FILETIME createTime ;
        //[FieldOffset(0x140)] public System.Runtime.InteropServices.ComTypes.FILETIME modifyTime ;
        [FieldOffset(0x148)] public Int64 dummy2; // looks like padding, always 0 ?
    }

    public class AAPakFileHeader
    {
        private readonly byte[] key = new byte[] { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F }; // AES_128 Key
        protected static readonly int headerSize = 0x200;
        protected static readonly int fileInfoSize = 0x150;

        public AAPak _owner;
        public int Size = headerSize;
        public long headerOffset = 0 ;
        public long FirstFileInfoOffset = 0;
        public byte[] rawData = new byte[headerSize]; // unencrypted header
        public byte[] data = new byte[headerSize]; // decrypted header data
        public bool isValid;
        public uint fileCount = 0;
        public uint extraFileCount = 0; // not sure what this is for

        public byte[] nullHash;
        public string nullHashString = "".PadRight(32, '0');

        public AAPakFileHeader(AAPak owner)
        {
            _owner = owner;
            nullHash = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        // Source:
        // https://stackoverflow.com/questions/44782910/aes128-decryption-in-c-sharp

        private static byte[] EncryptAES(byte[] message, byte[] key, bool doEncryption)
        {
            try
            {
                Aes aes = new AesManaged();
                aes.Key = key;
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cipher;

                if (doEncryption == true)
                    cipher = aes.CreateEncryptor();
                else
                    cipher = aes.CreateDecryptor();

                return cipher.TransformFinalBlock(message, 0, message.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ReadFileTable()
        {
            /*
            math SIZE = FILES
            math SIZE += EXTRA_FILES
            math SIZE *= 0x150
            goto 0 0 SEEK_END
            savepos INFO_OFF
            math INFO_OFF -= 0x200
            for INFO_OFF -= SIZE >= 0
                if INFO_OFF % 0x200
                    math INFO_OFF -= 0x10
                else
                    break
                endif
            next
            print "FileTable offset:   %INFO_OFF|x%"
            */
            long TotalFileInfoSize = (fileCount + extraFileCount) * fileInfoSize;
            _owner._gpFileStream.Seek(0, SeekOrigin.End);
            FirstFileInfoOffset = _owner._gpFileStream.Position ;

            // Search for the first file location, it needs to be alligned to a 0x200 size block
            FirstFileInfoOffset -= headerSize;
            FirstFileInfoOffset -= TotalFileInfoSize;
            var dif = FirstFileInfoOffset % 0x200;
            FirstFileInfoOffset -= dif;

            _owner._gpFileStream.Position = FirstFileInfoOffset; // This was 0x00000005c66cf200 or 24803865088 on my example pak


            int bufSize = Marshal.SizeOf(typeof(AAPakFileInfo));
            MemoryStream ms = new MemoryStream(bufSize); // Could probably do without the intermediate memorystream, but it's easier to process
            BinaryReader reader = new BinaryReader(ms);
            for (uint i = 0; i < fileCount; i++)
            {
                byte[] rawFileData = new byte[bufSize]; // decrypted header data
                _owner._gpFileStream.Read(rawFileData, 0, bufSize);
                byte[] decryptedFileData = EncryptAES(rawFileData, key, false);
                ms.SetLength(0);
                ms.Write(decryptedFileData, 0, bufSize);
                ms.Position = 0;
                //BinaryFormatter formatter = new BinaryFormatter);
                //AAPakFileInfo pfi = (AAPakFileInfo)formatter.Deserialize(_owner._gpFileStream);
                AAPakFileInfo pfi = new AAPakFileInfo();
                // Manually read the string for filename
                pfi.name = "";
                for (int c = 0; c < 0x108;c++)
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
                pfi.sizeDuplicate = reader.ReadInt64(); // ???
                pfi.paddingSize = reader.ReadInt32(); // ???
                pfi.md5 = reader.ReadBytes(16);
                pfi.dummy1 = reader.ReadInt32(); // ???
                pfi.createTime = reader.ReadInt64();
                pfi.modifyTime = reader.ReadInt64();
                //pfi.createTime.dwLowDateTime = reader.ReadInt32();
                //pfi.createTime.dwHighDateTime = reader.ReadInt32();
                //pfi.modifyTime.dwLowDateTime = reader.ReadInt32();
                //pfi.modifyTime.dwHighDateTime = reader.ReadInt32();
                pfi.dummy2 = reader.ReadInt64(); // ???

                _owner.files.Add(pfi);
            }
            ms.Dispose();
        }


        public void WriteFileTable()
        {
            _owner._gpFileStream.Position = FirstFileInfoOffset; // Mave back to our saved location


            int bufSize = Marshal.SizeOf(typeof(AAPakFileInfo));
            MemoryStream ms = new MemoryStream(bufSize); // Could probably do without the intermediate memorystream, but it's easier to process
            BinaryWriter writer = new BinaryWriter(ms);
            for (int i = 0; i < _owner.files.Count; i++)
            {
                ms.SetLength(0);
                AAPakFileInfo pfi = _owner.files[i];
                // Manually read the string for filename

                for (int c = 0; c < 0x108; c++)
                {
                    byte ch = 0;
                    if (c < pfi.name.Length)
                        ch = (byte)pfi.name[c];
                    writer.Write(ch);
                }
                writer.Write(pfi.offset);
                writer.Write(pfi.size);
                writer.Write(pfi.sizeDuplicate); // ???
                writer.Write(pfi.paddingSize); // ???
                writer.Write(pfi.md5);
                writer.Write(pfi.dummy1); // ???
                writer.Write(pfi.createTime);
                writer.Write(pfi.modifyTime);
                writer.Write(pfi.dummy2); // ???

                byte[] decryptedFileData = new byte[bufSize]; 
                ms.Position = 0;
                ms.Read(decryptedFileData, 0, bufSize);
                byte[] rawFileData = EncryptAES(decryptedFileData, key, true); // encrypted header data

                _owner._gpFileStream.Write(rawFileData, 0, bufSize);
            }
            ms.Dispose();
        }



        public void Decrypt()
        {
            data = EncryptAES(rawData, key, false);
            fileCount = BitConverter.ToUInt32(data, 8);// & 0x00FFFFFF; // High byte doesn't seem to be involved in here, so maybe it's only supposed to be 3 bytes that are used instead of 8 ?
            extraFileCount = BitConverter.ToUInt32(data, 12);// & 0x00FFFFFF;
        }

        public void Encrypt()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, headerSize);
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write((byte)'W');
            writer.Write((byte)'I');
            writer.Write((byte)'B');
            writer.Write((byte)'O');
            writer.Seek(8, SeekOrigin.Begin);
            writer.Write(fileCount);
            writer.Seek(12, SeekOrigin.Begin);
            writer.Write(extraFileCount);

            ms.Position = 0;
            ms.Read(data, 0, headerSize);

            ms.Dispose();

            rawData = EncryptAES(data, key, true);
        }

    }

    public class AAPak
    {
        protected AAPakFileInfo nullAAPakFileInfo = new AAPakFileInfo();

        public string _gpFilePath { get; private set; }
        public FileStream _gpFileStream { get; private set; }
        public AAPakFileHeader _header;
        public bool isOpen = false;
        public bool isDirty = false;
        public List<AAPakFileInfo> files = new List<AAPakFileInfo>();
        public List<string> folders = new List<string>();

        public AAPak(string filePath)
        {
            _header = new AAPakFileHeader(this);
            bool isLoaded = OpenPak(filePath);
            if (isLoaded)
            {
                isOpen = ReadHeader();
            }
            else
            {
                isOpen = false;
            }
        }

        ~AAPak()
        {
            if (isOpen)
                ClosePak();
        }

        public bool OpenPak(string filePath)
        {
            // Fail if already open
            if (isOpen)
                return false;

            // Check if it exists
            if (!File.Exists(filePath))
            {
                return false;
            }

            try
            {
                // Open stream
                _gpFileStream = new FileStream(filePath, FileMode.Open);
                _gpFilePath = filePath;
                isDirty = false;
                isOpen = true;
                return ReadHeader();
            }
            catch
            {
                _gpFilePath = null ;
                isOpen = false;
                return false;
            }
        }

        public void ClosePak()
        {
            if (!isOpen)
                return;
            if (isDirty)
                SaveHeader();
            _gpFileStream.Close();
            _gpFileStream = null;
            _gpFilePath = null;
            isOpen = false;
        }

        public void SaveHeader()
        {
            _header.WriteFileTable();
            _header.Encrypt();

            // Move to our expected header location
            _gpFileStream.Seek(_header.headerOffset, SeekOrigin.Begin);
            _gpFileStream.Write(_header.rawData, 0, _header.Size);

            isDirty = false;
        }

        protected bool ReadHeader()
        {
            files.Clear();
            // Read the last 512 bytes as raw header data
            _gpFileStream.Seek(-_header.Size, SeekOrigin.End);
            // Mark correct location as header offset location
            _header.headerOffset = _gpFileStream.Position;
            _gpFileStream.Read(_header.rawData, 0, _header.Size);
            _header.Decrypt();
            _header.isValid = (_header.data[0] == 'W') && (_header.data[1] == 'I') && (_header.data[2] == 'B') && (_header.data[3] == 'O');
            if (_header.isValid)
            {
                _header.ReadFileTable();
            }
            return _header.isValid ;
        }

        public void GenerateFolderList()
        {
            // There is no actual directory info stored in the pak file, so we just generate it based on filenames
            folders.Clear();
            if (!isOpen || !_header.isValid) return;
            foreach(AAPakFileInfo pfi in files)
            {
                // Horror function, I know :p
                string n = Path.GetDirectoryName(pfi.name.ToLower().Replace('/', Path.DirectorySeparatorChar)).Replace(Path.DirectorySeparatorChar, '/');
                var pos = folders.IndexOf(n);
                if (pos >= 0) continue;
                folders.Add(n);
            }
            folders.Sort();
        }

        public List<AAPakFileInfo> GetFilesInDirectory(string dirname)
        {
            var res = new List<AAPakFileInfo>();
            dirname = dirname.ToLower();
            foreach (AAPakFileInfo pfi in files)
            {
                // extract dir name
                string n = Path.GetDirectoryName(pfi.name.ToLower().Replace('/', Path.DirectorySeparatorChar)).Replace(Path.DirectorySeparatorChar, '/');
                if (n == dirname)
                    res.Add(pfi);
            }
            return res;
        }

        public AAPakFileInfo GetFileByName(string filename)
        {
            filename = filename.ToLower();
            foreach (AAPakFileInfo pfi in files)
            {
                if (pfi.name == filename)
                    return pfi;
            }
            return nullAAPakFileInfo; // return first file if it fails
        }

        public SubStream ExportFileAsStream(AAPakFileInfo file)
        {
            return new SubStream(_gpFileStream, file.offset, file.size);
        }

        public SubStream ExportFileAsStream(string fileName)
        {
            AAPakFileInfo file = GetFileByName(fileName);
            return new SubStream(_gpFileStream, file.offset, file.size);
        }

        public string UpdateMD5(AAPakFileInfo file)
        {
            MD5 hash = MD5.Create();
            var fs = ExportFileAsStream(file);
            var newHash = hash.ComputeHash(fs);
            hash.Dispose();
            if (!file.md5.SequenceEqual(newHash))
            {
                // Only update if different
                newHash.CopyTo(file.md5,0);
                isDirty = true;
            }
            return BitConverter.ToString(file.md5).Replace("-", ""); // Return the (updated) md5 as a string
        }


    }
}
