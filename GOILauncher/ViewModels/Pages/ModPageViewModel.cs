using System;
using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using GOILauncher.Services;
using Downloader;
using System.Xml.Linq;
using System.Reactive;
using GOILauncher.UI;

namespace GOILauncher.ViewModels.Pages
{
    public class ModPageViewModel : PageViewModelBase
    {
        public ModPageViewModel(GameService gameService, SettingService settingService,
            DownloadService downloadService,NotificationManager notificationManager)
        {
            _gameInfo = gameService.GameInfo;
            _gameService = gameService;
            _downloadService = downloadService;
            Setting = settingService.Setting;
            DownloadCommand = ReactiveCommand.CreateFromTask<Mod>(Download);
        }
        public override void Init()
        {
            _ = GetModpacks();
            _ = GetLevelLoaders();
            _ = GetModpackandLevelLoaders();
            _ = GetOtherMods();
        }
        public override void OnSelectedViewChanged()
        {

        }

        private async Task Download(Mod mod)
        {
            try
            {
                if (!mod.TargetGameVersion.Contains(_gameInfo.GameVersion))
                {
                    NotificationManager.ShowContentDialog("提示", $"版本不匹配！\n目标版本：{mod.TargetGameVersionString}\n当前版本：{_gameInfo.GameVersion}");
                    return;
                }
                _downloadService.DownloadStarted += mod.OnDownloadStarted;
                _downloadService.DownloadProgressChanged += mod.OnDownloadProgressChanged;
                _downloadService.DownloadFileCompleted += mod.OnDownloadCompleted;
                await _downloadService.DownloadFileTaskAsync(mod.Url,
                    Path.Combine(Setting.DownloadPath!, $"{mod.Name} {mod.Build}.zip"));
                _downloadService.DownloadStarted -= mod.OnDownloadStarted;
                _downloadService.DownloadProgressChanged -= mod.OnDownloadProgressChanged;
                _downloadService.DownloadFileCompleted -= mod.OnDownloadCompleted;
                FileService.ExtractZip(
                    Path.Combine(Setting.DownloadPath!, $"{mod.Name} {mod.Build}.zip"),
                    Setting.GamePath!,
                    !Setting.SaveMapZip);
                mod.IsExtracting = false;
                _gameService.GetModpackandLevelLoaderVersion();
                NotificationManager.ShowContentDialog("提示", $"已经安装{mod.Name} {mod.Build}！");
            }
            catch (Exception e)
            {
                NotificationManager.ShowContentDialog($"下载{mod.Name} {mod.Build}失败", e.Message);
            }
        }

        private async Task GetModpacks()
        {
            Modpacks = new ObservableCollection<Mod>(await LeanCloudService.GetMods("Modpack"));
        }
        private async Task GetLevelLoaders()
        {
            LevelLoaders = new ObservableCollection<Mod>(await LeanCloudService.GetMods("LevelLoader"));
        }
        private async Task GetModpackandLevelLoaders()
        {
            ModpackandLevelLoaders = new ObservableCollection<Mod>(await LeanCloudService.GetMods("ModpackandLevelLoader"));
        }
        private async Task GetOtherMods()
        {
            OtherMods = new ObservableCollection<Mod>(await LeanCloudService.GetMods("OtherMod"));
        }
        private readonly GameInfo _gameInfo;
        public Setting Setting { get; }
        private readonly DownloadService _downloadService;
        private readonly GameService _gameService;
        private readonly NotificationManager _notificationManager;
        [Reactive]
        public ObservableCollection<Mod> Modpacks { get; set; } = [];
        [Reactive]
        public ObservableCollection<Mod> LevelLoaders { get; set; } = [];
        [Reactive]
        public ObservableCollection<Mod> ModpackandLevelLoaders { get; set; } = [];
        [Reactive]
        public ObservableCollection<Mod> OtherMods { get; set; } = [];

        public ReactiveCommand<Mod, Unit> DownloadCommand { get; }

    }
}
