using Avalonia;
using GOILauncher.Helpers;
using ReactiveUI;
using System;
using System.IO;
using ReactiveUI.Fody.Helpers;
using GOILauncher.Services;
using LC.Newtonsoft.Json;

namespace GOILauncher.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Setting : ReactiveObject
    {
        public Setting()
        {
            this.WhenAnyValue(x => x.GamePath)
                .Subscribe(x => IsGamePathSelected = !string.IsNullOrEmpty(x));
            this.WhenAnyValue(x => x.LevelPath)
                .Subscribe(x => IsLevelPathSelected = !string.IsNullOrEmpty(x));
            this.WhenAnyValue(x => x.SteamPath)
                .Subscribe(x => IsSteamPathSelected = !string.IsNullOrEmpty(x));
        }

        [Reactive]
        public bool NightMode { get; set; }
        [Reactive]
        public bool SaveMapZip { get; set; }
        [Reactive]
        public string? DownloadPath { get; set; }
        [Reactive]
        public string? SteamPath { get; set; }
        [Reactive]
        public string? LevelPath { get; set; }
        [Reactive]
        public string? GamePath { get; set; }
        [Reactive]
        public int PreviewQuality { get; set; }
        [Reactive]
        [JsonIgnore]
        public bool IsGamePathSelected { get; set; }
        [Reactive]
        [JsonIgnore]
        public bool IsLevelPathSelected { get; set; }
        [Reactive]
        [JsonIgnore]
        public bool IsSteamPathSelected { get; set; }
    }
}
