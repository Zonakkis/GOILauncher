using Downloader;
using System.ComponentModel;

namespace GOILUpdater.Interfaces
{
    public interface IDownloadable
    {
        string URL { get; }
        double ProgressPercentage { get; set; }
        string Status { get; set; }
        void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs);
        public void OnDownloadProgressChanged(object? sender,
            DownloadProgressChangedEventArgs eventArgs);
        public void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs);
    }
}
