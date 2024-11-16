using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Threading;
using Downloader;
using DynamicData;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using Ionic.Zip;
using Ionic.Zlib;
using LeanCloud;
using LeanCloud.Storage;
using Microsoft.VisualBasic;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
using static System.Formats.Asn1.AsnWriter;

namespace GOILauncher.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        public MapViewModel()
        {
            hideDownloadedMap = true;
            SelectedLevelPathNoteHide = LevelPath != "未选择（选择游戏路径后自动选择，也可手动更改）";
            Setting.LevelPathChanged += () =>
            {
                SelectedLevelPathNoteHide = true;
                this.RaisePropertyChanged(nameof(LevelPath));
            };
            var isApplyFilterSettingValid = this.WhenAnyValue(x => x.FilterSettingChanged,
                                                x => x == true);
            ApplyFilterSettingCommand = ReactiveCommand.Create(ApplyFilterSetting, isApplyFilterSettingValid);
            var isDownloadValid = this.WhenAnyValue(x => x.LevelPath,
                    x => x != "未选择（选择游戏路径后自动选择，也可手动更改）");
            DownloadCommand = ReactiveCommand.Create(Download, isDownloadValid);
        }
        public override void Init()
        {
            if (!Directory.Exists($"{directory}Download"))
            {
                Directory.CreateDirectory($"{directory}Download");
            }
            Task.Run(() =>
            {
                LCObject.RegisterSubclass("Map", () => new Map());
                GetMaps();
            });
            Forms = ["不限", "原创", "移植"];
            Form = Forms[0];
            Styles = ["不限","挑战", "休闲"];
            Style = Styles[0];
            Difficulties = ["不限","地狱", "极难", "困难", "中等", "简单"];
            Difficulty = Difficulties[0];
            FilterSettingChanged = false;
        }
        public override void OnSelectedViewChanged()
        {
            foreach (var map in AllMaps)
            {
                map.CheckWhetherExisted();
            }
            RefreshMapList();
        }
        public void OnSelectedMapChanged(Map map)
        {
            IsSelectedMap = (map != null);
            if (IsSelectedMap)
            {
                CurrentMap = map!;
            }
        }
        public async void GetMaps()
        {
            LCQuery<Map> query = new("Map");
            query.OrderByAscending("Name");
            ReadOnlyCollection<Map> maps = await query.Find();
            bool levelPathExisted = (LevelPath != "未选择（选择游戏路径后自动选择，也可手动更改）");
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
                map.Preview = file.GetThumbnailUrl((int)(19.2 * PreviewQuality), (int)(10.8 * PreviewQuality));
                AllMaps.Add(map);
            }
            await Task.Run(RefreshMapList);
            query.OrderByDescending("updatedAt");
            query.Select("updatedAt");
            LastUpdateTime = (await query.Find()).First().UpdatedAt.ToLongDateString();
            this.RaisePropertyChanged(nameof(LastUpdateTime));
        }
        public void ApplyFilterSetting()
        {
            FilterSettingChanged = false;
            RefreshMapList();
        }
        public void RefreshMapList()
        {
            MapList = [];
            foreach (var map in AllMaps)
            {
                if (HideDownloadedMap && map.Downloaded)
                {
                    continue;
                }
                if (Form != Forms[0] && map.Form != Form)
                {
                    continue;
                }
                if (Style != Styles[0] && map.Style != Style) continue;
                if (Difficulty != Difficulties[0] && map.Difficulty != Difficulty) continue;
                MapList.Add(map);
            }
            this.RaisePropertyChanged(nameof(MapList));
        }
        public void SearchMap()
        {
            Map map = MapList.ToList().Find(t => t.Name == Search);
            SelectedMap = map;
        }
        public async void Download()
        {
            Map map = SelectedMap;
            map.Downloadable = false;
            CancellationTokenSource tokenSource = new();
            CancellationToken token = tokenSource.Token;
            DownloadService[] downloader = new DownloadService[map.DownloadURL.Count];
            map.Status = "获取下载地址中";
            map.IsDownloading = true;
            map.DirectURLs = new string[map.DownloadURL.Count];
            map.ReceivedBytes = new long[map.DownloadURL.Count];
            map.DownloadSpeeds = new double[map.DownloadURL.Count];
            for (int i = 0; i < map.DownloadURL.Count; i++)
            {
                map.DirectURLs[i] = await LanzouyunDownloadHelper.GetDirectURLAsync(map.DownloadURL[i]);
            }
            for (int i = 0; i < map.DownloadURL.Count; ++i)
            {
                map.DownloadTasks.Add(LanzouyunDownloadHelper.Download(
                    map.DirectURLs[i],
                    $"{DownloadPath}/{map.Name}.zip.{(i + 1):000}",
                    map.OnDownloadStarted,
                    map.OnDownloadProgressChanged,
                    map.OnDownloadCompleted,
                    token
                    ));
            }
            map.Status = "启动下载中";
            map.DownloadTasks.Add(map.WaitForDownloadFinish());
            await Task.WhenAll(map.DownloadTasks);
            map.Status = "合并中";
            await ZipHelper.CombineZipSegment($"{DownloadPath}", $"{DownloadPath}/{map.Name}.zip", $"*{map.Name}.zip.*");
            map.Status = "解压中";
            await ZipHelper.Extract($"{DownloadPath}/{map.Name}.zip", LevelPath, map.OnExtractProgressChanged);
            NotificationHelper.ShowNotification("下载完成", $"地图{map.Name}下载完成", InfoBarSeverity.Success);
            map.IsDownloading = false;
            map.Downloaded = true;
            await Task.Run(RefreshMapList);
        }

        private readonly string directory = System.AppDomain.CurrentDomain.BaseDirectory;
        [Reactive]
        public Map CurrentMap{ get; set; }
        [Reactive]
        public bool IsSelectedMap { get; set; }
        public ObservableCollection<Map> AllMaps { get; } = [];
        public ObservableCollection<Map> MapList { get; set; } = [];
        public string LastUpdateTime { get; set; }

        private Map selectedMap;
        [Reactive]
        public Map SelectedMap
        {
            get => selectedMap;
            set
            {
                OnSelectedMapChanged(value);
                this.RaiseAndSetIfChanged(ref selectedMap, value, nameof(SelectedMap));
            }
        }
        [Reactive]
        public string Search { get; set; }
        public ReactiveCommand<Unit, Unit> DownloadCommand { get; set; }
        [Reactive]
        public bool SelectedLevelPathNoteHide { get; set; }
        private Setting Setting { get; } = Setting.Instance;
        private string LevelPath => Setting.LevelPath;
        private string DownloadPath => Setting.DownloadPath;
        private int PreviewQuality => Setting.PreviewQuality;

        private bool hideDownloadedMap;
        public bool HideDownloadedMap
        {
            get => hideDownloadedMap;
            set
            {
                FilterSettingChanged = true;
                this.RaiseAndSetIfChanged(ref hideDownloadedMap, value, nameof(HideDownloadedMap));
            }
        }
        [Reactive]
        public bool FilterSettingChanged { get; set; }
        public ReactiveCommand<Unit, Unit> ApplyFilterSettingCommand { get; set; }
        [Reactive]
        public string[] Forms { get; set; }
        [Reactive]
        public string Form { get; set; }
        [Reactive]
        public string[] Styles { get; set; }
        [Reactive]
        public string Style { get; set; }
        [Reactive]
        public string[] Difficulties { get; set; }

        private string difficulty;
        public string Difficulty
        {
            get
            {
                return difficulty;
            }
            set
            {
                FilterSettingChanged = true;
                this.RaiseAndSetIfChanged(ref difficulty, value, nameof(Difficulty));
            }
        }
    }
}
