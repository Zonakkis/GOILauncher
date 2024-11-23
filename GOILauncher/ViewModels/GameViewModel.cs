using Avalonia.Controls.Shapes;
using Avalonia.OpenGL;
using GOILauncher.Helpers;
using GOILauncher.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

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
        public void LaunchGOI(int para)
        {
            switch (para)
            {
                case 1:
                    Process.Start($"{GamePath}/GettingOverIt.exe");
                    return;
                case 2:
                    if (SteamPath != "未选择（需要通过Steam启动游戏时才选择，否则可不选）")
                    {
                        Process.Start($"{GamePath}/steam.exe", "steam://rungameid/240720");
                    }
                    return;
            }
        }

        private Setting Setting { get; } = Setting.Instance;
        private string GamePath  => Setting.GamePath; 
        private string SteamPath => Setting.SteamPath;

        [Reactive]
        public bool SelectedGamePathNoteHide { get; set; }
        private GameInfo GameInfo { get; } = GameInfo.Instance;
        public string GameVersion => GameInfo.GameVersion;
        public string ModpackVersion => GameInfo.ModpackVersion;
        public string LevelLoaderVersion => GameInfo.LevelLoaderVersion;
        public string BepInExVersion => GameInfo.BepInExVersion;

    }
}







