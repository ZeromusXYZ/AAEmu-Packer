using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AAPacker; // Assuming this is where AAPakFileInfo lives



namespace AAPakEditor.Helpers
{
    public class PakScanner
    {
        private AAPak _pak;

        public PakScanner(AAPak pak)
        {
            _pak = pak;
        }

        // 1. Logic for the File List (CSV)
        public string GenerateCsvReport()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name,Path,Size,Padding,Offset,MD5");

            foreach (var pfi in _pak.Files)
            {
                sb.AppendLine($"\"{Path.GetFileName(pfi.Name)}\",\"{pfi.Name}\",{pfi.Size},{pfi.PaddingSize},{pfi.Offset},{BitConverter.ToString(pfi.Md5).Replace("-", "")}");
            }
            return sb.ToString();
        }

        // 2. Logic for the Empty Space (Holes)
        public string GenerateHoleReport()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--- Pak Hole Report ---");

            // Sort files by offset to find gaps between them
            var sortedFiles = _pak.Files.OrderBy(f => f.Offset).ToList();
            long totalWaste = 0;

            for (int i = 0; i < sortedFiles.Count - 1; i++)
            {
                var current = sortedFiles[i];
                var next = sortedFiles[i + 1];

                long currentEnd = current.Offset + current.Size + current.PaddingSize;
                long gap = next.Offset - currentEnd;

                if (gap > 0)
                {
                    sb.AppendLine($"Hole found after {current.Name}: {gap} bytes at Offset {currentEnd}");
                    totalWaste += gap;
                }
            }

            sb.AppendLine($"\r\nTotal Orphaned 'Hole' Data: {totalWaste / 1024} KB");
            return sb.ToString();
        }

        // 3. Logic for the Dirty Padding Check
        // Warning: This reads the actual file, so it's slower!
        public string GeneratePaddingReportCsv(Action<int> reportProgress = null)
        {
            StringBuilder sb = new StringBuilder();
            // CSV Headers
            sb.AppendLine("FileName,PaddingSize,StartOffset,SampleHex,SampleText");

            int processedCount = 0;

            using (FileStream fs = new FileStream(_pak.GpFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                foreach (var pfi in _pak.Files)
                {
                    processedCount++;
                    if (reportProgress != null && processedCount % 100 == 0)
                        reportProgress(processedCount);

                    // 1. Skip small padding (Filter noise)
                    if (pfi.PaddingSize < 500) continue;

                    // 2. Calculate where the padding starts
                    long paddingOffset = pfi.Offset + pfi.Size;

                    // 3. Read 30 bytes (or less if the padding is very small)
                    int bytesToRead = (int)Math.Min(30, pfi.PaddingSize);
                    byte[] buffer = new byte[bytesToRead];

                    fs.Seek(paddingOffset, SeekOrigin.Begin);
                    fs.Read(buffer, 0, bytesToRead);

                    // 4. Check if it's dirty (not all zeros)
                    if (buffer.Any(b => b != 0))
                    {
                        string hex = BitConverter.ToString(buffer).Replace("-", " ");
                        string text = CleanAscii(buffer);

                        // 5. Add to CSV
                        // We wrap FileName in quotes in case it has commas
                        sb.AppendLine($"\"{pfi.Name}\",{pfi.PaddingSize},0x{paddingOffset:X},\"{hex}\",\"{text}\"");
                    }
                }
            }
            return sb.ToString();
        }

        // Helper to make the 30 bytes readable as text (removes non-printable chars)
        private string CleanAscii(byte[] data)
        {
            char[] outChars = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                // Only keep standard characters, replace others with dots
                outChars[i] = (data[i] >= 32 && data[i] <= 126) ? (char)data[i] : '.';
            }
            return new string(outChars).Replace("\"", "'"); // Escape quotes for CSV
        }
                public void DumpDirtyPadding(long minSize, Action<int> reportProgress = null)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string dumpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "padding_dump", timestamp);
            Directory.CreateDirectory(dumpPath);

            int processedCount = 0;

            using (FileStream fs = new FileStream(_pak.GpFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                foreach (var pfi in _pak.Files)
                {
                    processedCount++;
                    if (reportProgress != null && processedCount % 100 == 0)
                        reportProgress(processedCount);

                    if (pfi.PaddingSize < minSize) continue;

                    long paddingOffset = pfi.Offset + pfi.Size;
                    fs.Seek(paddingOffset, SeekOrigin.Begin);
                    
                    byte[] checkBuffer = new byte[Math.Min(1024, pfi.PaddingSize)];
                    fs.Read(checkBuffer, 0, checkBuffer.Length);

                    if (checkBuffer.Any(b => b != 0))
                    {
                        byte[] fullBuffer = new byte[pfi.PaddingSize];
                        fs.Seek(paddingOffset, SeekOrigin.Begin);
                        fs.Read(fullBuffer, 0, (int)pfi.PaddingSize);

                        string safeName = pfi.Name.Replace("/", "_").Replace("\\", "_") + ".dump";
                        string outPath = Path.Combine(dumpPath, safeName);

                        File.WriteAllBytes(outPath, fullBuffer);
                    }
                }
            }
        }

        public void DumpResidualData(long minSize, Action<int> reportProgress = null)
{
    string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
    string dumpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "residual_dump", timestamp);
    Directory.CreateDirectory(dumpPath);

    // Sort files by offset so we can see the gaps between them
    var sortedFiles = _pak.Files.OrderBy(f => f.Offset).ToList();
    int processedCount = 0;

    using (FileStream fs = new FileStream(_pak.GpFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
        for (int i = 0; i < sortedFiles.Count - 1; i++)
        {
            processedCount++;
            if (reportProgress != null && processedCount % 100 == 0)
                reportProgress(processedCount);

            var current = sortedFiles[i];
            var next = sortedFiles[i + 1];

            long currentEnd = current.Offset + current.Size + current.PaddingSize;
            long gapSize = next.Offset - currentEnd;

            if (gapSize >= minSize && gapSize > 0)
            {
                // Seek to the hole
                fs.Seek(currentEnd, SeekOrigin.Begin);
                
                byte[] fullBuffer = new byte[gapSize];
                fs.Read(fullBuffer, 0, (int)gapSize);

                // Only save if it's not just a block of Zeros
                if (fullBuffer.Any(b => b != 0))
                {
                    // Clean up the name of the file before the hole to use in the filename
                    string prevName = Path.GetFileName(current.Name).Replace(".", "_");
                    string outName = $"Hole_After_{prevName}_at_0x{currentEnd:X}.bin";
                    
                    // Write the UNIQUE file to the folder
                    File.WriteAllBytes(Path.Combine(dumpPath, outName), fullBuffer);
                }
            }
        }
    }
}
    }
}