using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using SubStream;

namespace AAPakEditor
{
    
    [StructLayout(LayoutKind.Explicit)]
    public struct AAPakFileInfo
    {
        [FieldOffset(0x000)] public string name;
        [FieldOffset(0x100)] public long offset;
        [FieldOffset(0x108)] public long size;
        [FieldOffset(0x110)] public long xsize; // used for encryption ?
        [FieldOffset(0x118)] public long zsize; // ??
        [FieldOffset(0x120)] public byte[] md5;
        [FieldOffset(0x130)] public long dummy1; // ??
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
        public long fileCount = 0;
        public long extraFileCount = 0; // not sure what this is for
        private readonly byte[] key = new byte[] { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F }; // AES_128 Key


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
            var FileInfoSize = (fileCount + extraFileCount) * fileInfoSize;
            _owner._gpFileStream.Seek(0, SeekOrigin.End);

            // TODO: read file table

        }

        public void Decrypt()
        {
            data = EncryptAES(rawData, key, false);
            fileCount = BitConverter.ToInt64(data, 8) & 0x00FFFFFF; // High byte doesn't seem to be involved in here
            extraFileCount = BitConverter.ToInt64(data, 16) & 0x00FFFFFF;
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
