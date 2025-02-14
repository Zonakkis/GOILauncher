using System;
using Avalonia.Platform.Storage;
using FluentAvalonia.UI.Controls;
using GOILauncher.Models;
using GOILauncher.Services;
using ReactiveUI;
using System.IO;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using FluentAvalonia.UI.Windowing;
using GOILauncher.UI;

namespace GOILauncher.ViewModels.Pages
{
    public class SettingPageViewModel : PageViewModelBase
    {
        public SettingPageViewModel(FileService fileService)
        {
            _fileService = fileService;
            this.WhenAnyValue(x => x.GamePath)
                .Subscribe(x => IsGamePathSelected = !string.IsNullOrEmpty(x));
            this.WhenAnyValue(x => x.LevelPath)
                .Subscribe(x => IsLevelPathSelected = !string.IsNullOrEmpty(x));
            this.WhenAnyValue(x => x.SteamPath)
                .Subscribe(x => IsSteamPathSelected = !string.IsNullOrEmpty(x));
            this.WhenAnyValue(x => x.GamePath, x => x.LevelPath, x => x.SteamPath, x => x.DownloadPath, x => x.SaveMapZip, x => x.NightMode, x => x.PreviewQuality)
                .Subscribe(_ => SettingService.SaveSetting(_setting));
        }
        public async Task SelectGamePath()
        {
            var folder = await _fileService.OpenFolderPickerAsync("选择游戏路径");
            if (folder is not null)
            {
                var path = folder.Path.ToString()[8..];
                if (File.Exists($"{path}/GettingOverIt.exe"))
                {
                    GamePath = path;
                    LevelPath = $"{path}/Levels";
                }
                else
                {
                    NotificationManager.ShowContentDialog("提示",
                        "没有找到GettingOverIt.exe，是不是找错路径了喵？");
                }
            }
        }
        public async Task SelectLevelPath()
        {
            var folder = await _fileService.OpenFolderPickerAsync("选择地图路径");
            if (folder is not null)
            {
                var path = folder.Path.ToString()[8..];
                LevelPath = path;
            }
        }
        public async Task SelectSteamPath()
        {
            var folder = await _fileService.OpenFolderPickerAsync("选择Steam路径");
            if (folder is not null)
            {
                var path = folder.Path.ToString()[8..];
                if (File.Exists($"{path}/steam.exe"))
                {
                    SteamPath = path;
                }
                else
                {
                    NotificationManager.ShowContentDialog("提示",
                        "没有找到steam.exe，是不是找错路径了喵？");
                }
            }
        }
        public async Task SelectDownloadPath()
        {
            var folder = await _fileService.OpenFolderPickerAsync("选择下载路径");
            if (folder is not null)
            {
                var path = folder.Path.ToString()[8..];
                DownloadPath = path;
            }
        }
        private readonly Setting _setting = SettingService.LoadSetting();
        private readonly FileService _fileService;

        public string? GamePath
        {
            get => _setting.GamePath;
            set
            {
                _setting.GamePath = value;
                this.RaisePropertyChanged();
            }
        }
        public string? LevelPath
        {
            get => _setting.LevelPath;
            set
            {
                _setting.LevelPath = value;
                this.RaisePropertyChanged();
            }
        }
        public string? SteamPath
        {
            get => _setting.SteamPath;
            set
            {
                _setting.SteamPath = value;
                this.RaisePropertyChanged();
            }
        }
        public string? DownloadPath
        {
            get => _setting.DownloadPath;
            set
            {
                _setting.DownloadPath = value;
                this.RaisePropertyChanged();
            }
        }
        public bool SaveMapZip
        {
            get => _setting.SaveMapZip;
            set
            {
                _setting.SaveMapZip = value;
                this.RaisePropertyChanged();
            }
        }
        public bool NightMode
        {
            get => _setting.NightMode;
            set
            {
                _setting.NightMode = value;
                this.RaisePropertyChanged();
            }
        }
        public int PreviewQuality
        {
            get => _setting.PreviewQuality;
            set
            {
                _setting.PreviewQuality = value;
                this.RaisePropertyChanged();
            }
        }
        [Reactive]
        public bool IsGamePathSelected { get; set; }
        [Reactive]
        public bool IsLevelPathSelected { get; set; }
        [Reactive]
        public bool IsSteamPathSelected { get; set; }
    }
}
