using Downloader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class DownloadHelper
    {
        public static async Task Download(string url, string fileName,
            EventHandler<DownloadStartedEventArgs>? downloadStartedEvent = null,
            EventHandler<Downloader.DownloadProgressChangedEventArgs>? downloadProgressChangedEventArgs = null,
            EventHandler<AsyncCompletedEventArgs>? downloadCompletedEvent = null,
            CancellationToken cancellationToken = default)
        {
            using DownloadService downloadService = new(Configuration);
            downloadService.DownloadStarted += downloadStartedEvent;
            downloadService.DownloadProgressChanged += downloadProgressChangedEventArgs;
            downloadService.DownloadFileCompleted += downloadCompletedEvent;
            await downloadService.DownloadFileTaskAsync(url, fileName, cancellationToken);
        }

        private static DownloadConfiguration Configuration { get; } = new DownloadConfiguration()
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
        };
    }
}
