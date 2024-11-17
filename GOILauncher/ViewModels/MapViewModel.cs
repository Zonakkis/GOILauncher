﻿using Avalonia.Threading;
using DynamicData;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        public MapViewModel()
        {
            CurrentMap = new();
            hideDownloadedMap = true;
            SelectedLevelPathNoteHide = LevelPath != "未选择（选择游戏路径后自动选择，也可手动更改）";
            Setting.LevelPathChanged += () =>
            {
                SelectedLevelPathNoteHide = true;
                this.RaisePropertyChanged(nameof(LevelPath));
            };
            var isApplyFilterSettingValid = this.WhenAnyValue(x => x.FilterSettingChanged);
            ApplyFilterSettingCommand = ReactiveCommand.Create(ApplyFilterSetting, isApplyFilterSettingValid);
            var isDownloadValid = this.WhenAnyValue(x => x.LevelPath,
                    x => x != "未选择（选择游戏路径后自动选择，也可手动更改）");
            DownloadCommand = ReactiveCommand.Create(Download, isDownloadValid);
            Forms = ["不限", "原创", "移植"];
            Form = Forms[0];
            Styles = ["不限", "挑战", "休闲"];
            Style = Styles[0];
            Difficulties = ["不限", "地狱", "极难", "困难", "中等", "简单"];
            Difficulty = Difficulties[0];
            LastUpdateTime = "未知";
            Search = string.Empty;
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
            await RefreshMapList();
            query.OrderByDescending("updatedAt");
            query.Select("updatedAt");
            LastUpdateTime = (await query.Find()).First().UpdatedAt.ToLongDateString();
        }
        public async void ApplyFilterSetting()
        {
            FilterSettingChanged = false;
            await RefreshMapList();
        }
        public async Task RefreshMapList()
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                MapList.Clear();
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
            });
        }
        public void SearchMap()
        {
            Map? map = MapList.ToList().Find(t => t.Name == Search);
            SelectedMap = map;
        }
        public async void Download()
        {

            if (SelectedMap == null) return;
            Map map = SelectedMap;
            map.Downloadable = false;
            CancellationTokenSource tokenSource = new();
            CancellationToken token = tokenSource.Token;
            map.IsDownloading = true;
            map.Status = "启动下载中";
            await DownloadHelper.Download(
                    map.URL, $"{DownloadPath}/{map.Name}.zip",
                    map.OnDownloadStarted,
                    map.OnDownloadProgressChanged,
                    map.OnDownloadCompleted,
                    token
                    );
            map.Status = "解压中";
            await ZipHelper.Extract($"{DownloadPath}/{map.Name}.zip", LevelPath, map.OnExtractProgressChanged);
            NotificationHelper.ShowNotification("下载完成", $"地图{map.Name}下载完成", InfoBarSeverity.Success);
            map.IsDownloading = false;
            map.Downloaded = true;
            await RefreshMapList();
        }

        private readonly string directory = System.AppDomain.CurrentDomain.BaseDirectory;
        [Reactive]
        public Map CurrentMap{ get; set; }
        [Reactive]
        public bool IsSelectedMap { get; set; }
        public ObservableCollection<Map> AllMaps { get; } = [];
        public ObservableCollection<Map> MapList { get; set; } = [];

        public SourceList<Map> Maps { get; set; } = new();
        [Reactive]
        public string LastUpdateTime { get; set; }

        private Map? selectedMap;
        public Map? SelectedMap
        {
            get => selectedMap;
            set
            {
                OnSelectedMapChanged(value!);
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
                if (HideDownloadedMap == value) return;
                FilterSettingChanged = true;
                this.RaiseAndSetIfChanged(ref hideDownloadedMap, value, nameof(HideDownloadedMap));
            }
        }
        [Reactive]
        public bool FilterSettingChanged { get; set; }
        public ReactiveCommand<Unit, Unit> ApplyFilterSettingCommand { get; set; }
        [Reactive]
        public string[] Forms { get; set; }
        public string form = "不限";
        public string Form
        {
            get => form;
            set
            {
                if (value is not string val || val == Form) return;
                FilterSettingChanged = true;
                this.RaiseAndSetIfChanged(ref form, value, nameof(Form));
            }
        }
        [Reactive]
        public string[] Styles { get; set; }
        private string style = "不限";
        public string Style
        {
            get => style;
            set
            {
                if (value is not string val || val == Style) return;
                FilterSettingChanged = true;
                this.RaiseAndSetIfChanged(ref style, value, nameof(Style));
            }
        }
        [Reactive]
        public string[] Difficulties { get; set; }

        private string difficulty = "不限";

        public string Difficulty
        {
            get => difficulty;
            set
            {
                if (value is not string val || val == Difficulty) return;
                FilterSettingChanged = true;
                this.RaiseAndSetIfChanged(ref difficulty, value, nameof(Difficulty));
            }
        }
    }
}
