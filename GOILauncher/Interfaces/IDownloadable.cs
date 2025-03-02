﻿using Downloader;
using System.ComponentModel;
using GOILauncher.Helpers;
using ReactiveUI;
using System.Reactive;

namespace GOILauncher.Interfaces
{
    public interface IDownloadable<T>
    {
        string Url { get; }
        double ProgressPercentage { get; set; }
        bool Downloadable { get; set; }
        string Status { get; set; }
        bool IsDownloading { get; set; }
        bool IsExtracting { get; set; }
        bool IsInstalling { get; set; }
        ReactiveCommand<T, Unit> DownloadCommand { get; set; }
        void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {
            Downloadable = false;
            IsDownloading = true;
            Status = "启动下载中";
        }
        public void OnDownloadProgressChanged(object? sender,
            DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = eventArgs.ProgressPercentage;
            Status = $"下载中({ProgressPercentage:0.0}%/{StorageUnitConvertHelper.ByteTo(eventArgs.BytesPerSecondSpeed)}/s)";
        }
        public void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs)
        {
            IsDownloading = false;
        }
    }
}
