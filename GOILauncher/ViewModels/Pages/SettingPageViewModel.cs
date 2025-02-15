﻿using System;
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
using System.Diagnostics;
using System.Reactive.Linq;

namespace GOILauncher.ViewModels.Pages
{
    public class SettingPageViewModel : PageViewModelBase
    {

        public SettingPageViewModel(FileService fileService,SettingService settingService)
        {
            _fileService = fileService;
            Setting = settingService.Setting;
            Setting.WhenAnyValue(x => x.GamePath, x => x.LevelPath, x => x.SteamPath, x => x.DownloadPath, x => x.SaveMapZip, x => x.NightMode, x => x.PreviewQuality)
                .Skip(1)
                .Subscribe(_ => settingService.SaveSetting());

        }
        public async Task SelectGamePath()
        {
            var folder = await _fileService.OpenFolderPickerAsync("选择游戏路径");
            if (folder is not null)
            {
                var path = folder.Path.ToString()[8..];
                if (File.Exists($"{path}/GettingOverIt.exe"))
                {
                    Setting.GamePath = path;
                    Setting.LevelPath = $"{path}/Levels";
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
                Setting.LevelPath = path;
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
                    Setting.SteamPath = path;
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
                Setting.DownloadPath = path;
            }
        }
        private readonly FileService _fileService;
        public Setting Setting { get; }
    }
}
