using Downloader;
using System.ComponentModel;

namespace GOILauncher.Interfaces
{
    internal interface IDownloadable
    {
        string URL { get; }
        double ProgressPercentage { get; set; }
        bool Downloadable { get; set; }
        string Status { get; set; }
        bool IsDownloading { get; set; }
        void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs);
        public void OnDownloadProgressChanged(object? sender,
            DownloadProgressChangedEventArgs eventArgs);
        public void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs);
    }
}
