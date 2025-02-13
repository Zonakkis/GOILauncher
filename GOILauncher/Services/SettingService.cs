using GOILauncher.Models;
using System;
using System.IO;
using GOILauncher.Helpers;

namespace GOILauncher.Services;

public static class SettingService
{
    public static Setting LoadSetting()
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

    public static void SaveSetting(Setting setting)
    {
        FileService.SaveAsJson(AppDomain.CurrentDomain.BaseDirectory, "Settings.json", setting);
    }
}