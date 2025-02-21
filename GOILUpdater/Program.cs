using System.Diagnostics;
using System.IO.Compression;
using Downloader;
using System.Net;
using System.ComponentModel;
using GOILUpdater.Helpers;
using DownloadProgressChangedEventArgs = Downloader.DownloadProgressChangedEventArgs;
using System.Diagnostics.CodeAnalysis;

namespace GOILUpdater
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var exited = false;
            while (!exited)
            {
                if (Process.GetProcessesByName("GOILauncher").Length == 0)
                {
                    exited = true;
                }
                Thread.Sleep(100);
            }
            var url = args[0];
            var downloadService = new DownloadService(new DownloadConfiguration()
            {
                ChunkCount = 16,
                ParallelDownload = true,
                MaxTryAgainOnFailover = int.MaxValue,
                Timeout = 60000,
                //MaximumMemoryBufferBytes = 1024 * 1024 * 50,
                RequestConfiguration =
                {
                    KeepAlive = true,
                    UserAgent="Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:92.0) Gecko/20100101 Firefox/92.0",
                    ProtocolVersion = HttpVersion.Version11,
                }
            });
            downloadService.DownloadStarted += OnDownloadStarted;
            downloadService.DownloadProgressChanged += OnDownloadProgressChanged;
            downloadService.DownloadFileCompleted += OnDownloadCompleted;
            await downloadService.DownloadFileTaskAsync(url, Path.Combine(Directory.GetCurrentDirectory(), "GOILauncher.zip"));
            Console.WriteLine("准备解压……");
            await ExtractZipAsync(Path.Combine(Directory.GetCurrentDirectory(), "GOILauncher.zip"), Directory.GetCurrentDirectory());
            Console.WriteLine("解压完成");
            Process.Start("GOILauncher.exe");
        }

        private static void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {
            Console.WriteLine("开始下载");
        }

        private static void OnDownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs eventArgs)
        {
            Console.WriteLine($"下载中({eventArgs.ProgressPercentage:0.0}%/{StorageUnitConvertHelper.ByteTo(eventArgs.BytesPerSecondSpeed)}/s)");
        }

        private static void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs)
        {
            Console.WriteLine("下载完成");
        }
        private static async Task ExtractZipAsync(string zipPath, string destinationPath)
        {
            await Task.Run(() =>
            {
                using (var zipArchive = ZipFile.OpenRead(zipPath))
                {
                    foreach (var zipArchiveEntry in zipArchive.Entries)
                    {
                        var filePath = Path.Combine(destinationPath, zipArchiveEntry.FullName);
                        if (string.IsNullOrEmpty(zipArchiveEntry.Name))
                        {
                            continue;
                        }
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                        zipArchiveEntry.ExtractToFile(filePath, overwrite: true);
                    }
                }
                File.Delete(zipPath);
            });
        }
    }
}
