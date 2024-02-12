using GOI地图管理器.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.Helpers
{
    internal class ZipHelper
    {
        public static async Task CombineZipSegment(string path, string zipName, string searchPattern)
        {
            List<string> zipFiles = Directory.GetFiles(path, searchPattern).ToList();
            using (Stream zipStream = File.OpenWrite(Path.Combine(path, zipName)))
            {
                for (int i = 0; i < zipFiles.Count; i++)
                {
                    using (Stream stream = File.OpenRead(zipFiles[i]))
                    {
                        await Task.Run(() => stream.CopyTo(zipStream));
                    }
                    File.Delete(zipFiles[i]);
                }
            }
        }
        public static async Task ExtractMap(string zipPath, string destinationPath,Map map)
        {
            using (ZipFile zip = ZipFile.Read(zipPath))
            {
                zip.ExtractProgress += map.OnExtractProgressChanged;
                foreach (ZipEntry entry in zip)
                {
                    await Task.Run(() => entry.Extract(destinationPath));
                }
            }
            if (!Setting.Instance.saveMapZip)
            {
                File.Delete(zipPath);
            }
        }
    }
}
