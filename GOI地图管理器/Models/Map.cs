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
    public class Map:INotifyPropertyChanged
    {
        public Map(LCObject map)
        {
            MapObject = map;
            Name = (string)MapObject["Name"];
            IsLoaded = false;
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
        public void Load()
        {
            this.Author = (string)MapObject["Author"];
            this.Size = (string)MapObject["Size"];
            object obj = MapObject["Preview"];
            LCFile file;
            if ((string)obj != "") 
            {
                file = new LCFile("Preview.png", new Uri((string)obj));
            }
            else
            {
                file = new LCFile("Preview.png", new Uri("http://lc-3Dec7Zyj.cn-n1.lcfile.com/7B1JKdTscW56vNKj8LkmlzG9OEE6Ssep/No%20Image.png"));
            }
            this.Preview = file.GetThumbnailUrl(640, 360, 50, false, "png");
            IsLoaded = true;
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
        public LCObject MapObject { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Size { get; set; }
        public string Preview { get; set; }
        public bool IsLoaded { get; set; }

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
