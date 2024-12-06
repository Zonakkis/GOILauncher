using Downloader;
using GOILauncher.Helpers;
using GOILauncher.Interfaces;
using Ionic.Zip;
using LeanCloud.Storage;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace GOILauncher.Models
{
    public class Map:LCObject, INotifyPropertyChanged,IDownloadable
    {
        public Map() : base(nameof(Map))
        {
            Downloadable = true;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public bool CheckMapExists()
        {
            if (!IsDownloading && !Setting.IsDefault(nameof(Setting.LevelPath)))
            {
                if (Directory.GetDirectories(Setting.LevelPath)
                    .Any(directory => Path.GetFileName(directory).StartsWith(Name, StringComparison.OrdinalIgnoreCase))
                    || File.Exists($"{Setting.LevelPath}/{Name}.scene"))
                {
                    Downloaded = true;
                    Downloadable = false;
                    return true;
                }
                else
                {
                    Downloaded = false;
                    Downloadable = true;
                    return false;
                }
            }
            return false;
        }
        public void OnDownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = eventArgs.ProgressPercentage; 
            Status = $"下载中({ProgressPercentage:0.0}%/{StorageUnitConvertHelper.ByteTo(eventArgs.BytesPerSecondSpeed)}/s)";
        }
        public void OnDownloadCompleted(object? sender, AsyncCompletedEventArgs eventArgs)
        {

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
        public string Name => (this[nameof(Name)] as string)!;
        public string Author => (this[nameof(Author)] as string)!;
        public string Size => (this[nameof(Size)] as string)!;
        public string Preview 
        { 
            get => (this[nameof(Preview)] as string)!;
            set => this[nameof(Preview)] = value;
        }
        public bool HasPreview => Preview is { } str && str.StartsWith("http");
        public string Url => (this[nameof(Url)] as string)!;
        public string Form => (this[nameof(Form)] as string)!;
        public string Style => (this[nameof(Style)] as string)!;
        public string Difficulty => (this[nameof(Difficulty)] as string)!;
        private bool downloaded;
        public bool Downloaded { 
            get => downloaded;
            set
            {
                downloaded = value;
                NotifyPropertyChanged(nameof(Downloaded));
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
        private bool downloadable;
        public bool Downloadable
        {
            get => downloadable; 
            set
            {
                downloadable = value;
                NotifyPropertyChanged(nameof(Downloadable));
            }
        }
        private static Setting Setting => Setting.Instance;
    }
}
