using System;
using GOILauncher.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using ReactiveUI;
using GOILauncher.Services;
using Downloader;
using System.Reactive;
using GOILauncher.UI;
using GOILauncher.Services.LeanCloud;
using Avalonia.Threading;

namespace GOILauncher.ViewModels.Pages
{
    public class ModPageViewModel : PageViewModelBase
    {
        public ModPageViewModel(GameService gameService, AppService appService,
            DownloadService downloadService,LeanCloudService leanCloudService)
        {
            _gameInfo = gameService.GameInfo;
            _gameService = gameService;
            _leanCloudService = leanCloudService;
            _downloadService = downloadService;
            Setting = appService.Setting;
            DownloadCommand = ReactiveCommand.CreateFromTask<Mod>(Download);
        }
        public override void Init()
        {
            Dispatcher.UIThread.InvokeAsync(GetModpacks);
            Dispatcher.UIThread.InvokeAsync(GetLevelLoaders);
            Dispatcher.UIThread.InvokeAsync(GetModpackandLevelLoaders);
            Dispatcher.UIThread.InvokeAsync(GetOtherMods);
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
                await FileService.ExtractZipAsync(
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
            foreach(var modpack in await _leanCloudService.GetMods("Modpack"))
            {
                modpack.DownloadCommand = DownloadCommand;
                Modpacks.Add(modpack);
            }
        }
        private async Task GetLevelLoaders()
        {
            foreach (var levelLoader in await _leanCloudService.GetMods("LevelLoader"))
            {
                levelLoader.DownloadCommand = DownloadCommand;
                LevelLoaders.Add(levelLoader);
            }
        }
        private async Task GetModpackandLevelLoaders()
        {
            foreach (var modpackandLevelLoader in await _leanCloudService.GetMods("ModpackandLevelLoader"))
            {
                modpackandLevelLoader.DownloadCommand = DownloadCommand;
                ModpackandLevelLoaders.Add(modpackandLevelLoader);
            }
        }
        private async Task GetOtherMods()
        {
            foreach (var otherMod in await _leanCloudService.GetMods("OtherMod"))
            {
                otherMod.DownloadCommand = DownloadCommand;
                OtherMods.Add(otherMod);
            }
        }
        private readonly GameInfo _gameInfo;
        public Setting Setting { get; }
        private readonly LeanCloudService _leanCloudService;
        private readonly DownloadService _downloadService;
        private readonly GameService _gameService;
        public ObservableCollection<Mod> Modpacks { get; set; } = [];
        public ObservableCollection<Mod> LevelLoaders { get; set; } = [];
        public ObservableCollection<Mod> ModpackandLevelLoaders { get; set; } = [];
        public ObservableCollection<Mod> OtherMods { get; set; } = [];
        private ReactiveCommand<Mod, Unit> DownloadCommand { get; }

    }
}
