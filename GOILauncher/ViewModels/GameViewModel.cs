using System;
using GOILauncher.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;

namespace GOILauncher.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public GameViewModel(SettingViewModel settingViewModel)
        {
            _settingViewModel = settingViewModel;
            settingViewModel.WhenAnyValue(x => x.GamePath)
                            .Subscribe(x => IsGamePathSelected = !string.IsNullOrEmpty(x));
            settingViewModel.WhenAnyValue(x => x.SteamPath)
                            .Subscribe(x => IsSteamPathSelected = !string.IsNullOrEmpty(x));
            GameInfo.Refreshed += () =>
            {
                this.RaisePropertyChanged(nameof(GameVersion));
                this.RaisePropertyChanged(nameof(ModpackVersion));
                this.RaisePropertyChanged(nameof(LevelLoaderVersion));
                this.RaisePropertyChanged(nameof(BepInExVersion));
            };
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
                    GameProcess = Process.Start($"{GamePath}/GettingOverIt.exe");
                    break;
                case 2:
                    if (IsGamePathSelected)
                    {
                        GameProcess = Process.Start($"{SteamPath}/steam.exe", "steam://rungameid/240720");
                    }
                    break;
            }
            if(GameProcess is not null)
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
        private readonly SettingViewModel _settingViewModel;
        private string? GamePath => _settingViewModel.GamePath;
        private string? SteamPath => _settingViewModel.SteamPath;
        [Reactive]
        public bool IsGamePathSelected { get; set; }
        [Reactive]
        public bool IsSteamPathSelected { get; set; }
        [Reactive]
        public bool GameLaunched { get; set; }

        private Process? GameProcess { get; set; }
        private GameInfo GameInfo { get; } = GameInfo.Instance;
        public string GameVersion => GameInfo.GameVersion;
        public string ModpackVersion => GameInfo.ModpackVersion;
        public string LevelLoaderVersion => GameInfo.LevelLoaderVersion;
        public string BepInExVersion => GameInfo.BepInExVersion;

    }
}







