using GOILauncher.Helpers;
using System;
using System.IO;
using System.Reflection;

namespace GOILauncher.Models
{
    public class GameInfo
    {
        public required string GameVersion { get; init; }
        public required string ModpackVersion { get; init; }
        public required string LevelLoaderVersion { get; init; }
        public required string BepInExVersion { get; init; }
        public static GameInfo Instance => null;
    }
}
