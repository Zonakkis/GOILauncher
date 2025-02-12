using Avalonia.Platform.Storage;
using FluentAvalonia.UI.Controls;
using GOILauncher.Models;
using ReactiveUI;
using System.IO;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
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
        private static Setting Setting => Setting.Instance;
        public string GamePath
        {
            get => Setting.GamePath;
            set
            {
                Setting.GamePath = value;
                this.RaisePropertyChanged();
            }
        }
        public string LevelPath
        {
            get => Setting.LevelPath;
            set
            {
                Setting.LevelPath = value;
                this.RaisePropertyChanged();
            }
        }
        public string SteamPath
        {
            get => Setting.SteamPath;
            set
            {
                Setting.SteamPath = value;
                this.RaisePropertyChanged();
            }
        }
        public string DownloadPath
        {
            get => Setting.DownloadPath;
            set
            {
                Setting.DownloadPath = value;
                this.RaisePropertyChanged();
            }
        }
        public bool SaveMapZip
        {
            get => Setting.SaveMapZip;
            set
            {
                Setting.SaveMapZip = value;
                this.RaisePropertyChanged();
            }
        }
        public bool NightMode
        {
            get => Setting.NightMode;
            set
            {
                Setting.NightMode = value;
                this.RaisePropertyChanged();
            }
        }
        public int PreviewQuality
        {
            get => Setting.PreviewQuality;
            set
            {
                Setting.PreviewQuality = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
