using Downloader;
using GOILauncher.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    public class DownloadHelper
    {
        public static async Task Download(string fileName,
            IDownloadable download,
            CancellationToken cancellationToken = default)
        {
            await using DownloadService downloadService = new(Configuration);
            downloadService.DownloadStarted += download.OnDownloadStarted;
            downloadService.DownloadProgressChanged += download.OnDownloadProgressChanged;
            downloadService.DownloadFileCompleted += download.OnDownloadCompleted;
            await downloadService.DownloadFileTaskAsync(download.Url, fileName, cancellationToken);
        }
        private static DownloadConfiguration Configuration { get; } = new()
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
