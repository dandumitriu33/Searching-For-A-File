using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SearchingForAFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = args[1];
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
                }
                Console.WriteLine($"{searchResults[0].Name} was last modified on: {lastModification}");
                Thread.Sleep(3000);
            }
            

        }
    }
}
