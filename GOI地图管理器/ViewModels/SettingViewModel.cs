using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Platform.Storage;
using GOI地图管理器.Helpers;
using GOI地图管理器.Models;
using GOI地图管理器.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.ViewModels
{
    internal class SettingViewModel : ViewModelBase
    {
        public SettingViewModel()
        {
            SelectPathCommand = ReactiveCommand.Create<int>(SelectFolder);
        }
        private async void SelectFolder(int option)
        {
            var dialog = new FolderPickerOpenOptions
            {
                AllowMultiple = false
            };
            switch (option)
            {
                case 1:
                    dialog.Title = "选择游戏路径";
                    break;
                case 2:
                    dialog.Title = "选择地图路径";
                    break;
                case 3:
                    dialog.Title = "选择Steam路径";
                    break;
                case 4:
                    dialog.Title = "选择下载路径";
                    break;
            }
            var folder = await (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!.StorageProvider.OpenFolderPickerAsync(dialog);
            if (folder.Count > 0)
            {
                string path;
                switch (option)
                {
                    case 1:
                        path = folder[0].Path.ToString();
                        path = path.Substring(8, path.Length - 8);
                        if (File.Exists($"{path}/GettingOverIt.exe"))
                        {
                            GamePath = path;
                            LevelPath = $"{path}/Levels";
                        }
                        else
                        {
                            var messageBox = new MessageBoxWindow();
                            var messageBoxWindowViewModel = new MessageBoxWindowViewModel();
                            messageBoxWindowViewModel.Message = "没有找到GettingOverIt.exe，是不是找错路径了喵。";
                            messageBox.DataContext = messageBoxWindowViewModel;
                            await messageBox.ShowDialog((Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!);
                        }
                        return;
                    case 2:
                        path = folder[0].Path.ToString();
                        path = path.Substring(8, path.Length - 8);
                        LevelPath = path;
                        return;
                    case 3:
                        path = folder[0].Path.ToString();
                        path = path.Substring(8, path.Length - 8);
                        if (File.Exists($"{path}/steam.exe"))
                        {
                            SteamPath = path;
                        }
                        else
                        {
                            var messageBox = new MessageBoxWindow();
                            var messageBoxWindowViewModel = new MessageBoxWindowViewModel();
                            messageBoxWindowViewModel.Message = "没有找到steam.exe，是不是找错路径了喵。";
                            messageBox.DataContext = messageBoxWindowViewModel;
                            await messageBox.ShowDialog((Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!);
                        }
                        return;
                    case 4:
                        path = folder[0].Path.ToString();
                        path = path.Substring(8, path.Length - 8);
                        DownloadPath = path;
                        return;
                }
            }
        }
        public ReactiveCommand<int, Unit> SelectPathCommand { get; }
        public string GamePath
        {
            get => Setting.Instance.gamePath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.gamePath, value,"GamePath");
                Setting.Instance.Save();
            }
        }
        public string LevelPath
        {
            get => Setting.Instance.levelPath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.levelPath, value, "LevelPath");
                Setting.Instance.Save();
            }
        }
        public string SteamPath
        {
            get => Setting.Instance.steamPath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.steamPath, value, "SteamPath");
                Setting.Instance.Save();
            }
        }
        public string DownloadPath
        {
            get => Setting.Instance.downloadPath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.downloadPath, value, "DownloadPath");
                Setting.Instance.Save();
            }
        }
        public bool SaveMapZip
        {
            get => Setting.Instance.saveMapZip;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.Instance.saveMapZip, value, "SaveMapZip");
                Setting.Instance.Save();
            }
        }

    }
}
