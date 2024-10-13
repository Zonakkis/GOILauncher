using GOILauncher.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Joins;
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
            using Stream zipStream = File.OpenWrite(Path.Combine(path, zipName));
            for (int i = 0; i < zipFiles.Count; i++)
            {
                using (Stream stream = File.OpenRead(zipFiles[i]))
                {
                    await Task.Run(() => stream.CopyTo(zipStream));
                }
                File.Delete(zipFiles[i]);
            }
        }
        public static async Task Extract(string zipPath, string destinationPath,EventHandler<ExtractProgressEventArgs>? progressHandler = null)
        {
            using (ZipFile zip = ZipFile.Read(zipPath, new ReadOptions() { Encoding = Encoding.GetEncoding("GBK") }))
            {
                if(progressHandler is not null)
                {
                    zip.ExtractProgress += progressHandler;
                }
                foreach (ZipEntry entry in zip)
                {

                    await Task.Run(() => entry.Extract(destinationPath, ExtractExistingFileAction.OverwriteSilently));
                }
            }
            if (!Setting.Instance.saveMapZip)
            {
                File.Delete(zipPath);
            }
        }
    }
}
