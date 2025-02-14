using System;
using GOILauncher.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using GOILauncher.Services;

namespace GOILauncher.ViewModels.Pages
{
    public class GamePageViewModel : PageViewModelBase
    {
        public GamePageViewModel(SettingPageViewModel settingPageViewModel, GameService gameService)
        {
            SettingPage = settingPageViewModel;
            settingPageViewModel.WhenAnyValue(x => x.GamePath)
                            .Subscribe(gamePath =>
                            {
                                var gameInfo = gameService.GetGameInfo(gamePath!);
                                GameVersion = gameInfo.GameVersion;
                                ModpackVersion = gameInfo.ModpackVersion;
                                LevelLoaderVersion = gameInfo.LevelLoaderVersion;
                                BepInExVersion = gameInfo.BepInExVersion;
                            });
        }
        public override void Init()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public override void OnSelectedViewChanged()
        {

        }
        public void Launch(int para)
        {
            switch (para)
            {
                case 1:
                    GameProcess = Process.Start($"{SettingPage.GamePath}/GettingOverIt.exe");
                    break;
                case 2:
                    if (SettingPage.IsGamePathSelected)
                    {
                        GameProcess = Process.Start($"{SettingPage.SteamPath}/steam.exe", "steam://rungameid/240720");
                    }
                    break;
            }
            if (GameProcess is not null)
            {
                GameProcess.EnableRaisingEvents = true;
                GameProcess.Exited += (_, _) =>
                {
                    GameLaunched = false;
                    GameProcess = null;
                };
                GameLaunched = true;
            }
        }
        public void Close()
        {
            GameProcess?.Kill();
        }
        public SettingPageViewModel SettingPage { get; }
        [Reactive]
        public bool GameLaunched { get; set; }
        private Process? GameProcess { get; set; }
        [Reactive]
        public string GameVersion { get; set; }
        [Reactive]
        public string ModpackVersion { get; set; }
        [Reactive]
        public string LevelLoaderVersion { get; set; }
        [Reactive]
        public string BepInExVersion { get; set; }

    }
}







