using Downloader;
using GOILauncher.Helpers;
using GOILauncher.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel;
using System.Reactive;

namespace GOILauncher.Models
{
    public class Map: ReactiveObject,IDownloadable<Map>
    {
        public Map()
        {
            this.WhenAnyValue(x => x.IsDownloading, x => x.IsExtracting,
                    (isDownloading, isExtracting) => isDownloading || isExtracting)
                .Subscribe(isInstalling => IsInstalling = isInstalling);
        }
        public void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {
            Downloadable = false;
            IsDownloading = true;
            Status = "启动下载中";
        }
        public void OnDownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = eventArgs.ProgressPercentage; 
            Status = $"下载中({ProgressPercentage:0.0}%/{StorageUnitConvertHelper.ByteTo(eventArgs.BytesPerSecondSpeed)}/s)";
        }
        public void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs)
        {
            IsDownloading = false;
            IsExtracting = true;
            Status = "解压中";
        }
        public string Name { get;init; }
        public string Author { get; init; }
        public string Size { get; init; }
        public string Preview { get; set; } 
        public string Url { get; init; }
        public string Form { get; init; }
        public string Style { get; init; }
        public string Difficulty { get; init; }
        [Reactive]
        public bool IsDownloading { get; set; }
        [Reactive]
        public bool IsExtracting { get; set; }
        [Reactive]
        public bool IsInstalling { get; set; }
        [Reactive]
        public double ProgressPercentage { get; set; }
        [Reactive]
        public string Status { get; set; }
        [Reactive]
        public bool Downloaded { get; set; }
        [Reactive]
        public bool Downloadable { get; set; } = true;
        public ReactiveCommand<Map,Unit> DownloadCommand { get; set; }
    }
}