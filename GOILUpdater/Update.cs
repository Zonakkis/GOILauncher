using Downloader;
using LeanCloud.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GOILUpdater.Interfaces;
using GOILUpdater.Helpers;

namespace GOILUpdater
{
    public class Update(string url) : LCObject(nameof(Update)),IDownloadable
    {
        public void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {
            Console.WriteLine(Status);
        }
        public void OnDownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = eventArgs.ProgressPercentage;
            Status = $"下载中({ProgressPercentage:0.0}%/{StorageUnitConvertHelper.ByteTo(eventArgs.BytesPerSecondSpeed)}/s)";
            Console.WriteLine(Status);
        }
        public void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs)
        {
            Console.WriteLine("下载完成");
        }
        public string URL { get; } = url;
        public double ProgressPercentage { get; set; }
        public string Status { get; set; } = "准备下载……";
    }
}
