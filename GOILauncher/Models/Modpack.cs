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
    public class Modpack : LCObject, INotifyPropertyChanged
    {
        public Modpack() : base("Modpack")
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
        public void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = Convert.ToInt32((float)eventArgs.ReceivedBytesSize / eventArgs.TotalBytesToReceive * 100f);
        }

        public async void Download()
        {
            //if(ApplicableGameVersion)
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
            downloadService.DownloadProgressChanged += OnDownloadProgressChanged;
            IsDownloading= true;
            await downloadService.DownloadFileTaskAsync(DownloadURL, $"{Setting.Instance.downloadPath}/Modpack{Build}.zip", token);
            IsDownloading = false;
            await ZipHelper.Extract($"{Setting.Instance.downloadPath}/Modpack{Build}.zip", Setting.Instance.gamePath);
            GameInfo.Instance.GetModpackandLevelLoaderVersion(Setting.Instance.gamePath);
            var contentDialog = new ContentDialog()
            {
                Title = "提示",
                Content = $"已经安装Modack{Build}！",
                CloseButtonText = "好的",
            };
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
