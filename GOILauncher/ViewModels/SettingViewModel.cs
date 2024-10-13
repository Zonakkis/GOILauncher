using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Platform.Storage;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using GOILauncher.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class SettingViewModel : ViewModelBase
    {
        public SettingViewModel()
        {

        }
        
        public async void SelectGamePath()
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "选择游戏路径",
            };
            var folder = await (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!.StorageProvider.OpenFolderPickerAsync(dialog);
            if (folder.Count > 0)
            {
                string path = folder[0].Path.ToString();
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
        public async void SelectLevelPath()
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "选择地图路径",
            };
            var folder = await (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!.StorageProvider.OpenFolderPickerAsync(dialog);
            if (folder.Count > 0)
            {
                string path = folder[0].Path.ToString();
                path = path[8..];
                LevelPath = path;
            }
        }
        public async void SelectSteamPath()
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "选择Steam路径",
            };
            var folder = await (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!.StorageProvider.OpenFolderPickerAsync(dialog);
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
        public async void SelectDownloadPath()
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "选择下载路径",
            };
            var folder = await (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!.StorageProvider.OpenFolderPickerAsync(dialog);
            if (folder.Count > 0)
            {
                string path = folder[0].Path.ToString();
                path = path[8..];
                DownloadPath = path;
            }
        }
        public string GamePath
        {
            get => Setting.Instance.gamePath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.gamePath, value,nameof(GamePath));
                Setting.Instance.Save();
            }
        }
        public string LevelPath
        {
            get => Setting.Instance.levelPath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.levelPath, value, nameof(LevelPath));
                Setting.Instance.Save();
            }
        }
        public string SteamPath
        {
            get => Setting.Instance.steamPath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.steamPath, value, nameof(SteamPath));
                Setting.Instance.Save();
            }
        }
        public string DownloadPath
        {
            get => Setting.Instance.downloadPath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.downloadPath, value, nameof(DownloadPath));
                Setting.Instance.Save();
            }
        }
        public bool SaveMapZip
        {
            get => Setting.Instance.saveMapZip;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.saveMapZip, value, nameof(SaveMapZip));
                Setting.Instance.Save();
            }
        }
        public bool NightMode
        {
            get => Setting.Instance.nightMode;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.nightMode, value, nameof(NightMode));
                Task.Run(Setting.Instance.Save);
                if(value)
                {
                    Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;
                }
                else
                {
                    Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Light;
                }
            }
        }
        public int PreviewQuality
        {
            get => Setting.Instance.previewQuality;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.previewQuality, value, nameof(PreviewQuality));
                Setting.Instance.Save();
            }
        }
    }
}
