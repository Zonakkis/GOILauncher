using Downloader;
using GOILUpdater.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILUpdater.Helpers
{
    public class DownloadHelper
    {
        public static async Task Download(string fileName,
            IDownloadable download,
            CancellationToken cancellationToken = default)
        {
            using DownloadService downloadService = new(Configuration);
            downloadService.DownloadStarted += download.OnDownloadStarted;
            downloadService.DownloadProgressChanged += download.OnDownloadProgressChanged;
            downloadService.DownloadFileCompleted += download.OnDownloadCompleted;
            await downloadService.DownloadFileTaskAsync(download.URL, fileName, cancellationToken);
        }
        private static DownloadConfiguration Configuration { get; } = new DownloadConfiguration()
        {
            ChunkCount = 16,
            ParallelDownload = true,
            MaxTryAgainOnFailover = int.MaxValue,
            Timeout = 60000,
            RequestConfiguration =
                {
                    KeepAlive = true,
                    UserAgent="Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:92.0) Gecko/20100101 Firefox/92.0",
                    ProtocolVersion = HttpVersion.Version11,
                }
        };
    }
}
