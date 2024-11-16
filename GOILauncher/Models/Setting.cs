using Avalonia;
using Avalonia.Controls.Shapes;
using GOILauncher.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class Setting
    {
        private bool nightMode;
        private bool saveMapZip;
        private string downloadPath;
        private string steamPath;
        private string levelPath;
        private string gamePath;
        private int previewQuality;
        public event Action? GamePathChanged;
        public event Action? LevelPathChanged;

        static Setting()
        {
            if (File.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                Instance = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                if (Instance.NightMode)
                {
                    Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;
                }
            }
            else Instance = new()
            {
                GamePath = "未选择",
                LevelPath = "未选择（选择游戏路径后自动选择，也可手动更改）",
                SteamPath = "未选择（需要通过Steam启动游戏时才选择，否则可不选）",
                DownloadPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}Download",
                SaveMapZip = false,
                PreviewQuality = 40,
            };
        }
        public Setting()
        {
            gamePath = "未选择";
            levelPath = "未选择（选择游戏路径后自动选择，也可手动更改）";
            steamPath = "未选择（需要通过Steam启动游戏时才选择，否则可不选）";
            downloadPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}Download";
            saveMapZip = false;
            previewQuality = 40;
        }

        public string GamePath
        {
            get => gamePath; 
            set
            {
                if(gamePath != value)
                {
                    gamePath = value;
                    Save();
                    GamePathChanged?.Invoke();
                }

            }
        }
        public string LevelPath
        {
            get => levelPath; 
            set
            {
                if(levelPath != value)
                {
                    levelPath = value;
                    Save();
                    LevelPathChanged?.Invoke();
                }
            }
        }
        public string SteamPath
        {
            get => steamPath; 
            set
            {
                steamPath = value;
                Save();
            }
        }
        public string DownloadPath
        {
            get => downloadPath;
            set
            {
                downloadPath = value;
                Save();
            }
        }
        public bool SaveMapZip
        {
            get => saveMapZip; 
            set
            {
                saveMapZip = value;
                Save();
            }
        }
        public bool NightMode
        {
            get => nightMode;
            set
            {
                nightMode = value;
                Save();
                Application.Current!.RequestedThemeVariant = value ? Avalonia.Styling.ThemeVariant.Dark : Avalonia.Styling.ThemeVariant.Light;
            }
        }
        public int PreviewQuality
        {
            get => previewQuality; set
            {
                previewQuality = value; 
                Save();
            }
        }
        public static Setting Instance { get; private set; }
        public void Save()
        {
            StorageHelper.SaveJSON(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json", this, true);
        }
    }
}
