using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using Downloader;
using GOILauncher.Helpers;
using GOILauncher.Models;
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
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class DesignMapViewModel : ViewModelBase
    {
        public DesignMapViewModel()
        {
            if (!Directory.Exists($"{directory}Download"))
            {
                Directory.CreateDirectory($"{directory}Download");
            }
            var isDownloadValid = this.WhenAnyValue(x => x.LevelPath,
                                                x => x[0] != '未');
            DownloadCommand = ReactiveCommand.Create(Download, isDownloadValid);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com", null);
            LCObject.RegisterSubclass("Map", () => new Map());
            GetMaps();
        }
        public override void OnSelectedViewChanged()
        {
            LevelPath = Setting.Instance.levelPath;
            foreach (var map in Maps)
            {
                map.CheckWhetherExisted();
            }
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
            bool levelPathExisted = (LevelPath != "未选择");
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
            }
            query.OrderByDescending("updatedAt");
            LastUpdateTime = (await query.Find()).First().UpdatedAt.ToLongDateString();
            this.RaisePropertyChanged("Maps");
            this.RaisePropertyChanged("LastUpdateTime");
        }
        public void SearchMap()
        {
            Map map = Maps.ToList().Find(t => t.Name == Search);
            SelectedMap = map;
        }
        public async void Download()
        {
            Map map = SelectedMap;
            map.Downloadable = false;
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            DownloadService[] downloader = new DownloadService[map.DownloadURL.Count];
            List<string> directUrls = new List<string>();
            map.Status = "获取下载地址中";
            map.IsDownloading = true;
            map.ReceivedBytes = new long[map.DownloadURL.Count];
            map.DownloadSpeeds = new double[map.DownloadURL.Count];
            for (int i = 0; i < map.DownloadURL.Count; i++)
            {
                //downloader[i] = new DownloadService(downloadOpt);
                //downloader[i].DownloadStarted += map.OnDownloadStarted;
                //downloader[i].DownloadProgressChanged += map.OnDownloadProgressChanged;
                //downloader[i].DownloadFileCompleted += map.OnDownloadCompleted;
                //map.TotalBytes.Add(0);
                //map.ReceivedBytes.Add(0);
                //map.DownloadSpeeds.Add(0);

                string directUrl = await LanzouyunDownloadHelper.GetDirectURLAsync($"https://{map.DownloadURL[i]}");
                directUrls.Add(directUrl);
                //Trace.WriteLine(directUrl);
                //LanzouyunDownloadHelper.Download(directurl, $"{DownloadPath}/{SelectedMap.Name}.zip.{(i+1).ToString("D3")}");
            }
            map.Status = "启动下载中";
            for (int i = 0; i < directUrls.Count; i++)
            {
                map.DownloadTasks.Add(LanzouyunDownloadHelper.Download(
                    directUrls[i],
                    $"{DownloadPath}/{map.Name}.zip.{(i + 1).ToString("D3")}",
                    map.OnDownloadStarted,
                    map.OnDownloadProgressChanged,
                    map.OnDownloadCompleted,
                    token
                    ));

                //map.DownloadTasks.Add(downloader[i].DownloadFileTaskAsync(directUrls[i], $"{DownloadPath}/{map.Name}.zip.{(i + 1).ToString("D3")}", token));
            }
            try
            {
                map.DownloadTasks.Add(map.WaitForDownloadFinish());
                await Task.WhenAll(map.DownloadTasks);
            }
            catch (ArgumentException ex)
            {
                Trace.WriteLine(ex.Message);
            }
            map.Status = "合并中";
            await ZipHelper.CombineZipSegment($"{DownloadPath}", $"{DownloadPath}/{map.Name}.zip", $"*{map.Name}.zip.*");
            map.Status = "解压中";
            await ZipHelper.Extract($"{DownloadPath}/{map.Name}.zip", LevelPath,map.OnExtractProgressChanged);
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
        public ObservableCollection<Map> Maps { get; } = new ObservableCollection<Map>();
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

        private bool selectedLevelPathNoteHide;
        public bool SelectedLevelPathNoteHide
        {
            get => selectedLevelPathNoteHide;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedLevelPathNoteHide, value, "SelectedLevelPathNoteHide");
            }
        }

        public string levelPath = "未选择";
        public string LevelPath
        {
            get => Setting.Instance.levelPath;
            set
            {
                if (value[0] != '未')
                {
                    this.RaiseAndSetIfChanged(ref levelPath, value, "LevelPath");
                    SelectedLevelPathNoteHide = true;
                }
            }
        }
        public string DownloadPath
        {
            get => Setting.Instance.downloadPath;
        }


    }
}
