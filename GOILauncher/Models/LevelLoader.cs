using Downloader;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class LevelLoader : LCObject, INotifyPropertyChanged
    {
        public LevelLoader() : base(nameof(LevelLoader))
        {
            DownloadCommand = ReactiveCommand.Create(Download);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {
            Status = "下载中";
        }
        public void OnDownloadProgressChanged(object? sender, Downloader.DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = Convert.ToInt32((float)eventArgs.ReceivedBytesSize / eventArgs.TotalBytesToReceive * 100f);
        }

        public async void Download()
        {
            if (!TargetGameVersion.Contains(GameInfo.Instance.GameVersion))
            {
                await DialogHelper.ShowContentDialog("提示", $"游戏版本不匹配！\r\n当前版本：{GameInfo.Instance.GameVersion}\r\nMod所需版本：{TargetVersion}");
                return;
            }
            Status = "获取下载地址中";
            IsDownloading = true;
            DownloadURL = await LanzouyunDownloadHelper.GetDirectURLAsync(DownloadURL);
            Status = "启动下载中";
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token; 
            await LanzouyunDownloadHelper.Download(
                    DownloadURL,
                    $"{Setting.Instance.downloadPath}/{nameof(LevelLoader)}{Build}.zip",
                    OnDownloadStarted,
                    OnDownloadProgressChanged,
                    null,
                    token
                    );
            IsExtracting = true;
            Status = "解压中";
            await ZipHelper.Extract($"{Setting.Instance.downloadPath}/{nameof(LevelLoader)}{Build}.zip", Setting.Instance.gamePath);
            GameInfo.Instance.GetModpackandLevelLoaderVersion(Setting.Instance.gamePath);
            IsExtracting = false;
            IsDownloading = false;
            await DialogHelper.ShowContentDialog("提示", $"已经安装{nameof(LevelLoader)}{Build}！");

        }

        public string Build
        {
            get => (this["Build"] as string)!;
        }
        public List<string> TargetGameVersion
        {
            get
            {
                return (this[nameof(TargetGameVersion)] as List<object>)!.ConvertAll<string>(input => (input as string)!);
            }
        }

        public string TargetVersion
        {
            get
            {
                return TargetGameVersion.Concatenate("/");
            }
        }
        public string DownloadURL
        {
            get => (this["DownloadURL"] as string)!;
            set
            {
                this["DownloadURL"] = value;
            }
        }

        public ReactiveCommand<Unit, Unit> DownloadCommand { get; }

        public bool isDownloading;
        public bool IsDownloading
        {
            get => isDownloading;
            set
            {
                isDownloading = value;
                NotifyPropertyChanged("IsDownloading");
            }
        }

        public bool isExtracting;
        public bool IsExtracting
        {
            get => isExtracting;
            set
            {
                isExtracting = value;
                NotifyPropertyChanged("IsExtracting");
            }
        }

        public string status;
        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public int progressPercentage;
        public int ProgressPercentage
        {
            get => progressPercentage;
            set
            {
                progressPercentage = value;
                NotifyPropertyChanged("ProgressPercentage");
            }
        }
    }
}
