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
        public override void Init()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public override void OnSelectedViewChanged()
        {
            if (!SelectedGamePathNoteHide && Setting.Instance.gamePath != "未选择")
            {
                SelectedGamePathNoteHide = true;
                Task.Run(() =>
                {
                    GameInfo.Instance.GetGameVersion(new FileInfo($"{Setting.Instance.gamePath}/GettingOverIt.exe").Length);
                    GameInfo.Instance.GetModpackandLevelLoaderVersion(Setting.Instance.gamePath);
                    GameInfo.Instance.GetBepinExVersion(Setting.Instance.gamePath);
                });
            }
        }
        public void LaunchGOI(int para)
        {
            switch (para)
            {
                case 1:
                    Process.Start($"{Setting.Instance.gamePath}/GettingOverIt.exe");
                    return;
                case 2:
                    if (SteamPath != "未选择（需要通过Steam启动游戏时才选择，否则可不选）")
                    {
                        Process.Start($"{Setting.Instance.gamePath}/steam.exe", "steam://rungameid/240720");
                    }
                    return;
            }
        }
        public string SteamPath { get => Setting.Instance.steamPath; }

        private bool selectedGamePathNoteHide;
        public bool SelectedGamePathNoteHide
        {
            get => selectedGamePathNoteHide; set
            {
                this.RaiseAndSetIfChanged(ref selectedGamePathNoteHide, value, nameof(SelectedGamePathNoteHide));
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







