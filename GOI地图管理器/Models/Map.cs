using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Downloader;
using LeanCloud.Storage;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            this.Name = (string)MapObject["Name"];
            IsLoaded = false;
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
            LCFile file = new LCFile("Preview.png", new Uri((string)MapObject["Preview"]));
            this.Preview = file.GetThumbnailUrl(640, 360, 50, false, "png");
            IsLoaded = true;
        }

        public void OnDownloadStarted(object sender, DownloadStartedEventArgs eventArgs)
        {
            //DownloadSize += eventArgs.TotalBytesToReceive;
            SingleFileSize = eventArgs.TotalBytesToReceive;
            IsDownloading = true;
        }
        public void OnChunkDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            //Trace.WriteLine(eventArgs.ActiveChunks);
        }

        
        public void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            DownloadPercentage = Convert.ToInt32((float)(ReceivedSize + eventArgs.ReceivedBytesSize) / DownloadSize * 100f);
            Trace.WriteLine(DownloadPercentage);
        }
        public void OnDownloadCompleted(object sender, AsyncCompletedEventArgs eventArgs)
        {
            ReceivedSize += SingleFileSize;
            IsDownloading = false;
        }
        public LCObject MapObject { get; set; }
        public string Name { get; set; }

        public string Author { get; set; }

        public string Size { get; set; }

        public string Preview { get; set; }

        public bool IsLoaded { get; set; }

        public bool isDownloading;
        public bool IsDownloading
        {
            get => isDownloading; set
            {
                isDownloading = value;
                NotifyPropertyChanged("IsDownloading");
            }
        }
        public long DownloadSize { get; set; }
        public long SingleFileSize { get; set; }
        public long ReceivedSize { get; set; }

        public int downloadPercentage;
        public int DownloadPercentage
        {
            get => downloadPercentage;  set
            {
                downloadPercentage = value;
                NotifyPropertyChanged("DownloadPercentage");
            }
        }

    }
}
