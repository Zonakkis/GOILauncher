using Downloader;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class LevelLoader : LCObject, INotifyPropertyChanged
    {
        public LevelLoader() : base("LevelLoader")
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
        public void OnDownloadStarted(object sender, DownloadStartedEventArgs eventArgs)
        {
            Status = "下载中";
        }
        public void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = Convert.ToInt32((float)eventArgs.ReceivedBytesSize / eventArgs.TotalBytesToReceive * 100f);
        }

        public async void Download()
        {
            var contentDialog = new ContentDialog()
            {
                Title = "提示",
                CloseButtonText = "好的",
                FontSize = 200,
            };
            if (!ApplicableGameVersion.Contains(GameInfo.Instance.GameVersion))
            {
                contentDialog.Content = $"游戏版本不匹配！\r\n当前版本：{GameInfo.Instance.GameVersion}  Mod所需版本：{ApplicableGameVersion}";
                await contentDialog.ShowAsync();
                return;
            }
            Status = "获取下载地址中";
            IsDownloading = true;
            DownloadURL = await LanzouyunDownloadHelper.GetDirectURLAsync($"https://{DownloadURL}");
            Status = "启动下载中";
            var downloadOpt = new DownloadConfiguration()
            {
                ChunkCount = 8,
                ParallelDownload = true,
                MaxTryAgainOnFailover = int.MaxValue,
                Timeout = 5000,
            };
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            var downloadService = new DownloadService(downloadOpt);
            downloadService.DownloadStarted += OnDownloadStarted;
            downloadService.DownloadProgressChanged += OnDownloadProgressChanged;
            await downloadService.DownloadFileTaskAsync(DownloadURL, $"{Setting.Instance.downloadPath}/LevelLoader{Build}.zip", token);
            IsExtracting = true;
            Status = "解压中";
            await ZipHelper.Extract($"{Setting.Instance.downloadPath}/LevelLoader{Build}.zip", Setting.Instance.gamePath);
            GameInfo.Instance.GetModpackandLevelLoaderVersion(Setting.Instance.gamePath);
            IsExtracting = false;
            IsDownloading = false;
            contentDialog.Content = $"已经安装LevelLoader{Build}！";
            await contentDialog.ShowAsync();
        }

        public string Build
        {
            get => (this["Build"] as string)!;
        }
        public string ApplicableGameVersion
        {
            get => (this["ApplicableGameVersion"] as string)!;
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
