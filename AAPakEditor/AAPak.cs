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
using SubStream;

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
        [FieldOffset(0x118)] public Int64 xsize; // used for encryption ?
        [FieldOffset(0x120)] public Int32 zsize; // ??
        [FieldOffset(0x124)] public byte[] md5;
        [FieldOffset(0x134)] public Int32 dummy1; // ??
        [FieldOffset(0x138)] public System.Runtime.InteropServices.ComTypes.FILETIME createTime ;
        [FieldOffset(0x140)] public System.Runtime.InteropServices.ComTypes.FILETIME modifyTime ;
        [FieldOffset(0x148)] public long dummy2; // ??
    }

    public class AAPakFileHeader
    {
        public AAPak _owner;
        private static int headerSize = 0x200;
        private static int fileInfoSize = 0x150;
        public int Size = headerSize;
        public long headerOffset = 0 ;
        public byte[] rawData = new byte[headerSize]; // unencrypted header
        public byte[] data = new byte[headerSize]; // decrypted header data
        public bool isValid;
        public uint fileCount = 0;
        public uint extraFileCount = 0; // not sure what this is for
        private readonly byte[] key = new byte[] { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F }; // AES_128 Key
        public List<AAPakFileInfo> files = new List<AAPakFileInfo>();

        public AAPakFileHeader(AAPak owner)
        {
            _owner = owner;
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
            long FirstFileInfoOffset = _owner._gpFileStream.Position ;

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
                pfi.xsize = reader.ReadInt64(); // ???
                pfi.zsize = reader.ReadInt32(); // ???
                pfi.md5 = reader.ReadBytes(16);
                pfi.dummy1 = reader.ReadInt32(); // ???
                pfi.createTime.dwLowDateTime = reader.ReadInt32();
                pfi.createTime.dwHighDateTime = reader.ReadInt32();
                pfi.modifyTime.dwLowDateTime = reader.ReadInt32();
                pfi.modifyTime.dwHighDateTime = reader.ReadInt32();
                pfi.dummy2 = reader.ReadInt64(); // ???

                files.Add(pfi);
            }
            ms.Dispose();

        }

        public void Decrypt()
        {
            data = EncryptAES(rawData, key, false);
            fileCount = BitConverter.ToUInt32(data, 8);// & 0x00FFFFFF; // High byte doesn't seem to be involved in here, so maybe it's only supposed to be 3 bytes that are used instead of 8 ?
            extraFileCount = BitConverter.ToUInt32(data, 12);// & 0x00FFFFFF;
            ReadFileTable();
        }

        public void Encrypt()
        {
            rawData = EncryptAES(data, key, true);
        }

    }

    public class AAPak
    {
        public string _gpFilePath { get; private set; }
        public FileStream _gpFileStream { get; private set; }
        public bool isOpen = false;
        public bool isDirty = false;
        protected AAPakFileHeader _header ;

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
                return ReadHeader();
            }
            catch
            {
                _gpFilePath = null ;
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
            _header.Encrypt();

            // Move to our expected header location
            _gpFileStream.Seek(_header.headerOffset, SeekOrigin.Begin);
            _gpFileStream.Write(_header.rawData, 0, _header.Size);

            isDirty = false;
        }

        protected bool ReadHeader()
        {
            _header.files.Clear();
            // Read the last 512 bytes as raw header data
            _gpFileStream.Seek(-_header.Size, SeekOrigin.End);
            // Mark correct location as header offset location
            _header.headerOffset = _gpFileStream.Position;
            _gpFileStream.Read(_header.rawData, 0, _header.Size);
            _header.Decrypt();
            _header.isValid = (_header.data[0] == 'W') && (_header.data[1] == 'I') && (_header.data[2] == 'B') && (_header.data[3] == 'O');

            return _header.isValid ;
        }

    }
}
