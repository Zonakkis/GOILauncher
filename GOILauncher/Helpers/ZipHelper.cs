using GOILauncher.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class ZipHelper
    {
        public static async Task CombineZipSegment(string path, string zipName, string searchPattern)
        {
            if (File.Exists(zipName))
            {
                File.Delete(zipName);
            }
            List<string> zipFiles = [.. Directory.GetFiles(path, searchPattern)];
            if(zipFiles.Count == 1)
            {
                File.Move(zipFiles[0], zipName);
                return;
            }

            await using Stream zipStream = File.OpenWrite(Path.Combine(path, zipName));
            foreach (var zipFile in zipFiles)
            {
                await using (Stream stream = File.OpenRead(zipFile))
                {
                    await Task.Run(() => stream.CopyTo(zipStream));
                }
                File.Delete(zipFile);
            }
        }
        public static async Task Extract(string zipPath, string destinationPath,bool deleteAfterExtract, EventHandler<ExtractProgressEventArgs>? progressHandler = null)
        {
            using (var zip = ZipFile.Read(zipPath, new ReadOptions() { Encoding = Encoding.GetEncoding("GBK") }))
            {
                if(progressHandler is not null)
                {
                    zip.ExtractProgress += progressHandler;
                }
                foreach (var entry in zip)
                {

                    await Task.Run(() => entry.Extract(destinationPath, ExtractExistingFileAction.OverwriteSilently));
                }
            }
            if (deleteAfterExtract)
            {
                File.Delete(zipPath);
            }
        }
    }
}
