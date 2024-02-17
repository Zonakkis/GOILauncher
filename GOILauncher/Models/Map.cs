using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Downloader;
using Ionic.Zip;
using LC.Newtonsoft.Json.Linq;
using LeanCloud.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GOILauncher.Models
{
    public class Map:LCObject, INotifyPropertyChanged
    {
        public Map() : base("Map")
        {
            TotalByte = 0;
            Downloadable = true;
            TotalBytes = new List<long>();
            ReceivedBytes = new List<long>();
            DownloadSpeeds = new List<double>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public void CheckWhetherExisted()
        {
            if (Setting.Instance.levelPath != "未选择（选择游戏路径后自动选择，也可手动更改）" && File.Exists($"{Setting.Instance.levelPath}/{Name}.scene"))
            {
                Downloaded = true;
                Downloadable = false;
            }
        }
        public async Task WaitForDownloadFinish()
        {
            while(CompletedDownloadCount != DownloadURL.Count)
            {
                if (TotalByte != 0)
                {
                    ProgressPercentage = Convert.ToInt32((float)ReceivedBytes.Sum() / TotalByte * 100f);
                    Status = $"下载中（{ConvertStorageUnit(DownloadSpeeds.Sum())}）";
                    Trace.WriteLine(ProgressPercentage);
                }
                await Task.Delay(400);
            }
        }
        public void OnDownloadStarted(object sender, DownloadStartedEventArgs eventArgs)
        {
            var downloadService = (DownloadService)sender;
            TotalBytes[Convert.ToInt32(downloadService.Package.FileName.Substring(downloadService.Package.FileName.Length - 3, 3)) - 1] = eventArgs.TotalBytesToReceive;
            TotalByte = TotalBytes.Sum();
        }
        public void OnChunkDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            //Trace.WriteLine(eventArgs.ActiveChunks);
        }
        public void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            var downloadService = (DownloadService)sender; 
            ReceivedBytes[Convert.ToInt32(downloadService.Package.FileName.Substring(downloadService.Package.FileName.Length - 3, 3))-1] = eventArgs.ReceivedBytesSize;
            DownloadSpeeds[Convert.ToInt32(downloadService.Package.FileName.Substring(downloadService.Package.FileName.Length - 3, 3)) - 1] = eventArgs.BytesPerSecondSpeed;

            //ProgressPercentage = Convert.ToInt32((float)ReceivedSizes.Sum() / DownloadSize * 100f);
            //ProgressPercentage = Convert.ToInt32((float)(ReceivedSize + eventArgs.ReceivedBytesSize) / DownloadSize * 100f);
            //Trace.WriteLine(ProgressPercentage);
        }
        public void OnDownloadCompleted(object sender, AsyncCompletedEventArgs eventArgs)
        {
            CompletedDownloadCount++;

        }
        public void OnExtractProgressChanged(object sender, ExtractProgressEventArgs eventArgs)
        {
            if (eventArgs.TotalBytesToTransfer == 0)
            {
                ProgressPercentage = 0;
                return;
            }
            ProgressPercentage = Convert.ToInt32((float)eventArgs.BytesTransferred / eventArgs.TotalBytesToTransfer * 100f);
            Trace.WriteLine(ProgressPercentage);
        }
        public override string ToString()
        {
            return Name;
        }
        public string ConvertStorageUnit(double bytes)
        {
            if(bytes < 1024)
            {
                return $"{DownloadSpeeds.Sum().ToString("0.00")}B/s";
            }
            else if(bytes < 1048576)
            {
                return $"{(DownloadSpeeds.Sum()/1024D).ToString("0.00")}KB/s";
            }
            else
            {
                return $"{(DownloadSpeeds.Sum() / 1048576D).ToString("0.00")}MB/s";
            }
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
            get=>(this["DownloadURL"] as List<object>)!.ConvertAll<string>(input => (input as string)!);
        }

        private bool downloaded;
        public bool Downloaded { 
            get => downloaded;
            set
            {
                downloaded = value;
                NotifyPropertyChanged("Downloaded");
            }
        }

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
        public long TotalByte { get; set; }
        public List<long> TotalBytes { get; set; }
        public List<long> ReceivedBytes { get; set; }
        public List<double> DownloadSpeeds { get; set; }
        public int CompletedDownloadCount { get; set; }

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

        public bool downloadable;
        public bool Downloadable
        {
            get => downloadable; 
            set
            {
                downloadable = value;
                NotifyPropertyChanged("Downloadable");
            }
        }

    }
}
