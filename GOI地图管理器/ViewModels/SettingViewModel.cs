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
            if (File.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                Setting = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
            }
            else
            {
                Setting = new Setting();
            }
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
                }
            }
        }
        public ReactiveCommand<int, Unit> SelectPathCommand { get; }
        public string GamePath
        {
            get => Setting.gamePath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.gamePath, value,"GamePath");
                Setting.Save();
            }
        }
        public string LevelPath
        {
            get => Setting.levelPath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.levelPath, value, "LevelPath");
                Setting.Save();
            }
        }

        public string SteamPath
        {
            get => Setting.steamPath;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.steamPath, value, "SteamPath");
                Setting.Save();
            }
        }

        public Setting setting;

        public Setting Setting
        {
            get => setting;
            set => this.RaiseAndSetIfChanged(ref setting, value, "Setting");
        }
    }
}
