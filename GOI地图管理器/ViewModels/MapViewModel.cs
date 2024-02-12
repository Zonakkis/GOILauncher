using Avalonia.Markup.Xaml.Templates;
using Downloader;
using GOI地图管理器.Helpers;
using GOI地图管理器.Models;
using Ionic.Zip;
using Ionic.Zlib;
using LeanCloud;
using LeanCloud.Storage;
using Microsoft.VisualBasic;
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
            gamePath = "未选择";
            if (!Directory.Exists($"{directory}Download"))
            {
                Directory.CreateDirectory($"{directory}Download");
            }
            Maps = new ObservableCollection<Map>();
            var isDownloadValid = this.WhenAnyValue(x => x.GamePath,
                                                x => x.Length > 3);
            DownloadCommand = ReactiveCommand.Create(Download, isDownloadValid);
            LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com", null);
            LCObject.RegisterSubclass("Map", () => new Map());
            GetMaps();
        }
        public override void OnSelectedViewChanged()
        {
            GamePath = Setting.Instance.gamePath;
        }
        public void OnSelectedMapChanged(Map map)
        {
            IsSelectedMap = (map != null);
            if(IsSelectedMap) 
            {
                CurrentMap = map!;
            }
        }
        public async void GetMaps()
        {
            LCQuery<Map> query = new LCQuery<Map>("Map");
            query.OrderByAscending("Name");
            ReadOnlyCollection<Map> maps = await query.Find();
            bool levelPathExisted = (gamePath != "未选择");
            foreach (Map map in maps)
            {
                LCFile file;
                if (map.Preview != null && map.Preview != "") 
                {
                    file = new LCFile("Preview.png", new Uri(map.Preview));
                }
                else
                {
                    file = new LCFile("Preview.png", new Uri("http://lc-3Dec7Zyj.cn-n1.lcfile.com/7B1JKdTscW56vNKj8LkmlzG9OEE6Ssep/No%20Image.png"));
                }
                if (levelPathExisted)
                {
                    map.CheckWhetherExisted();
                }
                map.Preview = file.GetThumbnailUrl(640, 360, 50, false, "png");
                Maps.Add(map);
                Maps.Add(map);
                Maps.Add(map);
            }
            query.OrderByDescending("updatedAt");
            LastUpdateTime = (await query.Find()).First().UpdatedAt.ToLongDateString();
            this.RaisePropertyChanged("Maps");
        }
        public void SearchMap()
        {
            Map map = Maps.ToList().Find(t => t.Name == Search);
            SelectedMap = map;
        }
        public async void Download()
        {
            Map map = SelectedMap;
            map.Downloadable= false;
            var downloadOpt = new DownloadConfiguration()
            {
                ChunkCount = 16, // file parts to download, the default value is 1
                ParallelDownload = true // download parts of the file as parallel or not. The default value is false
            };
            var downloader = new DownloadService(downloadOpt);
            downloader.DownloadStarted += map.OnDownloadStarted;
            downloader.DownloadProgressChanged += map.OnDownloadProgressChanged;
            downloader.DownloadFileCompleted += map.OnDownloadCompleted;
            if (map.DownloadURL.Count == 1)
            {
                string directUrl = await LanzouyunDownloadHelper.GetDirectURL($"https://{map.DownloadURL[0]}");
                SelectedMap.DownloadSize = await LanzouyunDownloadHelper.GetFileSize(directUrl);
                string fileName = $"{directory}Download/{map.Name}.zip";
                await downloader.DownloadFileTaskAsync(directUrl, fileName);
                map.Status = "解压中";
                await ZipHelper.ExtractMap(fileName, LevelPath, map);
            }
            else
            {
                List<string> directUrls = new List<string>();
                for (int i = 0; i < map.DownloadURL.Count; i++)
                {
                    string directUrl = await LanzouyunDownloadHelper.GetDirectURL($"https://{map.DownloadURL[i]}");
                    directUrls.Add(directUrl);
                    //Trace.WriteLine(directUrl);
                    SelectedMap.DownloadSize += await LanzouyunDownloadHelper.GetFileSize(directUrl);
                    //LanzouyunDownloadHelper.Download(directurl, $"{directory}Download/{SelectedMap.Name}.zip.{(i+1).ToString("D3")}");
                }
                for (int i = 0; i < directUrls.Count; i++)
                {
                    await downloader.DownloadFileTaskAsync(directUrls[i], $"{directory}Download/{map.Name}.zip.{(i + 1).ToString("D3")}");
                }
                map.Status = "合并中";
                await ZipHelper.CombineZipSegment($"{directory}Download", $"{directory}Download/{map.Name}.zip", $"*{map.Name}.zip.*");
                map.Status = "解压中";
                await ZipHelper.ExtractMap($"{directory}Download/{map.Name}.zip", LevelPath, map);
                
            }
            map.IsDownloading = false;
            map.Downloaded = true;
        }

        private readonly string directory = System.AppDomain.CurrentDomain.BaseDirectory;
        public Map CurrentMap
        {
            get
            {
                return this.currentMap;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this.currentMap, value, "CurrentMap");
            }
        }

        private Map currentMap;
        public bool IsSelectedMap
        {
            get
            {
                return isSelectedMap;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isSelectedMap, value, "IsSelectedMap");
            }
        }

        private bool isSelectedMap;
        public ObservableCollection<Map> Maps { get; }
        public string LastUpdateTime { get; set; }

        private Map selectedMap;
        public Map SelectedMap
        {
            get => selectedMap;
            set
            {
                OnSelectedMapChanged(value);
                this.RaiseAndSetIfChanged(ref selectedMap, value, "SelectedMap");
            }
        }

        public string search;
        public string Search
        {
            get => search;
            set
            {
                this.RaiseAndSetIfChanged(ref search, value, "Search");
            }
        }
        public ReactiveCommand<Unit, Unit> DownloadCommand { get; }

        private bool selectedGamePathNoteHide;
        public bool SelectedGamePathNoteHide
        {
            get => selectedGamePathNoteHide;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedGamePathNoteHide, value, "UnSelectedGamePathNoteHide");
            }
        }

        public string gamePath;
        public string GamePath
        {
            get => gamePath;
            set
            {
                if (gamePath != value)
                {
                    this.RaiseAndSetIfChanged(ref gamePath, value, "GamePath");
                    SelectedGamePathNoteHide = true; 
                    foreach (var map in Maps)
                    {
                        map.CheckWhetherExisted();
                    }
                }
            }
        }
        public string LevelPath
        {
            get => Setting.Instance.levelPath; 
        }

    }
}
