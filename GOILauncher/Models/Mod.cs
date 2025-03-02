﻿using Downloader;
using GOILauncher.Helpers;
using GOILauncher.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Text.Json.Serialization;

namespace GOILauncher.Models
{
    public class Mod : ReactiveObject, IDownloadable<Mod>
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
        [JsonPropertyName("name")]
        public string Name { get; init; }
        [JsonPropertyName("author")]
        public string Author { get; init; }
        [JsonPropertyName("build")]
        public string Build { get; init; }
        [JsonPropertyName("url")]
        public string Url { get; init; }
        [JsonPropertyName("target_game_version")]
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
        public ReactiveCommand<Mod,Unit> DownloadCommand { get; set; }
    }
}