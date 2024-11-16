using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Downloader;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using Ionic.Zip;
using LC.Newtonsoft.Json.Linq;
using LeanCloud.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static GOILauncher.ViewModels.MapManageViewModel;

namespace GOILauncher.Models
{
    public class Map:LCObject, INotifyPropertyChanged
    {
        public Map() : base(nameof(Map))
        {
            TotalByte = 0;
            Downloadable = true;
            DownloadTasks = [];
            ReceivedBytes = [];
            DownloadSpeeds = [];
            DirectURLs = [];
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void CheckWhetherExisted()
        {
            if (!IsDownloading && Setting.Instance.LevelPath != "未选择（选择游戏路径后自动选择，也可手动更改）") 
            {
                if(File.Exists($"{Setting.Instance.LevelPath}/{Name}.scene"))
                {
                    Downloaded = true;
                    Downloadable = false;
                }
                else
                {
                    Downloaded = false;
                    Downloadable = true;
                }
            }
        }
        public async Task WaitForDownloadFinish()
        {
            while(CompletedDownloadCount != DownloadURL.Count)
            {
                if (TotalByte != 0)
                {
                    ProgressPercentage = Convert.ToInt32((float)ReceivedBytes.Sum() / TotalByte * 100f);
                    Status = $"下载中（{ProgressPercentage}%/{StorageUnitConvertHelper.FromBytetoOther(DownloadSpeeds.Sum())}）";
                    //百分比
                    //Trace.WriteLine(ProgressPercentage);

                }
                await Task.Delay(1000);
            }

        }
        public void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {
            //var downloadService = (DownloadService)sender!;
            TotalByte += eventArgs.TotalBytesToReceive;
            //Trace.WriteLine($"{downloadService.Package.FileName}Started.");
            //Trace.WriteLine($"总Bytes:{TotalByte}");
        }
        public void OnDownloadProgressChanged(object? sender, Downloader.DownloadProgressChangedEventArgs eventArgs)
        {
            var downloadService = (DownloadService)sender!; 
            int fileID = Convert.ToInt32(downloadService.Package.FileName.Substring(downloadService.Package.FileName.Length - 3, 3)) - 1;
            ReceivedBytes[fileID] = eventArgs.ReceivedBytesSize;
            DownloadSpeeds[fileID] = eventArgs.BytesPerSecondSpeed;
        }
        public void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs)
        {
            var downloadService = (DownloadService)sender!;
            int fileID = Convert.ToInt32(downloadService.Package.FileName.Substring(downloadService.Package.FileName.Length - 3, 3)) - 1;
            DownloadSpeeds[fileID] = 0;
            //Trace.WriteLine($"目标Bytes:{downloadService.Package.TotalFileSize} 已接收Bytes:{ReceivedBytes[fileID]}");
            //Trace.WriteLine($"{downloadService.Package.FileName}Completed.");
            if (ReceivedBytes[fileID] == 0 || downloadService.Package.TotalFileSize != ReceivedBytes[fileID])
            {
                TotalByte -= downloadService.Package.TotalFileSize;
                ReceivedBytes[fileID] = 0;
                DownloadSpeeds[fileID] = 0;
                DownloadTasks.Add(LanzouyunDownloadHelper.Download(
                    DirectURLs[fileID],
                    downloadService.Package.FileName,
                    OnDownloadStarted,
                    OnDownloadProgressChanged,
                    OnDownloadCompleted
                    ));
                return;
            }
            CompletedDownloadCount++;
        }
        public void OnExtractProgressChanged(object? sender, ExtractProgressEventArgs eventArgs)
        {
            if (eventArgs.TotalBytesToTransfer == 0)
            {
                ProgressPercentage = 0;
                return;
            }
            ProgressPercentage = Convert.ToInt32((float)eventArgs.BytesTransferred / eventArgs.TotalBytesToTransfer * 100f);
        }
        public override string ToString()
        {
            return Name;
        }

        public string Name 
        {
            get=> (this["Name"] as string)!;
        }
        public string Author 
        {
            get => (this["Author"] as string)!;
        }
        public string Size 
        { 
            get => (this["Size"] as string)!;
        }
        public string Preview 
        { 
            get => (this["Preview"] as string)!;
            set
            {
                this["Preview"] = value;
            }
        }
        public List<string> DownloadURL
        {
            get => (this["DownloadURL"] as List<object>)!.ConvertAll<string>(input => (input as string)!);
        }
        public string Form
        {
            get => (this[nameof(Form)] as string)!;
            set
            {
                this[nameof(Form)] = value;
            }
        }
        public string Style
        {
            get => (this[nameof(Style)] as string)!;
            set
            {
                this[nameof(Style)] = value;
            }
        }
        public string Difficulty
        {
            get => (this[nameof(Difficulty)] as string)!;
            set
            {
                this[nameof(Difficulty)] = value;
            }
        }
        private bool downloaded;
        public bool Downloaded { 
            get => downloaded;
            set
            {
                downloaded = value;
                NotifyPropertyChanged(nameof(Downloaded));
            }
        }

        public bool isDownloading;
        public bool IsDownloading
        {
            get => isDownloading; 
            set
            {
                isDownloading = value;
                NotifyPropertyChanged(nameof(IsDownloading));
            }
        }
        public long TotalByte { get; set; }
        public long[] ReceivedBytes { get; set; }
        public double[] DownloadSpeeds { get; set; }
        public List<Task> DownloadTasks { get; set; }
        public string[] DirectURLs { get; set; }
        public int CompletedDownloadCount { get; set; }

        public int progressPercentage;
        public int ProgressPercentage
        {
            get => progressPercentage; 
            set
            {
                progressPercentage = value;
                NotifyPropertyChanged(nameof(ProgressPercentage));
            }
        }

        public string status = string.Empty;
        public string Status
        {
            get => status; 
            set
            {
                status = value;
                NotifyPropertyChanged(nameof(Status));
            }
        }

        public bool downloadable;
        public bool Downloadable
        {
            get => downloadable; 
            set
            {
                downloadable = value;
                NotifyPropertyChanged(nameof(Downloadable));
            }
        }

    }
}
