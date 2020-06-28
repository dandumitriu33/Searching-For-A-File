using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Compression;

namespace SearchingForAFile
{
    class Program
    {
        private static string path { get; set; }
        public static string zipPath { get; set; }
        static void Main(string[] args)
        {
            path = args[1];
            DateTime lastModification = new DateTime(1961, 1, 1, 8, 0, 0);
            DirectoryInfo di = new DirectoryInfo(path);
            try
            {
                if (di.Exists)
                {
                    Console.WriteLine("The path is valid.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The process failed: {e.Message}");
            }
            //string[] searchResults = di.GetFiles(path, "*buzz*", SearchOption.AllDirectories);
            // https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?view=netcore-3.1#System_IO_Directory_GetFiles_System_String_System_String_System_IO_SearchOption_

            while (di.Exists)
            {
                var searchResults = di.GetFiles("*rel*", SearchOption.AllDirectories);
                if (searchResults[0].LastWriteTime > lastModification)
                {
                    lastModification = searchResults[0].LastWriteTime;
                    //var fileInfo = new FileInfo(searchResults[0].FullName);
                    //string newDirectoryName = fileInfo.DirectoryName;
                    string newDirectoryName = searchResults[0].DirectoryName;
                    DirectoryInfo di2 = new DirectoryInfo(newDirectoryName);
                    zipPath = di2.FullName;
                    Compress(di2);
                    
                }
                Console.WriteLine($"{searchResults[0].Name} was last modified on: {lastModification}");
                Thread.Sleep(3000);
            }


        }

        public static void Compress(DirectoryInfo directorySelected)
        {
            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                        FileInfo info = new FileInfo(zipPath + Path.DirectorySeparatorChar + fileToCompress.Name + ".gz");
                        Console.WriteLine($"Compressed {fileToCompress.Name}");
                    }
                }
            }
        }
    }
}
