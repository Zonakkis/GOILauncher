using Avalonia.Threading;
using Downloader;
using FluentAvalonia.UI.Controls;
using GOILauncher.Infrastructures.Interfaces;
using GOILauncher.Infrastructures.LeanCloud;
using GOILauncher.Models;
using GOILauncher.Services;
using GOILauncher.UI;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels.Pages
{
    public class MapPageViewModel : PageViewModelBase
    {
        public MapPageViewModel(GameService gameService, ILeanCloud LeanCloud,
            AppService appService, DownloadConfiguration downloadConfiguration,
            NotificationManager notificationManager)
        {
            _gameService = gameService;
            _leanCloud = LeanCloud;
            _downloadConfiguration = downloadConfiguration;
            _notificationManager = notificationManager;
            Setting = appService.Setting;
            this.WhenAnyValue(x => x.HideDownloadedMap, x => x.FilterMapName, x => x.Form, x => x.Style, x => x.Difficulty)
                .Subscribe(_ => Filter());
            Setting.WhenAnyValue(x => x.LevelPath)
                   .Where(x => !string.IsNullOrEmpty(x))
                   .Subscribe(x => gameService.SetLevelPath(x!));
        }
        public override void Init()
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), nameof(Download))))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), nameof(Download)));
            }
            Dispatcher.UIThread.InvokeAsync(GetMaps);
        }
        private async Task GetMaps()
        {
            var query = new LeanCloudQuery<Map>()
                            .OrderByAscending("name")
                            .Where("platform", "PC")
                            .Select("name", "author", "size", "preview",
                                    "url", "difficulty", "form", "style");
            foreach (var map in await _leanCloud.Find(query))
            {
                if(!string.IsNullOrEmpty(map.Preview))
                {
                    map.Preview = $"{map.Preview}?imageView/2/w/{(int)(19.2*Setting.PreviewQuality)}/h/{(int)(10.8*Setting.PreviewQuality)}/q/{Setting.PreviewQuality}";
                }
                map.DownloadCommand = ReactiveCommand.CreateFromTask<Map>(Download);
                Maps.Add(map);
                var downloadService = new DownloadService(_downloadConfiguration);
                downloadService.DownloadStarted += map.OnDownloadStarted;
                downloadService.DownloadProgressChanged += map.OnDownloadProgressChanged;
                downloadService.DownloadFileCompleted += map.OnDownloadCompleted;
                downloadServices.Add(map, downloadService);
            }
            Setting.WhenAnyValue(x => x.LevelPath)
                   .Where(x => !string.IsNullOrEmpty(x))
                   .Subscribe(_ =>
                   {
                       foreach (var map in Maps.Where(map => _gameService.CheckWhetherMapExists(map)))
                       {
                           _gameService.AddMap(map);
                           map.Downloaded = true;
                           map.Downloadable = false;
                       }
                       Filter();
                   });
        }
        private void Filter()
        {
            FilteredMaps.Clear();
            foreach (var map in Maps)
            {
                if (HideDownloadedMap && map.Downloaded) continue;
                if (!map.Name.Contains(FilterMapName, StringComparison.InvariantCultureIgnoreCase)) continue;
                if (Form != Forms[0] && map.Form != Form) continue;
                if (Style != Styles[0] && map.Style != Style) continue;
                if (Difficulty != Difficulties[0] && map.Difficulty != Difficulty) continue;
                FilteredMaps.Add(map);
            }
        }
        public async Task Download(Map map)
        {
            try
            {
                var downloadService = downloadServices[map];
                await downloadService.DownloadFileTaskAsync(map.Url,
                    Path.Combine(Setting.DownloadPath!, $"{map.Name}.zip"));
                await FileService.ExtractZipAsync(Path.Combine(Setting.DownloadPath!, $"{map.Name}.zip"),
                Setting.LevelPath!,
                !Setting.SaveMapZip);
                map.IsExtracting = false;
                map.Downloaded = true;
                if(HideDownloadedMap)
                {
                    FilteredMaps.Remove(map);
                }
                _gameService.AddMap(map);
                _notificationManager.AddNotification("成功", $"地图{map.Name}下载完成！", InfoBarSeverity.Success);
            }
            catch (Exception exception)
            {
                _notificationManager.AddNotification("错误", $"地图{map.Name}下载失败：{exception.Message}", InfoBarSeverity.Error);
            }
        }
        private readonly GameService _gameService;
        private readonly ILeanCloud _leanCloud;
        private readonly NotificationManager _notificationManager;
        private readonly DownloadConfiguration _downloadConfiguration;
        public Setting Setting { get; }
        private readonly Dictionary<Map, DownloadService> downloadServices = [];
        public ObservableCollection<Map> Maps { get; } = [];
        public ObservableCollection<Map> FilteredMaps { get; } = [];
        [Reactive]
        public Map SelectedMap { get; set; }
        [Reactive]
        public bool HideDownloadedMap { get; set; } = true;
        [Reactive]
        public string FilterMapName { get; set; } = string.Empty;
        public string[] Forms { get; } = ["不限", "原创", "移植"];
        [Reactive]
        public string? Form { get; set; } = "不限";
        public string[] Styles { get; } = ["不限", "挑战", "休闲"];
        [Reactive]
        public string Style { get; set; } = "不限";
        public string[] Difficulties { get; } = ["不限", "简单", "中等", "困难", "极难", "地狱"];
        [Reactive]
        public string Difficulty { get; set; } = "不限";
    }
}
