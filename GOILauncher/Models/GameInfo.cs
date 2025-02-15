using GOILauncher.Helpers;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.IO;
using System.Reflection;

namespace GOILauncher.Models
{
    public class GameInfo : ReactiveObject
    {
        [Reactive]
        public string GameVersion { get; set; }
        [Reactive]
        public string ModpackVersion { get; set; }
        [Reactive]
        public string LevelLoaderVersion { get; set; }
        [Reactive]
        public string BepInExVersion { get; set; }
        public static GameInfo Instance => null;
    }
}
