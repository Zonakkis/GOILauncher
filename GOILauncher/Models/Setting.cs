using Avalonia;
using GOILauncher.Helpers;
using System;
using System.IO;

namespace GOILauncher.Models
{
    public class Setting
    {
        public bool NightMode { get; set; }
        public bool SaveMapZip { get; set; }
        public string? DownloadPath { get; set; }
        public string? SteamPath { get; set; }
        public string? LevelPath { get; set; }
        public string? GamePath { get; set; }
        public int PreviewQuality { get; set; }
    }
}
