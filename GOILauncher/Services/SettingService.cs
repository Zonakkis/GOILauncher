using GOILauncher.Models;
using System;
using System.IO;
using GOILauncher.Helpers;

namespace GOILauncher.Services;

public class SettingService
{
    public SettingService()
    {
        Setting = LoadSetting();
    }
    public Setting LoadSetting()
    {
        if (File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
        {
            return FileService.LoadFromJson<Setting>(AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
        }
        return new Setting
        {
            DownloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download"),
            PreviewQuality = 40
        };
    }

    public void SaveSetting()
    {
        FileService.SaveAsJson(AppDomain.CurrentDomain.BaseDirectory, "Settings.json", Setting);
    }
    public Setting Setting { get; }
}