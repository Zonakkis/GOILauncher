using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Downloader;
using Ionic.Zip;
using LC.Newtonsoft.Json.Linq;
using LeanCloud.Storage;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GOI地图管理器.Models
{
    public class Map:LCObject, INotifyPropertyChanged
    {
        public Map() : base("Map")
        {
            Downloadable = true;
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
        public void OnDownloadStarted(object sender, DownloadStartedEventArgs eventArgs)
        {
            //DownloadSize += eventArgs.TotalBytesToReceive;
            SingleFileSize = eventArgs.TotalBytesToReceive;
            Status = "下载中";
            IsDownloading = true;
        }
        public void OnChunkDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            //Trace.WriteLine(eventArgs.ActiveChunks);
        }
        public void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = Convert.ToInt32((float)(ReceivedSize + eventArgs.ReceivedBytesSize) / DownloadSize * 100f);
            Trace.WriteLine(ProgressPercentage);
        }
        public void OnDownloadCompleted(object sender, AsyncCompletedEventArgs eventArgs)
        {
            ReceivedSize += SingleFileSize;

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
        public long DownloadSize { get; set; }
        public long SingleFileSize { get; set; }
        public long ReceivedSize { get; set; }

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
