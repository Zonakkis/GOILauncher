using Avalonia.Markup.Xaml.Templates;
using Downloader;
using GOI地图管理器.Helpers;
using GOI地图管理器.Models;
using Ionic.Zip;
using Ionic.Zlib;
using LeanCloud;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        
        public MapViewModel()
        {
            if (File.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                //Setting = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                Setting setting = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                GamePath = setting.gamePath;
                LevelPath = setting.levelPath;
            }
            else
            {
                //Setting = new Setting();
                GamePath = "未选择";
            }
            if (!Directory.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Download"))
            {
                Directory.CreateDirectory($"{System.AppDomain.CurrentDomain.BaseDirectory}Download");
            }
            Maps = new ObservableCollection<Map>();
            var isDownloadValid = this.WhenAnyValue(x => x.GamePath,
                                                x => x.Length > 3);
            isDownloadValid.Subscribe(x => UnSelectedGamePathNoteHide = x);
            DownloadCommand = ReactiveCommand.Create(Download, isDownloadValid);
            LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com", null);
            GetMaps();
        }

        public override void OnSelectedViewModelChanged()
        {
            if (GamePath == "未选择" && File.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                Setting setting = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                GamePath = setting.gamePath;
                LevelPath = setting.levelPath;
            }
        }
        public async void GetMaps()
        {
            LCQuery<LCObject> query = new LCQuery<LCObject>("Maps");
            query.OrderByAscending("Name");
            ReadOnlyCollection<LCObject> maps = await query.Find();
            foreach (LCObject map in maps)
            {
                this.Maps.Add(new Map(map));
            }
        }

        public async void Download()
        {
            Map map = SelectedMap;
            map.Downloadable= false;
            List<object> urls = map!.MapObject!["DownloadURL"] as List<object>;
            var downloadOpt = new DownloadConfiguration()
            {
                ChunkCount = 16, // file parts to download, the default value is 1
                ParallelDownload = true // download parts of the file as parallel or not. The default value is false
            };
            var downloader = new DownloadService(downloadOpt);
            downloader.DownloadStarted += map.OnDownloadStarted;
            downloader.DownloadProgressChanged += map.OnDownloadProgressChanged;
            downloader.DownloadFileCompleted += map.OnDownloadCompleted;
            if (urls!.Count == 1)
            {
                string directUrl = await LanzouyunDownloadHelper.GetDirectURL($"https://{(string)urls[0]}");
                SelectedMap.DownloadSize = LanzouyunDownloadHelper.GetFileSize(directUrl);
                string fileName = $"{System.AppDomain.CurrentDomain.BaseDirectory}Download/{map.Name}.zip";
                await downloader.DownloadFileTaskAsync(directUrl, fileName);
                map.Status = "解压中";
                using (ZipFile zip = new ZipFile(fileName))
                {
                    zip.ExtractProgress += map.OnExtractProgressChanged;
                    foreach (ZipEntry entry in zip)
                    {
                        await Task.Run(() => entry.Extract(LevelPath));
                    }
                }
            }
            else
            {
                List<string> directUrls = new List<string>();
                for (int i = 0; i < urls.Count; i++)
                {
                    string directUrl = await LanzouyunDownloadHelper.GetDirectURL($"https://{(string)urls[i]}");
                    directUrls.Add(directUrl);
                    //Trace.WriteLine(directUrl);
                    SelectedMap.DownloadSize += LanzouyunDownloadHelper.GetFileSize(directUrl);
                    //LanzouyunDownloadHelper.Download(directurl, $"{System.AppDomain.CurrentDomain.BaseDirectory}Download/{SelectedMap.Name}.zip.{(i+1).ToString("D3")}");
                }
                for (int i = 0; i < directUrls.Count; i++)
                {
                    await downloader.DownloadFileTaskAsync(directUrls[i], $"{System.AppDomain.CurrentDomain.BaseDirectory}Download/{map.Name}.zip.{(i + 1).ToString("D3")}");
                }
                map.Status = "解压中";
                string realZip = $"{System.AppDomain.CurrentDomain.BaseDirectory}Download/{map.Name}.zip";
                List<string> zipFiles = Directory.GetFiles($"{System.AppDomain.CurrentDomain.BaseDirectory}Download", $"*{map.Name}.zip.*").ToList();
                using (Stream zipStream = File.OpenWrite(realZip))
                {
                    for (int i = 0; i < zipFiles.Count; i++)
                    {
                        using (Stream stream = File.OpenRead(zipFiles[i]))
                        {
                            stream.CopyTo(zipStream);
                        }
                        File.Delete(zipFiles[i]);
                    }
                }
                using (ZipFile zip = ZipFile.Read(realZip))
                {
                    zip.ExtractProgress += map.OnExtractProgressChanged;
                    foreach (ZipEntry entry in zip)
                    {
                        await Task.Run(() => entry.Extract(LevelPath));
                    }
                }
            }
            map.IsDownloading = false;
        }

        public void OnSelectedMapChanged(Map value)
        {
            if(!value.IsLoaded)
            {
                value.Load();
            }
            this.IsSelected = true;
            if(File.Exists($"{LevelPath}/{value.Name}.scene"))
            {
                value.Downloadable = false;
            }
            this.CurrentMap = value;
            
        }

        public Map CurrentMap
        {
            get
            {
                return this._currentMap;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this._currentMap, value, "CurrentMap");
            }
        }

        private Map _currentMap;
        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this._isSelected, value, "IsSelected");
            }
        }

        private bool _isSelected;
        public ObservableCollection<Map> Maps { get; }
        public Map SelectedMap
        {
            get
            {
                return this._selectedMap;
            }
            set
            {
                this.OnSelectedMapChanged(value);
                this.RaiseAndSetIfChanged(ref this._selectedMap, value, "SelectedMap");
            }
        }

        private Map _selectedMap;

        public ReactiveCommand<Unit, Unit> DownloadCommand { get; }



        private bool unSelectedGamePathNoteHide;

        public bool UnSelectedGamePathNoteHide
        {
            get => unSelectedGamePathNoteHide;
            set
            {
                this.RaiseAndSetIfChanged(ref unSelectedGamePathNoteHide, value, "UnSelectedGamePathNoteHide");
            }
        }

        //public Setting setting;
        //public Setting Setting
        //{
        //    get => setting;
        //    set
        //    {
        //        this.RaiseAndSetIfChanged(ref setting, value,"Setting");
        //    }
        //}

        public string gamePath;

        public string GamePath
        {
            get => gamePath;
            set
            {
                this.RaiseAndSetIfChanged(ref gamePath, value,"GamePath");
            }
        }

        public string LevelPath { get; set; }
    }
}
