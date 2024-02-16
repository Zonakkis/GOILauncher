using Avalonia.Controls.Shapes;
using Avalonia.OpenGL;
using GOILauncher.Helpers;
using GOILauncher.Models;
using ReactiveUI;
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
                        Process.Start($"{SteamPath}/steam.exe", "steam://rungameid/240720");
                    }
                    return;
            }
        }

        public string gamePath = "未选择";
        public string GamePath
        {
            get => gamePath;
            set
            {
                if (gamePath != value)
                {
                    this.RaiseAndSetIfChanged(ref gamePath, value, "GamePath");
                    SelectedGamePathNoteHide = true;
                    GameInfo.Instance.GetGameVersion(new FileInfo($"{GamePath}/GettingOverIt.exe").Length);
                    GameInfo.Instance.GetModpackandLevelLoaderVersion(GamePath);
                    GameInfo.Instance.GetBepinExVersion(GamePath);

                }
            }
        }
        public string SteamPath { get => Setting.Instance.steamPath; }

        private bool selectedGamePathNoteHide;
        public bool SelectedGamePathNoteHide
        {
            get => selectedGamePathNoteHide; set
            {
                this.RaiseAndSetIfChanged(ref selectedGamePathNoteHide, value, "UnSelectedNoteHide");
            }
        }
        public string GameVersion
        {
            get => GameInfo.Instance.GameVersion;
        }
        public string ModpackVersion
        {
            get => GameInfo.Instance.ModpackVersion;
        }
        public string LevelLoaderVersion
        {
            get => GameInfo.Instance.LevelLoaderVersion;
        }
        public string BepInExVersion
        {
            get => GameInfo.Instance.BepInExVersion;
        }

    }
}







