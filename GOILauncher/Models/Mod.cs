using Avalonia.Controls.Documents;
using Downloader;
using GOILauncher.Helpers;
using GOILauncher.Interfaces;
using LeanCloud.Storage;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class Mod : ReactiveObject, IDownloadable 
    {
        public Mod()
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
        [Reactive]
        public string Name { get; init; }
        [Reactive]
        public string Author { get; init; }
        [Reactive]
        public string Build { get; init; }
        [Reactive]
        public string Url { get; init; }
        [Reactive]
        public List<string> TargetGameVersion { get; init; }
        public string TargetGameVersionString => string.Join("/", TargetGameVersion);
        [Reactive]
        public double ProgressPercentage { get; set; }
        [Reactive]
        public string Status { get; set; }
        [Reactive]
        public bool Downloadable { get; set; }
        [Reactive]
        public bool IsDownloading { get; set; }
        [Reactive]
        public bool IsExtracting { get; set; }
        [Reactive]
        public bool IsInstalling { get; set; }
    }
}
