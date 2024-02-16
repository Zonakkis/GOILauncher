using LeanCloud.Storage;
using LeanCloud;
using MapUploader.Models;

using ReactiveUI;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace MapUploader.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            LCLogger.LogDelegate = (LCLogLevel level, string info) =>
            {
                switch (level)
                {
                    case LCLogLevel.Debug:
                        Trace.WriteLine($"[DEBUG] {DateTime.Now} {info}\n");
                        break;
                    case LCLogLevel.Warn:
                        Trace.WriteLine($"[WARNING] {DateTime.Now} {info}\n");
                        break;
                    case LCLogLevel.Error:
                        Trace.WriteLine($"[ERROR] {DateTime.Now} {info}\n");
                        break;
                    default:
                        Trace.WriteLine(info);
                        break;
                }
            };
            LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com", "cB4AYnycm8sMlvK3fnAQO2On");
            LCObject.RegisterSubclass("Map", () => new Map());
            map.DownloadURL = new List<string>();
            map.Preview = "";
        }

        private Map map = new Map();

        public Map Map { get => map; set => this.RaiseAndSetIfChanged(ref map, value, "Map"); }

        public string downloadURL;
        public string DownloadURL { get => downloadURL; set => this.RaiseAndSetIfChanged(ref downloadURL, value, "DownloadURL"); }

        public void AddURL()
        {
            string url = DownloadURL.Split("//",StringSplitOptions.RemoveEmptyEntries)[1];
            map.DownloadURL.Add(url);
            this.RaisePropertyChanged("Map");
            this.RaisePropertyChanged("Map.DownloadURL");
        }

        public void Upload()
        {
            try
            {
                map.Save();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public void Reset()
        {
            map = new Map();
            map.DownloadURL = new List<string>();
            map.Preview = "";
            this.RaisePropertyChanged("Map");
            this.RaisePropertyChanged("Map.DownloadURL");
        }

    }
}
