using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using GOILauncher.Services;
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

namespace GOILauncher.ViewModels.Pages
{
    public class MapPageViewModel : PageViewModelBase
    {
        public MapPageViewModel(SettingPageViewModel settingPageModel)
        {
            SettingPage = settingPageModel;
            DownloadCommand = ReactiveCommand.Create(Download, settingPageModel.WhenAnyValue(x => x.IsLevelPathSelected));
        }
        public override void Init()
        {
            if (!Directory.Exists(Path.Combine(BaseDirectory, nameof(Download))))
            {
                Directory.CreateDirectory(Path.Combine(BaseDirectory, nameof(Download)));
            }
            LCObject.RegisterSubclass(nameof(Map), () => new Map());
            _ = GetMaps();
        }
        public override void OnSelectedViewChanged()
        {
            if (SettingPage.IsLevelPathSelected)
            {
                foreach (var map in AllMaps)
                {
                    CheckMapExists(map);
                }
                Refresh();
            }
        }

        private void CheckMapExists(Map map)
        {
            if (map.IsDownloading || SettingPage.IsLevelPathSelected) return;
            if (Directory.GetDirectories(SettingPage.LevelPath!)
                    .Any(directory => Path.GetFileName(directory)
                        .StartsWith(map.Name, StringComparison.OrdinalIgnoreCase))
                || File.Exists($"{SettingPage.LevelPath}/{map.Name}.scene"))
            {
                map.Downloaded = true;
                map.Downloadable = false;
            }
            else
            {
                map.Downloaded = false;
                map.Downloadable = true;
            }
        }
        public void OnSelectedMapChanged(Map? map)
        {
            IsSelectedMap = map is not null;
            if (IsSelectedMap)
            {
                CurrentMap = map!;
            }
        }
        public async Task GetMaps()
        {
            LCQuery<Map> query = new(nameof(Map));
            query.OrderByAscending(nameof(Map.Name));
            query.WhereEqualTo("Platform", "PC");
            var maps = await query.Find();
            foreach (var map in maps)
            {
                if (SettingPage.IsLevelPathSelected)
                {
                    CheckMapExists(map);
                }
                if (string.IsNullOrEmpty(map.Preview))
                {
                    map.Preview = "avares://GOILauncher/Assets/No Preview.png";
                }
                else
                {
                    LCFile file = new("Preview.png", new Uri(map.Preview));
                    map.Preview = file.GetThumbnailUrl((int)(19.2 * SettingPage.PreviewQuality), (int)(10.8 * SettingPage.PreviewQuality));
                }
                AllMaps.Add(map);
            }
            Refresh();
            query.OrderByDescending("updatedAt");
            query.Select("updatedAt");
            LastUpdateTime = (await query.First()).UpdatedAt.ToLongDateString();
        }
        public void Refresh()
        {
            Maps.Clear();
            foreach (var map in AllMaps)
            {
                if (HideDownloadedMap && map.Downloaded) continue;
                if (!map.Name.Contains(FilterMapName, StringComparison.InvariantCultureIgnoreCase)) continue;
                if (Form != Forms[0] && map.Form != Form) continue;
                if (Style != Styles[0] && map.Style != Style) continue;
                if (Difficulty != Difficulties[0] && map.Difficulty != Difficulty) continue;
                Maps.Add(map);
            }
        }
        public async Task Download()
        {
            if (SelectedMap == null) return;
            var map = SelectedMap;
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            await DownloadHelper.Download(
                    $"{SettingPage.DownloadPath}/{map.Name}.zip",
                    map,
                    token
                    );
            map.Status = "解压中";
            await FileService.ExtractZip($"{SettingPage.DownloadPath}/{map.Name}.zip", SettingPage.LevelPath!, SettingPage.SaveMapZip, map.OnExtractProgressChanged);
            _ = NotificationHelper.ShowNotification("下载完成", $"地图{map.Name}下载完成", InfoBarSeverity.Success);
            map.IsDownloading = false;
            map.Downloaded = true;
            Refresh();
        }
        public SettingPageViewModel SettingPage { get; }
        private static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
        [Reactive]
        public Map CurrentMap { get; set; } = new();

        [Reactive]
        public bool IsSelectedMap { get; set; }
        public ObservableCollection<Map> AllMaps { get; } = [];
        public ObservableCollection<Map> Maps { get; set; } = [];
        [Reactive]
        public string LastUpdateTime { get; set; } = "未知";

        private Map? selectedMap;
        public Map? SelectedMap
        {
            get => selectedMap;
            set
            {
                OnSelectedMapChanged(value);
                this.RaiseAndSetIfChanged(ref selectedMap, value);
            }
        }
        public ReactiveCommand<Unit, Task> DownloadCommand { get; set; }

        private bool hideDownloadedMap = true;
        public bool HideDownloadedMap
        {
            get => hideDownloadedMap;
            set
            {
                if (HideDownloadedMap == value) return;
                this.RaiseAndSetIfChanged(ref hideDownloadedMap, value);
                Refresh();
            }
        }
        private string filterMapName = string.Empty;
        public string FilterMapName
        {
            get => filterMapName;
            set
            {
                if (FilterMapName == value) return;
                this.RaiseAndSetIfChanged(ref filterMapName, value);
                Refresh();
            }
        }

        [Reactive]
        public string[] Forms { get; set; } = ["不限", "原创", "移植"];
        private string? form = "不限";
        public string? Form
        {
            get => form;
            set
            {
                if (value is null || value == Form) return;
                this.RaiseAndSetIfChanged(ref form, value);
                Refresh();
            }
        }
        [Reactive]
        public string[] Styles { get; set; } = ["不限", "挑战", "休闲"];
        private string? style = "不限";
        public string? Style
        {
            get => style;
            set
            {
                if (value is null || value == Style) return;
                this.RaiseAndSetIfChanged(ref style, value);
                Refresh();
            }
        }
        [Reactive]
        public string[] Difficulties { get; set; } = ["不限", "简单", "中等", "困难", "极难", "地狱"];

        private string? difficulty = "不限";

        public string? Difficulty
        {
            get => difficulty;
            set
            {
                if (value is null || value == Difficulty) return;
                this.RaiseAndSetIfChanged(ref difficulty, value);
                Refresh();
            }
        }
    }
}
