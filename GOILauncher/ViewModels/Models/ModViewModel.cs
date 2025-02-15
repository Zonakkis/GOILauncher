using System;
using GOILauncher.Interfaces;
using GOILauncher.Models;
using ReactiveUI;
using System.Collections.Generic;
using Downloader;
using GOILauncher.Helpers;
using System.ComponentModel;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace GOILauncher.ViewModels.Models;

public class ModViewModel : ReactiveObject,IDownloadable
{
    public ModViewModel(Mod mod)
    {
        try
        {
            _mod = mod;
            this.WhenAnyValue(x => x.IsDownloading, x => x.IsExtracting,
                (isDownloading, isExtracting) => isDownloading || isExtracting)
                .Subscribe(isInstalling => IsInstalling = isInstalling);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    private readonly Mod _mod;
    public string Name => _mod.Name;
    public string Author => _mod.Author;
    public string Build => _mod.Build;
    public string Url => _mod.Url;
    public List<string> TargetGameVersion => _mod.TargetGameVersion;
    public string TargetGameVersionString => string.Join("/", TargetGameVersion);
    public double ProgressPercentage
    { 
        get => _mod.ProgressPercentage; 
        set
        {
            _mod.ProgressPercentage = value;
            this.RaisePropertyChanged();
        }
    }
    public bool Downloadable
    {
        get => _mod.Downloadable;
        set
        {
            _mod.Downloadable = value;
            this.RaisePropertyChanged();
        }
    }
    public string Status
    {
        get => _mod.Status;
        set
        {
            _mod.Status = value;
            this.RaisePropertyChanged();
        }
    }
    public bool IsDownloading
    {
        get => _mod.IsDownloading;
        set
        {
            _mod.IsDownloading = value;
            this.RaisePropertyChanged();
        }
    }
    public bool IsExtracting
    {
        get => _mod.IsExtracting;
        set
        {
            _mod.IsExtracting = value;
            this.RaisePropertyChanged();
        }
    }
    [Reactive]
    public bool IsInstalling { get; set; }
    public void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
    {
        Downloadable = false;
        IsDownloading = true;
        Status = "启动下载中";
    }
    public void OnDownloadProgressChanged(object? sender,DownloadProgressChangedEventArgs eventArgs)
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
}