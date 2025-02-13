using System;
using Avalonia.Platform.Storage;
using FluentAvalonia.UI.Controls;
using GOILauncher.Models;
using GOILauncher.Services;
using ReactiveUI;
using System.IO;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        public SettingViewModel()
        {
            this.WhenAnyValue(x=>x.GamePath, x => x.LevelPath, x => x.SteamPath, x => x.DownloadPath, x => x.SaveMapZip, x => x.NightMode, x => x.PreviewQuality)
                .Subscribe(_ => SettingService.SaveSetting(_setting));
        }
        public async Task SelectGamePath()
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "选择游戏路径",
            };
            var folder = await App.TopLevel!.StorageProvider.OpenFolderPickerAsync(dialog);
            if (folder.Count > 0)
            {
                var path = folder[0].Path.ToString();
                path = path[8..];
                if (File.Exists($"{path}/GettingOverIt.exe"))
                {
                    GamePath = path;
                    LevelPath = $"{path}/Levels";
                }
                else
                {
                    var contentDialog = new ContentDialog()
                    {
                        Title = "提示",
                        Content = "没有找到GettingOverIt.exe，是不是找错路径了喵？",
                        CloseButtonText = "好的",
                    };
                    await contentDialog.ShowAsync();
                }
            }
        }
        public async Task SelectLevelPath()
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "选择地图路径",
            };
            var folder = await App.TopLevel!.StorageProvider.OpenFolderPickerAsync(dialog);
            if (folder.Count > 0)
            {
                var path = folder[0].Path.ToString();
                path = path[8..];
                LevelPath = path;
            }
        }
        public async Task SelectSteamPath()
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "选择Steam路径",
            };
            var folder = await App.TopLevel!.StorageProvider.OpenFolderPickerAsync(dialog);
            if (folder.Count > 0)
            {
                string path = folder[0].Path.ToString();
                path = path[8..];
                if (File.Exists($"{path}/steam.exe"))
                {
                    SteamPath = path;
                }
                else
                {
                    var contentDialog = new ContentDialog()
                    {
                        Title = "提示",
                        Content = "没有找到steam.exe，是不是找错路径了喵？",
                        CloseButtonText = "好的",
                    };
                    await contentDialog.ShowAsync();
                }
            }
        }
        public async Task SelectDownloadPath()
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "选择下载路径",
            };
            var folder = await App.TopLevel!.StorageProvider.OpenFolderPickerAsync(dialog);
            if (folder.Count > 0)
            {
                var path = folder[0].Path.ToString();
                path = path[8..];
                DownloadPath = path;
            }
        }
        private readonly Setting _setting = SettingService.LoadSetting();
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
    }
}
