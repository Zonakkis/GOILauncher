using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Downloader;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Interfaces;
using Ionic.Zip;
using LC.Newtonsoft.Json.Linq;
using LeanCloud.Storage;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static GOILauncher.ViewModels.MapManageViewModel;

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
        public void CheckWhetherExisted()
        {
            if (!IsDownloading && Setting.Instance.LevelPath != "未选择（选择游戏路径后自动选择，也可手动更改）")
            {
                if (Directory.GetDirectories(Setting.Instance.LevelPath)
                    .Any(directory => Path.GetFileName(directory).StartsWith(Name, StringComparison.OrdinalIgnoreCase))
                    || File.Exists($"{Setting.Instance.LevelPath}/{Name}.scene"))
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
        public void OnDownloadStarted(object? sender, DownloadStartedEventArgs eventArgs)
        {

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
        public override string ToString()
        {
            return Name;
        }

        public string Name => (this[nameof(Name)] as string)!;
        public string Author => (this[nameof(Author)] as string)!;
        public string Size => (this[nameof(Size)] as string)!;
        public string Preview 
        { 
            get => (this[nameof(Preview)] as string)!;
            set
            {
                this[nameof(Preview)] = value;
            }
        }
        public string URL => (this[nameof(URL)] as string)!;
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

        public double progressPercentage;
        public double ProgressPercentage
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
