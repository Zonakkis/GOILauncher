using GOILauncher.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Text;

namespace GOILauncher.ViewModels
{
    internal class GameViewModel : ViewModelBase
    {
        public GameViewModel()
        {
            SelectedGamePathNoteHide = !Setting.IsDefault(nameof(GamePath));
            GameInfo.Refreshed += () =>
            {
                this.RaisePropertyChanged(nameof(GameVersion));
                this.RaisePropertyChanged(nameof(ModpackVersion));
                this.RaisePropertyChanged(nameof(LevelLoaderVersion));
                this.RaisePropertyChanged(nameof(BepInExVersion));
            };
            if(SelectedGamePathNoteHide)GameInfo.Refresh(GamePath);
            Setting.GamePathChanged += () =>
            {
                SelectedGamePathNoteHide = true;
                GameInfo.Refresh(GamePath);
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
                    if (!Setting.IsDefault(nameof(SteamPath)))
                    {
                        GameProcess = Process.Start($"{GamePath}/steam.exe", "steam://rungameid/240720");
                    }
                    break;
            }
            if(GameProcess is not null)
            {
                GameProcess.EnableRaisingEvents = true;
                GameProcess.Exited += (sender, e) =>
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
        private Setting Setting { get; } = Setting.Instance;
        private string GamePath  => Setting.GamePath; 
        private string SteamPath => Setting.SteamPath;
        [Reactive]
        public bool SelectedGamePathNoteHide { get; set; }
        [Reactive]
        public bool GameLaunched { get; set; }
        public Process? GameProcess { get; set; }
        private GameInfo GameInfo { get; } = GameInfo.Instance;
        public string GameVersion => GameInfo.GameVersion;
        public string ModpackVersion => GameInfo.ModpackVersion;
        public string LevelLoaderVersion => GameInfo.LevelLoaderVersion;
        public string BepInExVersion => GameInfo.BepInExVersion;

    }
}







