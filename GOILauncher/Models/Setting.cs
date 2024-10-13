using Avalonia;
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

        public Setting()
        {
            gamePath = "未选择";
            levelPath = "未选择（选择游戏路径后自动选择，也可手动更改）";
            steamPath = "未选择（需要通过Steam启动游戏时才选择，否则可不选）";
            downloadPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}Download";
            saveMapZip = false;
            previewQuality = 40;
        }




        public string gamePath;
        public string levelPath;
        public string steamPath;
        public string downloadPath;
        public bool saveMapZip;
        public bool nightMode;
        public int previewQuality;

        public static Setting Instance = new();

        public static void Load(string path)
        {
            Instance = StorageHelper.LoadJSON<Setting>(path, "Settings.json");
            if(Instance.nightMode)
            {
                Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;
            }
        }
        public void Save()
        {
            StorageHelper.SaveJSON(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json", this, true);
        }
    }
}
