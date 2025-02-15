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
using GOILauncher.ViewModels.Models;
using Downloader;
using System.Xml.Linq;
using System.Reactive;
using GOILauncher.UI;

namespace GOILauncher.ViewModels.Pages
{
    public class ModPageViewModel : PageViewModelBase
    {
        public ModPageViewModel(GamePageViewModel gamePageViewModel, SettingPageViewModel settingPageViewModel, DownloadService downloadService)
        {
            _gamePageViewModel = gamePageViewModel;
            _downloadService = downloadService;
            Setting = settingPageViewModel;
            DownloadCommand = ReactiveCommand.CreateFromTask<ModViewModel>(Download);
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

        private async Task Download(ModViewModel mod)
        {
            if(!mod.TargetGameVersion.Contains(_gamePageViewModel.GameVersion))
            {
                NotificationManager.ShowContentDialog("提示", $"版本不匹配！\n目标版本：{mod.TargetGameVersionString}\n当前版本：{_gamePageViewModel.GameVersion}");
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
            _gamePageViewModel.GetGameInfo(Setting.GamePath!);
            NotificationManager.ShowContentDialog("提示", $"已经安装{mod.Name} {mod.Build}！");
        }

        private async Task GetModpacks()
        {
            Modpacks = new ObservableCollection<ModViewModel>(
                            (await LeanCloudService.GetMods("Modpack"))
                            .Select(mod => new ModViewModel(mod)));
        }
        private async Task GetLevelLoaders()
        {
            LevelLoaders = new ObservableCollection<ModViewModel>(
                            (await LeanCloudService.GetMods("LevelLoader"))
                            .Select(mod => new ModViewModel(mod)));
        }
        private async Task GetModpackandLevelLoaders()
        {
            ModpackandLevelLoaders = new ObservableCollection<ModViewModel>(
                            (await LeanCloudService.GetMods("ModpackandLevelLoader"))
                            .Select(mod => new ModViewModel(mod)));
        }
        private async Task GetOtherMods()
        {
            OtherMods = new ObservableCollection<ModViewModel>(
                            (await LeanCloudService.GetMods("OtherMod"))
                            .Select(mod => new ModViewModel(mod)));
        }
        private readonly GamePageViewModel _gamePageViewModel;
        public SettingPageViewModel Setting { get; }
        private readonly DownloadService _downloadService;
        [Reactive]
        public ObservableCollection<ModViewModel> Modpacks { get; set; } = [];
        [Reactive]
        public ObservableCollection<ModViewModel> LevelLoaders { get; set; } = [];
        [Reactive]
        public ObservableCollection<ModViewModel> ModpackandLevelLoaders { get; set; } = [];
        [Reactive]
        public ObservableCollection<ModViewModel> OtherMods { get; set; } = [];

        public ReactiveCommand<ModViewModel,Unit> DownloadCommand { get; }

    }
}
