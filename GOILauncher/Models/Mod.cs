using Downloader;
using GOILauncher.Helpers;
using GOILauncher.Interfaces;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class Mod : LCObject, INotifyPropertyChanged, IDownloadable 
    {
        public Mod(string className) : base(className)
        {
            DownloadableChanged += () => NotifyPropertyChanged(nameof(Downloadable));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async void Download()
        {
            if (!TargetGameVersion.Contains(GameInfo.GameVersion) && !TargetGameVersion.Concatenate("/").Contains(GameInfo.GameVersion))
            {
                await NotificationHelper.ShowContentDialog("提示", $"游戏版本不匹配！\r\n当前版本：{GameInfo.GameVersion}\r\nMod所需版本：{TargetVersion}");
                return;
            }
            Downloadable = false;
            IsDownloading = true;
            Status = "启动下载中";
            CancellationTokenSource tokenSource = new();
            CancellationToken token = tokenSource.Token;
            await DownloadHelper.Download(
                    $"{Setting.DownloadPath}/{Name}{Build}.zip",
                    this,
                    token
                    );
            IsExtracting = true;
            Status = "解压中";
            await ZipHelper.Extract($"{Setting.DownloadPath}/{Name}{Build}.zip", Setting.GamePath);
            GameInfo.Refresh(Setting.GamePath);
            IsExtracting = false;
            IsDownloading = false;
            Downloadable =  true;
            await NotificationHelper.ShowContentDialog("提示", $"已经安装{Name}{Build}！");
        }
        public void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {

        }

        public void OnDownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = eventArgs.ProgressPercentage;
            Status = $"({ProgressPercentage:0.0}%/{StorageUnitConvertHelper.ByteTo(eventArgs.BytesPerSecondSpeed)}/s)下载中";
        }

        public void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs)
        {

        }
        protected static event Action? DownloadableChanged;
        private static bool isDownloadable = true;
        protected static bool IsDownloadable
        {
            get => isDownloadable;
            set
            {
                isDownloadable = value;
                DownloadableChanged?.Invoke();
            }
        }
        public string Name => (this[nameof(Name)] as string)!;
        public string Author => (this[nameof(Author)] as string)!;
        public string Build => (this[nameof(Build)] as string)!;
        public string URL => (this[nameof(URL)] as string)!;
        public List<string> TargetGameVersion => (this[nameof(TargetGameVersion)] as List<object>)!.ConvertAll<string>(input => (input as string)!);
        public string TargetVersion => TargetGameVersion.Concatenate("/");
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
        private static Setting Setting => Setting.Instance;
        private static GameInfo GameInfo => GameInfo.Instance;
    }
}
