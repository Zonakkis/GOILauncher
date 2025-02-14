using Avalonia.Controls.Documents;
using Downloader;
using GOILauncher.Helpers;
using GOILauncher.Interfaces;
using LeanCloud.Storage;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class Mod : INotifyPropertyChanged, IDownloadable 
    {
        public Mod() 
        {
            DownloadableChanged += () => NotifyPropertyChanged(nameof(Downloadable));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CheckGameVersion()
        {
            return TargetGameVersion.Contains(GameInfo.GameVersion) || TargetGameVersion.Concatenate("/").Contains(GameInfo.GameVersion);
        }
        public async Task Download()
        {
            if (!CheckGameVersion())
            {
                await NotificationHelper.ShowContentDialog("提示", $"游戏版本不匹配！\r\n当前版本：{GameInfo.GameVersion}\r\nMod所需版本：{TargetVersion}");
                return;
            }
            Downloadable = false;
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            //await DownloadHelper.Download(
            //        Path.Combine(SettingPage.DownloadPath, $"{Name}{Build}.zip"),
            //        this,
            //        token
            //        );
            IsExtracting = true;
            Status = "解压中";
            //await ZipHelper.Extract($"{SettingPage.DownloadPath}/{Name}{Build}.zip", SettingPage.GamePath,false);
            //GameInfo.Refresh(SettingPage.GamePath);
            IsExtracting = false;
            Downloadable = true;
            await NotificationHelper.ShowContentDialog("提示", $"已经安装{Name}{Build}！");
        }
        public void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {
            IsDownloading = true;
            Status = "下载中";
        }
        public void OnDownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = eventArgs.ProgressPercentage;
            Status = $"({ProgressPercentage:0.0}%/{StorageUnitConvertHelper.ByteTo(eventArgs.BytesPerSecondSpeed)}/s)下载中";
        }
        protected static event Action? DownloadableChanged;
        private static bool _isDownloadable = true;
        protected static bool IsDownloadable
        {
            get => _isDownloadable;
            set
            {
                _isDownloadable = value;
                DownloadableChanged?.Invoke();
            }
        }
        public string Name { get; init; }
        public string Author { get; init; }
        public string Build { get; init; }
        public string Url { get; init; }
        public List<string> TargetGameVersion { get; init; }
        public string TargetVersion => string.Join("/", TargetGameVersion);
        private double progressPercentage;
        public double ProgressPercentage
        {
            get => progressPercentage;
            set
            {
                progressPercentage = value;
                NotifyPropertyChanged(nameof(ProgressPercentage));
            }
        }
        private string status = string.Empty;
        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyPropertyChanged(nameof(Status));
            }
        }
        private bool isDownloading;
        public bool IsDownloading
        {
            get => isDownloading;
            set
            {
                isDownloading = value;
                NotifyPropertyChanged(nameof(IsDownloading));
            }
        }
        private bool isExtracting;
        public bool IsExtracting
        {
            get => isExtracting;
            set
            {
                isExtracting = value;
                NotifyPropertyChanged(nameof(IsExtracting));
            }
        }
        public bool Downloadable { get => Mod.IsDownloadable; set => Mod.IsDownloadable = value; }
        private static GameInfo GameInfo => GameInfo.Instance;
    }
}
